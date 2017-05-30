using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Common.Utilities;
using System.IO;
using log4net.Config;

namespace Common
{
    /// <summary>
    /// Log4Net实现类
    /// </summary>
    public class Log4Net : ILogger
    {
        private readonly ILog log;

        public Log4Net()
        {
            string configFilename = WebHelper.ResolveUrl("~/Config/log4net.config");
            FileInfo configFileInfo = new FileInfo(WebHelper.GetPhysicalFilePath(configFilename));
            if (configFileInfo.Exists)
            {
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }
            log = LogManager.GetLogger("log4net");
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message"></param>
        public void Debug(object message)
        {
            log.Debug(message);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Debug(object message, Exception ex)
        {
            log.Debug(message, ex);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        public void Error(object message)
        {
            log.Error(message);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Error(object message, Exception ex)
        {
            log.Error(message, ex);
        }

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="message"></param>
        public void Fatal(object message)
        {
            log.Fatal(message);
        }

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Fatal(object message, Exception ex)
        {
            log.Fatal(message, ex);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        public void Info(object message)
        {
            log.Info(message);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Info(object message, Exception ex)
        {
            log.Info(message, ex);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        public void Warn(object message)
        {
            log.Warn(message);
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Warn(object message, Exception ex)
        {
            log.Warn(message, ex);
        }
    }
}
