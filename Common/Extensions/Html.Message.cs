using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Common.Extensions
{
    public static partial class HtmlExtensions
    {
        /// <summary>
        /// 输出提示信息
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static MvcHtmlString Message(this HtmlHelper html)
        {
            object obj;
            bool result = html.ViewData.TryGetValue("errorMessage", out obj);
            if (result)
            {
                TagBuilder span = new TagBuilder("span");
                span.AddCssClass("glyphicon glyphicon-exclamation-sign");
                span.Attributes.Add("role", "alert");
                TagBuilder div = new TagBuilder("div");
                div.Attributes.Add("role", "alert");
                div.AddCssClass("alert alert-danger");
                div.InnerHtml = span.ToString() + obj.ToString();
                return MvcHtmlString.Create(div.ToString());
            }
            else if (html.ViewData.TryGetValue("message", out obj))
            {
                TagBuilder span = new TagBuilder("span");
                span.AddCssClass("glyphicon glyphicon-exclamation-sign");
                span.Attributes.Add("role", "alert");
                TagBuilder div = new TagBuilder("div");
                div.Attributes.Add("role", "alert");
                div.AddCssClass("alert alert-success");
                div.InnerHtml = span.ToString() + obj.ToString();
                return MvcHtmlString.Create(div.ToString());
            }
            return MvcHtmlString.Empty;
        }

    }
}

