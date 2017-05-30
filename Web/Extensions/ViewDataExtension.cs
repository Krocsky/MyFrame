using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web
{
    /// <summary>
    /// ViewData扩展方法
    /// </summary>
    public static class ViewDataExtension
    {
        /// <summary>
        /// 从ViewData获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Get<T>(this IDictionary<string, object> dictionary, string key, T defaultValue)
        {
            if (dictionary.ContainsKey(key))
            {
                object value;
                dictionary.TryGetValue(key, out value);
                if (value != null)
                {
                    Type tType = typeof(T);
                    if (tType.IsInterface || (tType.IsClass && tType != typeof(string)))
                    {
                        if (value is T)
                            return (T)value;
                    }
                    else if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return (T)Convert.ChangeType(value, Nullable.GetUnderlyingType(tType));
                    }
                    else if (tType.IsEnum)
                    {
                        return (T)Enum.Parse(tType, value.ToString());
                    }
                    else
                    {
                        return (T)Convert.ChangeType(value, tType);
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// 从ViewData获取String类型
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(this IDictionary<string, object> dictionary, string key)
        {
            return Get<string>(dictionary, key, string.Empty);
        }

        /// <summary>
        /// 从ViewData获取Bool类型
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBool(this IDictionary<string, object> dictionary, string key)
        {
            return Get<bool>(dictionary, key, false);
        }

        /// <summary>
        /// 从ViewData获取Int类型
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetInt(this IDictionary<string, object> dictionary, string key)
        {
            return Get<int>(dictionary, key, 0);
        }

        /// <summary>
        /// 从ViewData获取Double类型
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static double GetDouble(this IDictionary<string, object> dictionary, string key)
        {
            return Get<double>(dictionary, key, 0);
        }
    }
}