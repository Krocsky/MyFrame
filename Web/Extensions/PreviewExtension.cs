using System;
using System.Linq;

namespace Web
{
    public static class PreviewExtension
    {
        /// <summary>   
        /// 将指定字符串按指定长度进行截取并加上指定的后缀  
        /// </summary>   
        /// <param name="oldStr"> 需要截断的字符串 </param>   
        /// <param name="maxLength"> 字符串的最大长度 </param>   
        /// <param name="endWith"> 超过长度的后缀 </param>
        /// <returns> 如果超过长度，返回截断后的新字符串加上后缀，否则，返回原字符串 </returns>   
        public static string StringTruncat(string oldStr, int maxLength, string endWith)
        {
            if (string.IsNullOrEmpty(oldStr))
                return oldStr + endWith;

            if (maxLength < 1)
                throw new Exception("返回的字符串长度必须大于[0] ");

            if (oldStr.Length > maxLength)
            {
                string strTmp = oldStr.Substring(0, maxLength);
                if (string.IsNullOrEmpty(endWith))
                    return strTmp;
                return strTmp + endWith;
            }
            return oldStr;
        }

        /// <summary>
        /// 格式化数字，保留2位小数，不四舍五入
        /// </summary>
        /// <returns></returns>
        public static string FormatDoublePara(double parameter)
        {
            var tempStr = parameter.ToString().Split('.');
            var afterPara = "";
            if (tempStr.Count() < 2)
            {
                return tempStr[0];
            }
            if (tempStr[1].Length >= 3)
            {
                afterPara = tempStr[1].Substring(0, 2);
            }
            else
            {
                afterPara = tempStr[1];
            }
            return tempStr[0] + "." + afterPara;
        }
    }
}