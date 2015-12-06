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

            // Remember: Bundles {version} parameters doesn't work correctly with min files

            bundles.Add(new ScriptBundle("~/bundles/bootstrapscripts")
                        .Include("~/Scripts/jquery-{version}.js",
                                "~/Scripts/bootstrap.min.js",
                                "~/Scripts/VmScripts/VmScripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui")
                        .Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidate")
                        .Include("~/Scripts/jquery.validate.min.js",
                                "~/Scripts/jquery.validate.unobtrusive.min.js"));         

            RegisterTeamScripts(bundles);
        }

        private static void RegisterTeamScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/teameditscripts")
                        .Include("~/Scripts/VmScripts/TeamOperations/CreateEdit.js"));

            bundles.Add(new ScriptBundle("~/bundles/teamlistscripts")
                        .Include("~/Scripts/VmScripts/TeamOperations/List.js"));
        }
    }
}