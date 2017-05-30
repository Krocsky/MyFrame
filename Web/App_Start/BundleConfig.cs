using Common.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //string bootstrapCdn = "http://cdn.bootcss.com/bootstrap/3.3.6/css/bootstrap.min.css";

            bundles.Add(new StyleBundle("~/Content/bootstrap"/*, bootstrapCdn*/).Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                      "~/Content/animate.min.css",
                      "~/Content/style.min.css",
                      "~/Content/iCheck/custom.css",
                      "~/Content/admin.css"));

            bundles.Add(new ScriptBundle("~/Scripts/adminjs").Include(
                        "~/Scripts/jquery-1.12.4.min.js",
                        "~/Scripts/jquery.livequery.js",
                        "~/Scripts/jquery.form.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/bootstrap-suggest/bootstrap-suggest.min.js",
                        "~/Scripts/contabs.min.js",
                        "~/Scripts/hplus.min.js",
                        "~/Scripts/admin.js",
                        "~/Scripts/index.js"));

            bundles.Add(new ScriptBundle("~/Scripts/thirdpartyjs").Include(
                        "~/Scripts/jquery.metisMenu.js",
                        "~/Scripts/jquery.slimscroll.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/icheck.min.js",
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/jquery-scrollpagination/scrollpagination.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/jQuery.print.js",
                        "~/Scripts/ueditor/ueditor.all.min.js",
                        "~/Scripts/distpicker.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ueditor/ueditorjs").Include(
                        "~/Scripts/ueditor/ueditor.all.min.js",
                        "~/Scripts/ueditor/ueditor.config.js"));

            bundles.Add(new ScriptBundle("~/Scripts/layer/layerjs").Include(
                        "~/Scripts/layer/layer.js"));

            bundles.Add(new StyleBundle("~/bundles/fileinput/css").Include(
                      "~/Scripts/fileinput/css/fileinput.css"));

            bundles.Add(new ScriptBundle("~/bundles/fileinput/js").Include(
                        "~/Scripts/fileinput/js/fileinput.js",
                        "~/Scripts/fileinput/js/fileinput_locale_zh.js",
                        "~/Scripts/fileinput/js/fileinput.init.js"));

            bundles.Add(new ScriptBundle("~/bundles/fullavatareditor/js").Include(
                        "~/Scripts/fullavatareditor/scripts/swfobject.js",
                        "~/Scripts/fullavatareditor/scripts/fullAvatarEditor.js"));

            bundles.Add(new StyleBundle("~/Content/bootstraptablecss").Include(
                      "~/Scripts/bootstrap-table/bootstrap-table.min.css",
                      "~/Scripts/bootstrap-gtreetable/bootstrap-gtreetable.css",
                      "~/Scripts/bootstrap-clockpicker/bootstrap-clockpicker.min.css"));

            bundles.Add(new ScriptBundle("~/Scripts/bootstraptablejs").Include(
                        "~/Scripts/bootstrap-table/bootstrap-table.min.js",
                        "~/Scripts/bootstrap-table/extensions/mobile/bootstrap-table-mobile.js",
                        "~/Scripts/bootstrap-table/extensions/toolbar/bootstrap-table-toolbar.js",
                        "~/Scripts/bootstrap-table/locale/bootstrap-table-zh-CN.js",
                        "~/Scripts/bootstrap-gtreetable/bootstrap-gtreetable.js",
                        "~/Scripts/bootstrap-gtreetable/languages/bootstrap-gtreetable.zh-CN.js",
                        "~/Scripts/bootstrap-clockpicker/bootstrap-clockpicker.min.js"));

            bundles.Add(new StyleBundle("~/Content/webuploadercss").Include(
                      "~/Scripts/webuploader/webuploader.css",
                      "~/Scripts/webuploader/style.css"));

            bundles.Add(new ScriptBundle("~/Scripts/webuploaderjs").Include(
                        "~/Scripts/webuploader/jquery.js",
                        "~/Scripts/webuploader/webuploader.js",
                        "~/Scripts/webuploader/upload.js"));

            bundles.Add(new StyleBundle("~/bundles/blueimpcss").Include(
                      "~/Scripts/blueimp/css/blueimp-gallery.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/blueimpjs").Include(
                        "~/Scripts/blueimp/js/jquery.blueimp-gallery.min.js"));

            bundles.Add(new StyleBundle("~/bundles/datepickercss").Include(
                      "~/Scripts/bootstrap-datepicker/css/bootstrap-datepicker3.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/datepickerjs").Include(
                        "~/Scripts/bootstrap-datepicker/js/bootstrap-datepicker.min.js",
                        "~/Scripts/bootstrap-datepicker/locales/bootstrap-datepicker.zh-CN.min.js"));

            bundles.Add(new StyleBundle("~/bundles/datetimepickercss").Include(
                      "~/Scripts/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepickerjs").Include(
                        "~/Scripts/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js",
                        "~/Scripts/bootstrap-datetimepicker/locales/bootstrap-datetimepicker.zh-CN.js"));

            bundles.Add(new StyleBundle("~/Content/bootstraptablegtreecss").Include("~/Scripts/bootstrap-gtreetable/bootstrap-gtreetable.css"));

            bundles.Add(new ScriptBundle("~/Scripts/bootstraptablegtreejs").Include(
                        "~/Scripts/bootstrap-gtreetable/bootstrap-gtreetable.js",
                        "~/Scripts/bootstrap-gtreetable/languages/bootstrap-gtreetable.zh-CN.js"));

            //jquery step Js
            bundles.Add(new StyleBundle("~/bundles/jquerystepcss").Include(
                        "~/Scripts/jquery-steps/jquery.steps.css"));
            bundles.Add(new ScriptBundle("~/bundles/jquerystepjs").Include(
                        "~/Scripts/jquery-steps/jquery.steps.js"));

            //jquery fancytree
            bundles.Add(new StyleBundle("~/bundles/jqueryfancytreecss").Include(
                        "~/Scripts/jquery-fancytree/ui.fancytree.css"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryfancytreejs").Include(
                        "~/Scripts/jquery-fancytree/jquery.fancytree-all.js"));

            //jquery printer
            bundles.Add(new ScriptBundle("~/bundles/jqueryprint").Include(
                        "~/Scripts/jQuery.print.js"));

            //jquery magnific popup
            bundles.Add(new StyleBundle("~/bundles/jquerymagnificecss").Include(
                        "~/Scripts/jquery-magnific/magnific-popup.css"));
            bundles.Add(new ScriptBundle("~/bundles/jquerymagnificejs").Include(
                        "~/Scripts/jquery-magnific/jquery.magnific-popup.js"));

            //jquery fine uploader
            bundles.Add(new StyleBundle("~/bundles/jqueryfinduploadercss").Include(
                        "~/Scripts/fine-uploader/fine-uploader.css"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryfinduploaderjs").Include(
                        "~/Scripts/fine-uploader/fine-uploader.js"));

            //three level linkage
            bundles.Add(new ScriptBundle("~/bundles/distpicker").Include(
                "~/Scripts/distpicker.js"));

            BundleTable.EnableOptimizations = false;
        }
    }

    public class FixCssRewrite : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return ConvertUrlsToAbsolute(VirtualPathUtility.GetDirectory(WebHelper.ResolveUrl(includedVirtualPath)), input);
        }

        private string ConvertUrlsToAbsolute(string baseUrl, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return content;
            }
            Regex regex = new Regex("url\\(['\"]?(?<url>[^)]+?)['\"]?\\)");
            return regex.Replace(content, (MatchEvaluator)(match => ("url(" + RebaseUrlToAbsolute(baseUrl, match.Groups["url"].Value) + ")")));
        }


        private string RebaseUrlToAbsolute(string baseUrl, string url)
        {
            if ((string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(baseUrl)) || url.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                return url;
            }
            if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl = baseUrl + "/";
            }
            return WebHelper.ResolveUrl(VirtualPathUtility.ToAbsolute(baseUrl + url));
        }
    }
}
