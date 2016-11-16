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
            //// NOTE: Bundles {version} parameters doesn't work correctly with min files

            //// TODO: find developer
            bundles.Add(new ScriptBundle("~/bundles/useractionscripts")
                .Include("~/Scripts/UserActions.js"));

            RegisterCommonBundles(bundles);
            RegisterAdminStyles(bundles);
            RegisterAdminScripts(bundles);

            RegisterTeamScripts(bundles);
            RegisterTornamentScripts(bundles);
            RegisterGameScripts(bundles);
        }

        private static void RegisterCommonBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/commoncss")
                   .Include(
                       "~/Content/bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/commonscripts")
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
            RegisterPlayerScripts(bundles);
        }

        #region Domain bundles

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

        private static void RegisterPlayerScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/playerscripts")
                .Include("~/Scripts/VmScripts/PlayerOperations/LinkPlayerToUser.js"));
        }

        #endregion

        #region Admin bundles

        private static void RegisterAdminStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/admincss")
                   .Include(
                       "~/Content/metisMenu.min.css",
                       "~/Content/font-awesome.min.css",
                       "~/Content/Admin/admin.css",
                       "~/Content/Admin/vm.css"));
        }

        private static void RegisterAdminScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/adminscripts")
                        .Include(
                            "~/Scripts/metisMenu.min.js",
                            "~/Scripts/VMScripts/Admin/admin.js",
                            "~/Scripts/VMScripts/Admin/BackButton.js"));
        }

        #endregion
    }
}