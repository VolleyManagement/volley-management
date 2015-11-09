namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.UI.App_GlobalResources;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;

    /// <summary>
    /// Represents view model of team home page
    /// </summary>
    public class TeamHomePageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamHomePageViewModel"/> class.
        /// </summary>
        public TeamHomePageViewModel()
        {
            this.Roster = new List<PlayerNameViewModel>();
        }

        /// <summary>
        /// Gets or sets the name of a team.
        /// </summary>
        [Display(Name = "TeamName", ResourceType = typeof(ViewModelResources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the coach of a team.
        /// </summary>
        [Display(Name = "TeamCoach", ResourceType = typeof(ViewModelResources))]
        public string Coach { get; set; }

        /// <summary>
        /// Gets or sets the achievements of a team.
        /// </summary>
        [Display(Name = "TeamAchievements", ResourceType = typeof(ViewModelResources))]
        public string Achievements { get; set; }

        /// <summary>
        /// Gets or sets the captain of a team.
        /// </summary>
        [Display(Name = "TeamCaptain", ResourceType = typeof(ViewModelResources))]
        public PlayerNameViewModel Captain { get; set; }

        /// <summary>
        /// Gets or sets the roster of a team.
        /// </summary>
        [Display(Name = "TeamRoster", ResourceType = typeof(ViewModelResources))]
        public List<PlayerNameViewModel> Roster { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps domain model of <see cref="Team"/> to view model of <see cref="Team"/> home page.
        /// </summary>
        /// <param name="team"><see cref="Team"/> domain model.</param>
        /// <param name="captain">Captain of the team.</param>
        /// <param name="roster">Roster of team's players.</param>
        /// <returns>View model of <see cref="Team"/> home page.</returns>
        public static TeamHomePageViewModel Map(Team team, Player captain, IEnumerable<Player> roster)
        {
            if (team == null)
            {
                return null;
            }

            var viewModel = new TeamHomePageViewModel()
            {
                Name = team.Name,
                Coach = team.Coach,
                Achievements = team.Achievements
            };

            if (captain != null)
            {
                viewModel.Captain = PlayerNameViewModel.Map(captain);
            }

            if (roster != null)
            {
                viewModel.Roster = new List<PlayerNameViewModel>();

                foreach (var player in roster)
                {
                    viewModel.Roster.Add(PlayerNameViewModel.Map(player));
                }
            }

            return viewModel;
        }

        /// <summary>
        /// Maps view model of <see cref="Team"/> home page to domain model of <see cref="Team"/>.
        /// </summary>
        /// <returns><see cref="Team"/> domain model.</returns>
        public Team ToDomain()
        {
            Team team = new Team()
            {
                Name = this.Name,
                CaptainId = this.Captain.Id,
                Coach = this.Coach,
                Achievements = this.Achievements
            };

            return team;
        }

        #endregion
    }
}