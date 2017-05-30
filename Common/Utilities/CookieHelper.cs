using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;

namespace Common.Utilities
{
    /// <summary>
    /// Cookie工具类
    /// </summary>
    public static class CookieHelper
    {
        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="cookiename">key</param>
        /// <returns></returns>
        public static string Get(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            string str = string.Empty;
            if (cookie != null)
            {
                str = EncryptionHelper.Base64_Decode(cookie.Value);
            }
            return str;
        }

        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cookiename"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Get<T>(string cookiename, T defaultValue)
        {
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];

                if (cookie != null)
                {
                    object value = EncryptionHelper.Base64_Decode(cookie.Value);
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
                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
            
        }

        /// <summary>
        /// 添加一个Cookie（24小时过期）
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="cookievalue"></param>
        public static void Set(string cookiename, string cookievalue)
        {
            Set(cookiename, cookievalue, DateTime.Now.AddDays(1.0));
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookiename">cookie名</param>
        /// <param name="cookievalue">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void Set(string cookiename, string cookievalue, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookiename)
            {
                Value = EncryptionHelper.Base64_Encode(cookievalue),
                Expires = expires
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        public static void Clear(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}
