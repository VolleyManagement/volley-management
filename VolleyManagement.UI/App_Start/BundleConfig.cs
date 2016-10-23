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
            bundles.Add(new StyleBundle("~/bundles/css")
                   .Include(
                       "~/Content/bootstrap.min.css",
                       "~/Content/Site.css",
                       "~/Content/themes/base/all.css"));

            //// NOTE: Bundles {version} parameters doesn't work correctly with min files

            bundles.Add(new ScriptBundle("~/bundles/bootstrapscripts")
                        .Include(
                            "~/Scripts/jquery-{version}.js",
                            "~/Scripts/bootstrap.min.js",
                            "~/Scripts/VmScripts/VmScripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/vmscripts")
                        .Include("~/Scripts/VmScripts/VmScripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui")
                        .Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidation")
                        .Include(
                            "~/Scripts/jquery.validate.min.js",
                            "~/Scripts/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/useractionscripts")
                .Include("~/Scripts/UserActions.js"));

            RegisterTeamScripts(bundles);
            RegisterTornamentScripts(bundles);
            RegisterGameScripts(bundles);
            RegisterFeedbackScripts(bundles);
        }

        private static void RegisterTeamScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/teameditscripts")
                .Include("~/Scripts/VmScripts/TeamOperations/CreateEdit.js"));

            bundles.Add(new ScriptBundle("~/bundles/teamlistscripts")
                .Include("~/Scripts/VmScripts/TeamOperations/List.js"));
        }

        private static void RegisterTornamentScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/tournamentteamsscripts")
                        .Include("~/Scripts/VmScripts/TournamentOperations/AddTeams.js"));
            bundles.Add(new ScriptBundle("~/bundles/tournamentroundsscripts")
                        .Include("~/Scripts/VmScripts/TournamentOperations/SwapRounds.js"));
        }

        private static void RegisterGameScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/gameresultsscripts")
                .Include("~/Scripts/VmScripts/GameOperations/DeleteGame.js"));
        }

        private static void RegisterFeedbackScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/feedbackscripts")
                .Include("~/Scripts/VmScripts/FeedbackOperations/BackButton.js"));
        }
    }
}