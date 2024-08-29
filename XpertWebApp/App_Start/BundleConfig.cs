using System.Web;
using System.Web.Optimization;

namespace XpertWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            bundles.Add(new StyleBundle("~/bundles/Googleapifonts1", "https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700;900&display=swap"));
            bundles.Add(new StyleBundle("~/bundles/Googleapifonts2", "https://use.fontawesome.com/releases/v5.15.2/css/all.css"));
            //bundles.Add(new StyleBundle("~/bundles/Googleapifonts3", "https://fonts.googleapis.com/css2?family=Gabriela&display=swap"));

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/Migration/jquery-1.10.2.js",
            //            "~/Scripts/Migration/jquery-migrate-1.0.0.js"
            ////"~/Scripts/jquery-{version}.js"
            //)); ;

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*",
            //            "~/Scripts/additional-methods.js"
            //));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"

            //));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/js/mdb.min.js"
                     
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap-login-form.min.css"
                      //"~/Content/animate.css"

                     
            ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
