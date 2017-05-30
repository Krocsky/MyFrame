using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Common.Utilities
{
    /// <summary>
    /// web.config文件帮助类
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// 获取AppSetting值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAppSetting(string name)
        {
            var setting = ConfigurationManager.AppSettings[name];
            return setting ?? string.Empty;
        }

        /// <summary>
        /// 获取ConnectionString值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConnectionString(string name)
        {
            var setting = ConfigurationManager.ConnectionStrings[name];
            if (setting == null)
                return string.Empty;
            return setting.ConnectionString;
        }
    }
}
