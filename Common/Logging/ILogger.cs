using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message"></param>
        void Debug(object message);

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Debug(object message, Exception ex);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        void Error(object message);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Error(object message, Exception ex);

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="message"></param>
        void Fatal(object message);

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Fatal(object message, Exception ex);

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        void Info(object message);

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Info(object message, Exception ex);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        void Warn(object message);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Warn(object message, Exception ex);

    }
}
