using System;
using System.Linq;
using Quartz;
using System.Collections.Generic;
using Quartz.Impl;
using Common;
using Data;

namespace Service
{
    /// <summary>
    /// 自运行任务业务逻辑
    /// </summary>
    public class QuartzTaskService : IQuartzTaskService
    {
        private readonly IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
        private readonly IRepository<QuartzTask> _quartzTaskRepository;
        private static List<QuartzTask> _tasks;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="quartzTaskRepository"></param>
        public QuartzTaskService(IRepository<QuartzTask> quartzTaskRepository)
        {
            _quartzTaskRepository = quartzTaskRepository;
            _tasks = _quartzTaskRepository.Table.ToList();
        }

        public IQueryable<QuartzTask> QuartzTasks
        {
            get
            {
                return _quartzTaskRepository.Table;
            }
        }

        /// <summary>
        /// 获取单个任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QuartzTask FindById(int id)
        {
            return _quartzTaskRepository.FirstOrDefault(id);
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="task"></param>
        public void Create(QuartzTask task)
        {
            _quartzTaskRepository.Insert(task);
        }

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="task"></param>
        public void Update(QuartzTask task)
        {
            _quartzTaskRepository.Update(task);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="task"></param>
        public void Delete(QuartzTask task)
        {
            _quartzTaskRepository.Delete(task);
        }
    }
}
