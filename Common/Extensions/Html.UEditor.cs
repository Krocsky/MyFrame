using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Common.Extensions
{
    public static partial class HtmlExtensions
    {
        /// <summary>
        /// 输出UEditor编辑器
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="typeId"></param>
        /// <param name="itemId"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString UEditor(this HtmlHelper htmlHelper, string name, int? typeId = null, long itemId = 0, string value = null, object htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("参数name不能为空", "args name is null");
            }

            TagBuilder builder = new TagBuilder("textarea");
            var data = new Dictionary<string, object>();
            if (typeId.HasValue)
                data.Add("typeId", typeId);
            data.Add("itemId", itemId);
            builder.Attributes.Add("id", name);
            builder.Attributes.Add("data", JsonConvert.SerializeObject(data));
            builder.Attributes.Add("plugin", "ueditor");
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            builder.InnerHtml = value ?? string.Empty;
            return MvcHtmlString.Create(builder.ToString());
        }

        /// <summary>
        /// 利用ViewModel输出UEditor编辑器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="typeId"></param>
        /// <param name="itemId"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString UEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int? typeId = null, long itemId = 0, Dictionary<string, object> htmlAttributes = null)
        {
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            if (htmlAttributes != null)
                attributes = new Dictionary<string, object>(htmlAttributes);

            var data = new Dictionary<string, object>();
            if (typeId.HasValue)
                data.Add("typeId", typeId);
            data.Add("itemId", itemId);
            attributes.Add("data", JsonConvert.SerializeObject(data));
            attributes.Add("plugin", "ueditor");
            return htmlHelper.TextAreaFor(expression, attributes);
        }
    }
}
