using Common;
using Quartz;
using System;

namespace Service
{
    public class JobDetail : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            int Id = context.JobDetail.JobDataMap.GetInt("Id");
            QuartzTask task = TaskSchedulerFactory.GetScheduler().GetTask(Id);

            if (task == null)
            {
                throw new ArgumentException("Not found task ：" + task.Name);
            }


            IQuartzTaskService quartzTaskService = DIContainer.Resolve<IQuartzTaskService>();

            task.IsRunning = true;
            DateTime lastStart = DateTime.Now;

            try
            {
                ITask excuteTask = (ITask)Activator.CreateInstance(Type.GetType(task.ClassType));
                excuteTask.Execute(task);
                task.LastIsSuccess = true;
            }
            catch (Exception)
            {
                task.LastIsSuccess = false;
            }

            task.IsRunning = false;

            task.LastStart = lastStart;
            if (context.NextFireTimeUtc.HasValue)
                task.NextStart = context.NextFireTimeUtc.Value.LocalDateTime;
            else
                task.NextStart = null;

            task.LastEnd = DateTime.Now;
            DIContainer.Resolve<ITaskScheduler>().SaveTaskStatus(task);
        }
    }
}
