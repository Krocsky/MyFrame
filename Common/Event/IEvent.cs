using System;
using System.Linq;

namespace Common
{
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// 执行事件1
        /// </summary>
        void HandleEvent();
    }
}
