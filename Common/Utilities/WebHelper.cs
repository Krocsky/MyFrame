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
    /// 提供与Web请求时可使用的工具类:包括Url解析、Url/Html编码、获取IP地址、返回http状态码
    /// </summary>
    public static class WebHelper
    {
        public static readonly String HtmlNewLine = "<br />";

        #region Url & Path

        /// <summary>
        /// 将URL转换为在请求客户端可用的 URL（转换 ~/ 为绝对路径）
        /// </summary>
        /// <param name="relativeUrl">相对url</param>
        /// <returns>返回绝对路径</returns>
        public static string ResolveUrl(string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (relativeUrl.StartsWith("~/"))
            {
                string[] urlParts = relativeUrl.Split('?');
                string resultUrl = VirtualPathUtility.ToAbsolute(urlParts[0]);
                if (urlParts.Length > 1)
                    resultUrl += "?" + urlParts[1];

                return resultUrl;
            }

            return relativeUrl;
        }

        /// <summary>
        /// 获取带传输协议的完整的主机地址
        /// </summary> 
        /// <param name="uri">Uri</param>
        /// <returns>
        /// <para>返回带传输协议的完整的主机地址</para>
        ///     <example>https://www.spacebuilder.cn:8080</example>
        /// </returns>
        public static string HostPath(Uri uri)
        {
            if (uri == null)
                return string.Empty;

            string port = uri.IsDefaultPort ? string.Empty : (":" + Convert.ToString(uri.Port, CultureInfo.InvariantCulture));
            return uri.Scheme + Uri.SchemeDelimiter + uri.Host + port;
        }


        /// <summary>
        /// 获取物理文件路径
        /// </summary>
        /// <param name="filePath">
        ///     <remarks>
        ///         <para>filePath支持以下格式：</para>
        ///         <list type="bullet">
        ///         <item>~/abc/</item>
        ///         <item>c:\abc\</item>
        ///         <item>\\192.168.0.1\abc\</item>
        ///         </list>
        ///     </remarks>
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetPhysicalFilePath(string filePath)
        {
            string calculatedFilePath = null;

            // Make sure it isn't a drive reference like "c:\blah" or a UNC name like "\\machine\share"
            if ((filePath.IndexOf(":\\") != -1) || (filePath.IndexOf("\\\\") != -1))
                calculatedFilePath = filePath;
            else
            {
                if (HostingEnvironment.IsHosted)
                {
                    calculatedFilePath = HostingEnvironment.MapPath(filePath);
                }
                else
                {
                    filePath = filePath.Replace('/', Path.DirectorySeparatorChar).Replace("~", "");
                    calculatedFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
                }
            }
            return calculatedFilePath;
        }

        public static string GetCurrentUrl(bool needParam = true)
        {
            if (needParam)
                return HttpContext.Current.Request.Url.PathAndQuery;
            return HttpContext.Current.Request.Url.ToString();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savePath"></param>
        public static void Download(string url,string savePath)
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadFile(url, HttpContext.Current.Server.MapPath(savePath));
            }
            catch{}
        }

        /// <summary>
        /// 获取Url文件流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream GetStream(string url)
        {
            WebClient client = new WebClient();
            return client.OpenRead(url);
        }

        #endregion

        #region Encode/Decode

        /// <summary>
        /// html编码
        /// </summary>
        /// <remarks>
        /// <para>调用HttpUtility.HtmlEncode()，当前已知仅作如下转换：</para>
        /// <list type="bullet">
        ///     <item>&lt; = &amp;lt;</item>
        ///     <item>&gt; = &amp;gt;</item>
        ///     <item>&amp; = &amp;amp;</item>
        ///     <item>&quot; = &amp;quot;</item>
        /// </list>
        /// </remarks>
        /// <param name="rawContent">待编码的字符串</param>
        public static string HtmlEncode(String rawContent)
        {
            if (string.IsNullOrEmpty(rawContent))
                return rawContent;

            return HttpUtility.HtmlEncode(rawContent);
        }

        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="rawContent">待解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string HtmlDecode(String rawContent)
        {
            if (string.IsNullOrEmpty(rawContent))
                return rawContent;

            return HttpUtility.HtmlDecode(rawContent);
        }

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="urlToEncode">待编码的url字符串</param>
        /// <returns>编码后的url字符串</returns>
        public static string UrlEncode(string urlToEncode)
        {
            if (string.IsNullOrEmpty(urlToEncode))
                return urlToEncode;

            return HttpUtility.UrlEncode(urlToEncode).Replace("'", "%27");
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="urlToDecode">待解码的字符串</param>
        /// <returns>解码后的字符串</returns>
        public static string UrlDecode(string urlToDecode)
        {
            if (string.IsNullOrEmpty(urlToDecode))
                return urlToDecode;

            return HttpUtility.UrlDecode(urlToDecode);
        }

        #endregion

        #region IPAddress

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns>返回获取的ip地址</returns>
        public static string GetIP()
        {
            return GetIP(HttpContext.Current);
        }

        /// <summary>
        /// 透过代理获取真实IP
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>返回获取的ip地址</returns>
        public static string GetIP(HttpContext httpContext)
        {
            string result = string.Empty;
            if (httpContext == null)
                return result;

            // 透过代理取真实IP
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (result == "::1")
                result = "127.0.0.1";

            return result;
        }

        #endregion

        #region Return Response StatusCode

        /// <summary>
        /// 返回 StatusCode 404
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        public static void Return404(HttpContext httpContext)
        {
            ReturnStatusCode(httpContext, 404, null, false);
            if (httpContext != null)
            {
                httpContext.Response.SuppressContent = true;
                httpContext.Response.End();
            }
        }

        /// <summary>
        /// 返回 StatusCode 403
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        public static void Return403(HttpContext httpContext)
        {
            ReturnStatusCode(httpContext, 403, null, false);
            if (httpContext != null)
            {
                httpContext.Response.SuppressContent = true;
                httpContext.Response.End();
            }
        }

        /// <summary>
        /// 返回 StatusCode 304
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <param name="endResponse">是否终止HttpResponse</param>
        public static void Return304(HttpContext httpContext, bool endResponse = true)
        {
            ReturnStatusCode(httpContext, 304, "304 Not Modified", endResponse);
        }

        /// <summary>
        /// 返回http状态码
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <param name="statusCode">状态码</param>
        /// <param name="status">状态描述字符串</param>
        /// <param name="endResponse">是否终止HttpResponse</param>
        private static void ReturnStatusCode(HttpContext httpContext, int statusCode, string status, bool endResponse)
        {
            if (httpContext == null)
                return;

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = statusCode;

            if (!string.IsNullOrEmpty(status))
                httpContext.Response.Status = status;

            if (endResponse)
                httpContext.Response.End();
        }

        #endregion

        #region SetStatusCode

        /// <summary>
        /// 设置当前响应的状态码为指定值
        /// </summary>
        /// <param name="response">当前响应</param>
        public static void SetStatusCodeForError(HttpResponseBase response)
        {
            response.StatusCode = 300;
        }

        #endregion

        #region 替换url字符串

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }

            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }

        #endregion
    }
}
