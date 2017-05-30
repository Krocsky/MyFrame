using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.Web;

namespace Common.Utilities
{
    /// <summary>
    /// Html工具类
    /// </summary>
    public class HtmlHelper
    {
        /// <summary>
        /// 移除html内的Elemtnts/Attributes及&amp;nbsp;，超过charLimit个字符进行截断
        /// </summary>
        /// <param name="rawHtml">待截字的html字符串</param>
        /// <param name="charLimit">最多允许返回的字符数</param>
        public static string TrimHtml(string rawHtml, int charLimit)
        {
            if (string.IsNullOrEmpty(rawHtml))
                return string.Empty;

            string nohtml = StripHtml(rawHtml, true, false);
            nohtml = StripBBTags(nohtml);

            if (charLimit <= 0 || charLimit >= nohtml.Length)
                return nohtml;
            else
                return StringHelper.Trim(nohtml, charLimit);
        }

        /// <summary>
        /// 移除Html标签
        /// </summary>
        /// <param name="rawString">待处理字符串</param>
        /// <param name="removeHtmlEntities">是否移除Html实体</param>
        /// <param name="enableMultiLine">是否保留换行符（<p/><br/>会转换成换行符）</param>
        /// <returns>返回处理后的字符串</returns>
        public static string StripHtml(string rawString, bool removeHtmlEntities, bool enableMultiLine)
        {
            string result = rawString;
            if (enableMultiLine)
            {
                result = Regex.Replace(result, "</p(?:\\s*)>(?:\\s*)<p(?:\\s*)>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                result = Regex.Replace(result, "<br(?:\\s*)/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
            result = result.Replace("\"", "''");
            if (removeHtmlEntities)
            {
                //StripEntities removes the HTML Entities 
                result = Regex.Replace(result, "&[^;]*;", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
            return Regex.Replace(result, "<[^>]+>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }


        /// <summary>
        /// 移除Html用于内容预览
        /// </summary>
        /// <remarks>
        /// 将br、p替换为\n，“'”替换为对应Html实体，并过滤所有Html、Xml、UBB标签
        /// </remarks>
        /// <param name="rawString">用于预览的文本</param>
        /// <returns>返回移除换行及html、ubb标签的字符串</returns>
        public static string StripForPreview(string rawString)
        {
            string tempString;

            tempString = rawString.Replace("<br>", "\n");
            tempString = tempString.Replace("<br/>", "\n");
            tempString = tempString.Replace("<br />", "\n");
            tempString = tempString.Replace("<p>", "\n");
            tempString = tempString.Replace("'", "&#39;");

            tempString = StripHtml(tempString, false, false);
            tempString = StripBBTags(tempString);

            return tempString;
        }

        /// <summary>
        /// 清除UBB标签
        /// </summary>
        /// <param name="content">待处理的字符串</param>
        /// <remarks>处理后的字符串</remarks>
        public static string StripBBTags(string content)
        {
            return Regex.Replace(content, @"\[[^\]]*?\]", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 移除script标签
        /// Helper function used to ensure we don't inject script into the db.
        /// </summary>
        /// <remarks>
        /// 移除&lt;script&gt;及javascript:
        /// </remarks>
        /// <param name="rawString">待处理的字符串</param>
        /// <remarks>处理后的字符串</remarks>
        public static string StripScriptTags(string rawString)
        {
            // Perform RegEx
            rawString = Regex.Replace(rawString, "<script((.|\n)*?)</script>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            rawString = rawString.Replace("\"javascript:", "\"");

            return rawString;
        }
    }
}
