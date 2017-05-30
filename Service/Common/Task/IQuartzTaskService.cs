using Common;
using Quartz;
using System;
using System.Linq;

namespace Service
{
    /// <summary>
    /// 自运行任务业务逻辑接口
    /// </summary>
    public interface IQuartzTaskService
    {
        IQueryable<QuartzTask> QuartzTasks { get; }

        /// <summary>
        /// 获取单个任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        QuartzTask FindById(int id);

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="task"></param>
        void Create(QuartzTask task);

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="task"></param>
        void Update(QuartzTask task);

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="task"></param>
        void Delete(QuartzTask task);
    }
}
