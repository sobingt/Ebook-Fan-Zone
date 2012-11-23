using System.Web.Optimization;

namespace EbookZone.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // CSS Bundles
            bundles.Add(new StyleBundle("~/Content/styles").Include(
                "~/Content/*.css",
                "~/Content/themes/base/minified/*.css")
            );

            // JavaScript Bundles

            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                "~/Scripts/jquery-1.8.2.min.js")
            );

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include(
                "~/Scripts/bootstrap.min.js")
            );

            bundles.Add(new ScriptBundle("~/Scripts/knockout").Include(
                "~/Scripts/knockout-2.1.0.js"));
        }
    }
}