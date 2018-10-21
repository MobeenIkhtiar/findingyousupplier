using System.Web;
using System.Web.Optimization;

namespace FindingYouSupplier
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/Content/assets/js/vendor/modernizr-2.8.3-respond-1.4.2.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/assets/js/plugins.js",
                        "~/Content/assets/js/modernizr.js",
                        "~/Content/assets/js/main.js"
                        ));

            bundles.Add(new ScriptBundle("~/Admin/js").Include(
                      "~/Content/Admin/assets/js/ace-extra.min.js",
                      "~/Content/Admin/assets/js/jquery-2.1.4.min.js",
                      "~/Content/Admin/assets/js/bootstrap.min.js",
                      "~/Content/Admin/assets/js/jquery-ui.custom.min.js",
                      "~/Content/Admin/assets/js/ace-elements.min.js",
                      "~/Content/Admin/assets/js/ace.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Template/css").Include(
                      "~/Content/assets/css/bootstrap.min.css",
                      "~/Content/assets/css/roboto-webfont.css",
                      "~/Content/assets/css/style.css",
                      "~/Content/assets/css/responsive.css"
                      
                      ));
            bundles.Add(new StyleBundle("~/Admin/css").Include(
                      "~/Content/Admin/assets/css/bootstrap.min.css",
                      "~/Content/Admin/assets/font-awesome/4.5.0/css/font-awesome.min.css",
                      "~/Content/Admin/assets/css/fonts.googleapis.com.css",
                      "~/Content/Admin/assets/css/ace.min.css",
                      "~/Content/Admin/assets/css/ace-skins.min.css"
                      ));
        }
    }
}
