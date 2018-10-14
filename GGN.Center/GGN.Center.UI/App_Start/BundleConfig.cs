using System.Web;
using System.Web.Optimization;

namespace GGN.Center.UI
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            //// 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/taob/loginstyle").Include(
                      "~/Content/taob/css/framework-font.css",
                      "~/Content/taob/css/framework-login.css"
                      ));

            bundles.Add(new ScriptBundle("~/taob/loginjs").Include(
                      "~/Content/taob/js/jquery/jquery-2.1.1.min.js",
                      "~/Content/taob/js/cookie/jquery.cookie.js",
                      "~/Content/taob/js/md5/jquery.md5.js"
                      ));


            //bundles.Add(new StyleBundle("~/taoa/mainstyle").Include(
            //          "~/Content/taoa/css/bootstrap.min14ed.css?v=3.3.6",
            //          "~/Content/taoa/css/font-awesome.min93e3.css?v=4.4.0",
            //          "~/Content/taoa/css/animate.min.css",
            //          "~/Content/taoa/css/style.min862f.css?v=4.1.0"
            //          ));

            //bundles.Add(new ScriptBundle("~/taoa/mainjs").Include(
            //          "~/Content/taoa/js/jquery.min.js?v=2.1.4",
            //          "~/Content/taoa/js/bootstrap.min.js?v=3.3.6",
            //          "~/Content/taoa/js/plugins/metisMenu/jquery.metisMenu.js",
            //          "~/Content/taoa/js/plugins/slimscroll/jquery.slimscroll.min.js",

            //          "~/Content/taoa/js/plugins/layer/layer.min.js",
            //          "~/Content/taoa/js/taoa.min.js?v=4.1.0",
            //          "~/Content/taoa/js/contabs.min.js",
            //          "~/Content/taoa/js/plugins/pace/pace.min.js"
            //          ));
        }
    }
}
