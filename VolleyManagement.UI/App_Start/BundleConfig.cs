namespace VolleyManagement.UI
{
    using System.Web.Optimization;

    /// <summary>
    /// Bundle configuration class
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers bundles
        /// </summary>
        /// <param name="bundles">Collection of bundles</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css")
                        .Include("~/Content/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapscripts")
                        .Include(
                                "~/Scripts/jquery-{version}.js",
                                "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/useractionscripts")
                      .Include("~/Scripts/UserActions.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidation")
                       .Include(
                               "~/Scripts/jquery-{version}.js",
                               "~/Scripts/jquery.validate.js",
                               "~/Scripts/jquery.validate.unobtrusive.js"));
        }
    }
}