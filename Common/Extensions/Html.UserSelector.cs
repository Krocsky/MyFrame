using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Common.Extensions
{
    public static partial class HtmlExtensions
    {
        /// <summary>
        /// 学生选择器控件
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString UserSelector(this HtmlHelper html, string name, string defaultValue = null, int userId = 0 )
        {
            return html.EditorForModel("UserSelector", new { name = name, defaultValue = defaultValue, userId = userId });
        }
    }
}

