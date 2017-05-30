using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Common.Extensions
{
    public static partial class HtmlExtensions
    {
        /// <summary>
        /// 获取参数列表
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="parentId">父级节点Id</param>
        /// <returns></returns>
        public static MvcHtmlString GetParameterList(this HtmlHelper html, string name, TenantType tenantType, int parentId)
        {
            TagBuilder builder = new TagBuilder("select");
            Dictionary<string, object> data = new Dictionary<string, object>();
            Dictionary<string, object> result = new Dictionary<string, object>();
            Dictionary<string, object> extradata = new Dictionary<string, object>();


            data.Add("data", Json.Encode(result));
            builder.MergeAttributes(data);
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("id", name);

            return MvcHtmlString.Create(builder.ToString());
        }

    }
}

