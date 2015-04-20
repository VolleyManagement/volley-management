namespace VolleyManagement.UI.Areas.WebApi.ViewModels.Teams
{
    using System.ComponentModel.DataAnnotations;

    using VolleyManagement.Domain;
    using VolleyManagement.Domain.Teams;
    using VolleyManagement.UI.App_GlobalResources;

    /// <summary>
    /// Represents team view model
    /// </summary>
    public class TeamViewModel
    {
        /// <summary>
        /// Gets or sets the team Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the captain Id.
        /// </summary>
        public int CaptainId { get; set; }

        /// <summary>
        /// Gets or sets the team name.
        /// </summary>
        [Display(Name = "TeamName", ResourceType = typeof(ViewModelResources))]
        [StringLength(Constants.Team.MAX_NAME_LENGTH, ErrorMessageResourceName = "MaxLengthErrorMessage"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the team coach.
        /// </summary>
        [Display(Name = "TeamCoach", ResourceType = typeof(ViewModelResources))]
        [StringLength(Constants.Team.MAX_COACH_NAME_LENGTH, ErrorMessageResourceName = "MaxLengthErrorMessage"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Coach { get; set; }

        /// <summary>
        /// Gets or sets the team achievements.
        /// </summary>
        [Display(Name = "TeamAchievements", ResourceType = typeof(ViewModelResources))]
        [StringLength(Constants.Team.MAX_ACHIEVEMENTS_LENGTH, ErrorMessageResourceName = "MaxLengthErrorMessage"
            , ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Achievements { get; set; }

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
                Achievements = team.Achievements,
                //CaptainId = team.CaptainId
            };

            return teamViewModel;
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        public Team ToDomain()
        {
            return new Team
            {
                Id = this.Id,
                Name = this.Name,
                Coach = this.Coach,
                Achievements = this.Achievements,
                //CaptainId = this.CaptainId
            };
        }
        #endregion
    }
}