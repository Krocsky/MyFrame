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
        private static bool ShowFirst = false;
        private static bool ShowLast = false;
        private static bool ShowPrevious = true;
        private static bool ShowNext = true;
        private static bool ShowIndividualPages = true;
        private static int IndividualPagesDisplayedCount = 5;

        /// <summary>
        /// 异步分页控件
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="model"></param>
        /// <param name="ajaxUrl"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPager<TModel>(this HtmlHelper html, IPagedList<TModel> model, string ajaxUrl, string target)
        {
            return GeneratePagerButton<TModel>(html, model, true, ajaxUrl, target);
        }

        /// <summary>
        /// 分页控件
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="html"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static MvcHtmlString Pager<TModel>(this HtmlHelper html, IPagedList<TModel> model)
        {
            return GeneratePagerButton<TModel>(html, model);
        }

        #region utilities

        private static MvcHtmlString GeneratePagerButton<TModel>(HtmlHelper html, IPagedList<TModel> model, bool isAjax = false, string ajaxUrl = null, string target = null)
        {
            var routeValuesPage = html.ViewContext.RouteData.Values["pageindex"];
            if (model == null || model.TotalCount == 0)
                return null;

            var links = new StringBuilder();
            if (model.TotalPages > 1)
            {
                if (ShowFirst)
                {
                    if ((model.PageIndex >= 3) && (model.TotalPages > 5))
                    {
                        routeValuesPage = 1;

                        links.Append("<li class=\"paginate_button first\">");
                        links.Append(string.Format("<a href=\"{0}\">{1}</a>", GetPagingUrl(html, 1, ajaxUrl), "首页"));
                        links.Append("</li>");
                    }
                }
                if (ShowPrevious)
                {
                    if (model.PageIndex > 1)
                    {
                        routeValuesPage = (model.PageIndex - 1);

                        links.Append("<li>");
                        links.Append(string.Format("<a href=\"{0}\">{1}</a>", GetPagingUrl(html, model.PageIndex - 1, ajaxUrl), "«"));
                        links.Append("</li>");
                    }
                }
                if (ShowIndividualPages)
                {
                    int firstIndividualPageIndex = GetFirstIndividualPageIndex(model.TotalPages, model.PageIndex);
                    int lastIndividualPageIndex = GetLastIndividualPageIndex(model.TotalPages, model.PageIndex);
                    for (int i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
                    {
                        if (model.PageIndex == i)
                        {
                            links.AppendFormat("<li class=\"active\"><span>{0}</span></li>", i);
                        }
                        else
                        {
                            routeValuesPage = (i + 1);

                            links.Append("<li>");
                            links.Append(string.Format("<a href=\"{0}\">{1}</a>", GetPagingUrl(html, i, ajaxUrl), i));
                            links.Append("</li>");
                        }
                    }
                }
                if (ShowNext)
                {
                    if ((model.PageIndex + 1) < model.TotalPages)
                    {
                        routeValuesPage = (model.PageIndex + 1);

                        links.Append("<li class=\"paginate_button next\">");
                        links.Append(string.Format("<a href=\"{0}\">{1}</a>", GetPagingUrl(html, model.PageIndex + 1, ajaxUrl), "»"));
                        links.Append("</li>");
                    }
                }
                if (ShowLast)
                {
                    if (((model.PageIndex + 3) < model.TotalPages) && (model.TotalPages > IndividualPagesDisplayedCount))
                    {
                        routeValuesPage = model.TotalPages;

                        links.Append("<li class=\"paginate_button last\">");
                        links.Append(string.Format("<a href=\"{0}\">{1}</a>", GetPagingUrl(html, model.TotalPages, ajaxUrl), "末页"));
                        links.Append("</li>");
                    }
                }

                //正常满页计算
                var normalPages = model.TotalCount / model.PageSize;
                //取余数
                int reminder = model.TotalCount % model.PageSize;
                if (reminder > 0)
                {
                    normalPages++;
                }
                links.Append("<li>");
                links.Append(string.Format("<span href=\"{0}\">共计{1}条</span>", "#", model.TotalCount));
                links.Append("</li>");
                links.Append("<li>");
                links.Append(string.Format("<span href=\"{0}\">共计{1}页</span>", "#", normalPages));
                links.Append("</li>");
            }

            var result = links.ToString();

            string ajax = string.Empty;
            if (isAjax)
            {
                ajax = string.Format("data-ajax=\"true\" data-url=\"{0}\" data-target=\"{1}\"", ajaxUrl, target);
            }

            if (!String.IsNullOrEmpty(result))
            {
                result = string.Format("<ul class=\"pagination pull-right\" {0}>{1}</ul>", ajax, result);
            }
            return MvcHtmlString.Create(result);
        }

        private static string GetPagingUrl(this HtmlHelper htmlHelper, int pageIndex, string currentUrl = null)
        {
            object pageIndexObj = null;
            if (htmlHelper.ViewContext.RouteData.Values.TryGetValue("pageindex", out pageIndexObj))
            {
                htmlHelper.ViewContext.RouteData.Values["pageindex"] = pageIndex;

                return UrlHelper.GenerateUrl(null, null, null, htmlHelper.ViewContext.RouteData.Values, RouteTable.Routes, htmlHelper.ViewContext.RequestContext, true);
            }

            if (string.IsNullOrEmpty(currentUrl))
                currentUrl = HttpUtility.HtmlEncode(htmlHelper.ViewContext.HttpContext.Request.RawUrl);

            if (currentUrl.IndexOf("?") == -1)
            {
                return currentUrl + string.Format("?pageindex={0}", pageIndex);
            }
            else
            {
                if (currentUrl.IndexOf("pageindex=", StringComparison.InvariantCultureIgnoreCase) == -1)
                    return currentUrl + string.Format("&pageindex={0}", pageIndex);
                else
                    return Regex.Replace(currentUrl, @"pageindex=(\d+\.?\d*|\.\d+)", "pageindex=" + pageIndex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
        }

        private static int GetFirstIndividualPageIndex(int totalPages, int pageIndex)
        {
            if ((totalPages < IndividualPagesDisplayedCount) ||
                ((pageIndex - (IndividualPagesDisplayedCount / 2)) < 0))
            {
                return 1;
            }
            if ((pageIndex + (IndividualPagesDisplayedCount / 2)) >= totalPages)
            {
                return (totalPages - IndividualPagesDisplayedCount) + 1;
            }
            return (pageIndex - (IndividualPagesDisplayedCount / 2)) + 1;
        }

        /// <summary>
        /// Get last individual page index
        /// </summary>
        /// <returns>Page index</returns>
        private static int GetLastIndividualPageIndex(int totalPages, int pageIndex)
        {
            int num = IndividualPagesDisplayedCount / 2;
            if ((IndividualPagesDisplayedCount % 2) == 0)
            {
                num--;
            }
            if ((totalPages < IndividualPagesDisplayedCount) ||
                ((pageIndex + num) >= totalPages))
            {
                return totalPages;
            }
            if ((pageIndex - (IndividualPagesDisplayedCount / 2)) < 0)
            {
                return IndividualPagesDisplayedCount;
            }
            return (pageIndex + num) + 1;
        }

        #endregion
    }
}

