using Common;
using System.Web;
using System.Web.Mvc;

namespace Web
{
    /// <summary>
    /// 导航扩展
    /// </summary>
    public static class NavigationExtension
    {
        /// <summary>
        /// 获取导航Url
        /// </summary>
        /// <param name="value"></param>
        public static string GetUrl(this Navigation nav)
        {
            string url = "#";
            if (!string.IsNullOrEmpty(nav.RouteName))
            {
                try
                {
                    url = new UrlHelper(HttpContext.Current.Request.RequestContext).RouteUrl(nav.RouteName);
                }
                catch
                { }
            }
            else if (!string.IsNullOrEmpty(nav.Url))
            {
                url = nav.Url;
            }
            return url;
        }
    }
}