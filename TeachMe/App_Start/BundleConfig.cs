using System.Web;
using System.Web.Optimization;

namespace TeachMe
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-1.11.1.js"));//"~/Scripts/jquery-{version}.js")); // 

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                         "~/Scripts/jquery-ui-{version}.js"));
            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                   "~/Content/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            // old css
            bundles.Add(new StyleBundle("~/Content/css1").Include(
                      "~/Content/amelia.css",
                      "~/Content/site.css"));

            // UnifyTheme css
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css")
                      .Include("~/Content/UnifyTheme/*.css", new CssRewriteUrlTransform()));

            // UnifyTheme plugins css
            bundles.Add(new StyleBundle("~/Content/cssplugins")
                     .Include("~/Content/UnifyTheme/plugins/font-awesome.min.css", new CssRewriteUrlTransform())
                     .Include("~/Content/UnifyTheme/plugins/line-icons.css", new CssRewriteUrlTransform())
                     .Include("~/Content/UnifyTheme/plugins/flexslider.css","~/Content/UnifyTheme/plugins/parallax-slider.css")
                     .Include("~/Content/UnifyTheme/plugins/box-shadows.css", "~/Content/UnifyTheme/plugins/animate.css", "~/Content/irfan.rating.css"));

            bundles.Add(new ScriptBundle("~/bundles/unify/page").Include(
                      "~/Scripts/UnifyTheme/jquery-migrate-1.2.1.min.js",
                       "~/Scripts/UnifyTheme/pages/app.js"));
            bundles.Add(new ScriptBundle("~/bundles/unify").Include(
                      "~/Scripts/UnifyTheme/back-to-top.js",
                      "~/Scripts/irfan.rating.js",
                      "~/Scripts/UnifyTheme/jquery.flexslider-min.js",
                      "~/Scripts/UnifyTheme/modernizr.js",
                      "~/Scripts/UnifyTheme/jquery.cslider.js"));

            // Brand and Social Buttons brand-buttons-inversed.css
            bundles.Add(new StyleBundle("~/Content/brand-buttons").Include(
                     "~/Content/UnifyTheme/plugins/brand-buttons-inversed.css",
                     "~/Content/UnifyTheme/plugins/brand-buttons.css"));

            // Other plugins
            //bundles.Add(new ScriptBundle("~/bundles/irfan.rating").Include(
            // "~/Scripts/irfan.rating.js"));
            //bundles.Add(new StyleBundle("~/Content/irfan.rating").Include(
            //       "~/Content/irfan.rating.css"));

            bundles.Add(new ScriptBundle("~/bundles/fileupload").Include(
                     "~/Scripts/jquery.fileupload.js",
                     "~/Scripts/jquery.ui.widget.js"));
            bundles.Add(new StyleBundle("~/Content/fileupload").Include(
                    "~/Content/jquery.fileupload.css"));

            bundles.Add(new ScriptBundle("~/bundles/multiselect").Include(
   "~/Scripts/jquery.multi-select.js"));
            bundles.Add(new StyleBundle("~/Content/multiselect").Include(
                     "~/Content/multi-select.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapswitch").Include(
        "~/Scripts/bootstrap-switch.js"));
            bundles.Add(new StyleBundle("~/Content/bootstrapswitch").Include(
                     "~/Content/bootstrap-switch.css"));
        }
    }
}
