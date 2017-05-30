using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 自运行任务接口
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 执行任务的方法
        /// </summary>
        /// <param name=" task">任务配置状态信息</param>
        void Execute(QuartzTask task = null);
    }
}
