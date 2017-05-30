using Common;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// 自运行任务控制器
    /// </summary>
    public class QuartzTaskScheduler : ITaskScheduler
    {
        private static List<QuartzTask> _tasks;
        private readonly IScheduler sched;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QuartzTaskScheduler()
        {
            sched = StdSchedulerFactory.GetDefaultScheduler();
        }

        /// <summary>
        /// 开始任务
        /// </summary>
        public void Start()
        {
            var taskService = DIContainer.Resolve<IQuartzTaskService>();

            if (_tasks == null)
                _tasks = taskService.QuartzTasks.ToList();

            if (_tasks.Count == 0)
                return;

            var taskScheduler = DIContainer.Resolve<ITaskScheduler>();

            foreach (var task in _tasks)
            {
                if (!task.Enabled)
                    continue;

                Type type = Type.GetType(task.ClassType);
                if (type == null)
                    continue;

                string triggerName = type.Name + "_trigger";

                IJobDetail job = JobBuilder.Create(typeof(JobDetail))
                                           .WithIdentity(type.Name)
                                           .Build();

                job.JobDataMap.Add(new KeyValuePair<string, object>("Id", task.Id));


                TriggerBuilder tb = TriggerBuilder.Create()
                                                  .WithIdentity(triggerName)
                                                  .WithCronSchedule(task.TaskRule);

                if(task.NextStart.HasValue)
                {
                    tb.StartAt(new DateTimeOffset(task.NextStart.Value));
                }
                else if(task.StartDate > DateTime.MinValue)
                {
                    tb.StartAt(new DateTimeOffset(task.StartDate));
                }

                if (task.EndDate > task.StartDate)
                {
                    tb.EndAt(task.EndDate);
                }

                ICronTrigger trigger = (ICronTrigger)tb.Build();
                DateTime nextStart = sched.ScheduleJob(job, trigger).LocalDateTime;

                if (task.NextStart == null || task.NextStart < nextStart)
                {
                    task.NextStart = nextStart;
                    taskScheduler.SaveTaskStatus(task);
                }
            }

            sched.Start();
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        public void Stop()
        {
            _tasks = null;
            sched.Standby();
            sched.Clear();
        }

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="task"></param>
        public void Update(QuartzTask task)
        {
            if (task == null)
                return;

            int index = _tasks.FindIndex(n => n.Id == task.Id);

            if (_tasks[index] == null)
                return;

            task.ClassType = _tasks[index].ClassType;
            task.LastEnd = _tasks[index].LastEnd;
            task.LastStart = _tasks[index].LastStart;
            task.LastIsSuccess = _tasks[index].LastIsSuccess;

            _tasks[index] = task;

            Type type = Type.GetType(task.ClassType);
            if (type == null)
            {
                return;
            }

            Remove(type.Name);

            if (!task.Enabled)
                return;

            string triggerName = type.Name + "_trigger";

            IJobDetail job = JobBuilder.Create(typeof(JobDetail))
                                       .WithIdentity(type.Name)
                                       .Build();

            job.JobDataMap.Add(new KeyValuePair<string, object>("Id", task.Id));


            TriggerBuilder tb = TriggerBuilder.Create()
                                              .WithIdentity(triggerName)
                                              .WithCronSchedule(task.TaskRule);

            if (task.NextStart.HasValue)
            {
                tb.StartAt(new DateTimeOffset(task.NextStart.Value));
            }
            else if (task.StartDate > DateTime.MinValue)
            {
                tb.StartAt(new DateTimeOffset(task.StartDate));
            }

            if (task.EndDate.HasValue && task.EndDate > task.StartDate)
            {
                tb.EndAt(task.EndDate);
            }

            ICronTrigger trigger = (ICronTrigger)tb.Build();

            DateTime nextStart = sched.ScheduleJob(job, trigger).LocalDateTime;
            if (task.NextStart.HasValue && task.NextStart < nextStart)
                task.NextStart = nextStart;
        }

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="task"></param>
        public void SaveTaskStatus(QuartzTask task)
        {
            if (task == null)
                return;

            var taskService = DIContainer.Resolve<IQuartzTaskService>();

            var t = taskService.FindById(task.Id);
            if(t!=null)
            {
                t.LastStart = task.LastStart;
                t.LastEnd = task.LastEnd;
                t.NextStart = task.NextStart;
                t.IsRunning = task.IsRunning;
                t.LastIsSuccess = task.LastIsSuccess;
                t.LastFailMessage = task.LastFailMessage;
                taskService.Update(t);
            }
        }

        /// <summary>
        /// 重启所有任务
        /// </summary>
        public void ResumeAll()
        {
            Stop();
            Start();
        }

        /// <summary>
        /// 获取单个任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QuartzTask GetTask(int id)
        {
            return _tasks.FirstOrDefault(n => n.Id == id);
        }

        /// <summary>
        /// 运行单个任务
        /// </summary>
        /// <param name="Id">任务Id</param>
        public void Run(int Id)
        {
            QuartzTask task = GetTask(Id);
            Run(task);
            SaveTaskStatus(task);
        }

        /// <summary>
        /// 运行单个任务
        /// </summary>
        /// <param name="task">要运行的任务</param>
        public void Run(QuartzTask task)
        {
            if (task == null)
                return;

            Type type = Type.GetType(task.ClassType);
            if (type == null)
            {
                return;
            }

            //任务重复运行检测
            if (task.IsRunning)
            {
                return;
            }

            ITask t = (ITask)Activator.CreateInstance(type);
            if (t != null)
            {
                task.IsRunning = true;
                DateTime lastStart = DateTime.Now;
                try
                {
                    t.Execute();
                    task.LastIsSuccess = true;
                    task.LastFailMessage = null;
                }
                catch (Exception ex)
                {
                    task.LastIsSuccess = false;
                    task.LastFailMessage = ex.Message;
                }
                task.IsRunning = false;
                task.LastStart = lastStart;
                task.LastEnd = DateTime.Now;
                SaveTaskStatus(task);
            }
        }

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="name">调度器中任务的名称</param>
        private void Remove(string name)
        {
            sched.DeleteJob(new JobKey(name));
        }
    }
}
