using System.Web.Optimization;

namespace CssSpriteSheetGenerator.WebUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/Site").Include(
                "~/Content/bootstrap.css",
                "~/Content/Site.css",
                "~/Content/bootstrap-responsive.css"));
        }
    }
}