namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using VolleyManagement.Domain;
    using VolleyManagement.Domain.Teams;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.UI.App_GlobalResources;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
    
    /// <summary>
    /// Represents team view model
    /// </summary>
    public class TeamViewModel
    {
        /// <summary>
        /// Gets or sets the player Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name of the team
        /// </summary>
        [Display(Name = "TeamName", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "TeamNameRequired"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(Constants.Player.MAX_FIRST_NAME_LENGTH, ErrorMessageResourceName = "TeamMaxLengthErrorMessage"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets coach of the team
        /// </summary>
        [Display(Name = "TeamCoach", ResourceType = typeof(ViewModelResources))]
        [StringLength(Constants.Player.MAX_LAST_NAME_LENGTH, ErrorMessageResourceName = "TeamMaxLengthErrorMessage"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Coach { get; set; }

        /// <summary>
        /// Gets or sets the player birth year
        /// </summary>
        [Display(Name = "TeamAchievements", ResourceType = typeof(ViewModelResources))]
        public string Achievements { get; set; }

        /// <summary>
        /// Gets or sets the captain of the team
        /// </summary>
        [Display(Name = "TeamCaptain", ResourceType = typeof(ViewModelResources))]
        public PlayerViewModel Captain { get; set; }

        /// <summary>
        /// Gets or sets the roster of the team
        /// </summary>
        [Display(Name = "TeamRoster", ResourceType = typeof(ViewModelResources))]
        public IEnumerable<PlayerViewModel> Roster { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="team"> Domain object </param>
        /// <returns> View model object </returns>
        public static TeamViewModel Map(Team team)
        {
            var teamViewModel = new TeamViewModel
            {
                Id = team.Id,
                Name = team.Name,
                Coach = team.Coach,
                Achievements = team.Achievements                
            };

            if (team.Captain != null)
            {
                teamViewModel.Captain = PlayerViewModel.Map(team.Captain)
            }

            teamViewModel.Roster = new List<PlayerViewModel>();
            foreach (var player in team.Roster)
            {
                ((List<PlayerViewModel>)teamViewModel.Roster).Add(PlayerViewModel.Map(player));
            }

            return teamViewModel;
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        public Team ToDomain()
        {
            Team domainTeam = new Team
            {
                Id = this.Id,
                Name = this.Name,
                Coach = this.Coach,
                Achievements = this.Achievements    
            };

            if (this.Captain != null)
            {
                domainTeam.Captain = this.Captain.ToDomain();
            }

            domainTeam.Roster = new List<Player>();
            foreach (var player in Roster)
            {
                ((List<Player>)domainTeam.Roster).Add(player.ToDomain());
            }
            
            return domainTeam;
        }
        #endregion
    }
}