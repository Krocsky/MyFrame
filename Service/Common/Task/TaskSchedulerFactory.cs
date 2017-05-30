using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 任务工厂
    /// </summary>
    /// <remarks>
    /// 用于获取TaskScheduler
    /// </remarks>
    public static class TaskSchedulerFactory
    {
        private static ITaskScheduler _scheduler = null;

        /// <summary>
        /// 获取任务调度器
        /// </summary>
        /// <returns></returns>
        public static ITaskScheduler GetScheduler()
        {
            if (_scheduler == null)
            {
                _scheduler = DIContainer.Resolve<ITaskScheduler>();
            }
            return _scheduler;
        }
    }
}
