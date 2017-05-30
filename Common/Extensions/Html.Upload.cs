using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Common.Extensions
{
    public static partial class HtmlExtensions
    {
        /// <summary>
        /// 上传文件控件
        /// </summary>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static MvcHtmlString Upload(this HtmlHelper html, string name, TenantType tenantType, int itemId = 0, UploadOptions options = null)
        {
            TagBuilder builder = new TagBuilder("input");
            Dictionary<string, object> data = new Dictionary<string, object>();
            Dictionary<string, object> result = new Dictionary<string, object>();
            Dictionary<string, object> extradata = new Dictionary<string, object>();

            extradata.Add("tenantType", tenantType);
            extradata.Add("itemId", itemId);

            result.Add("uploadExtraData", Json.Encode(extradata));
            result.Add("uploadUrl", new UrlHelper(html.ViewContext.RequestContext).Action("Upload", "Api"));
            result.Add("language", "zh");

            if (options != null)
            {
                if (!string.IsNullOrEmpty(options.AllowedFileExtensions))
                    result.Add("allowedFileExtensions", options.AllowedFileExtensions);

                if (options.MaxFileCount > 0)
                {
                    data.Add("multiple", true);
                    result.Add("maxFileCount", options.MaxFileCount);
                }

                if (options.MaxFileSize > 0)
                    result.Add("maxFileSize", options.MaxFileSize);
            }
            data.Add("type", "file");
            data.Add("data", Json.Encode(result));
            builder.MergeAttributes(data);
            builder.MergeAttribute("id", name);

            return MvcHtmlString.Create(builder.ToString());
        }

    }
}

