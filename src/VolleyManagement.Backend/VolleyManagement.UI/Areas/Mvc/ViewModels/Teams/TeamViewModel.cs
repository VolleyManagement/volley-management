﻿namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Domain;
    using Domain.PlayersAggregate;
    using Domain.TeamsAggregate;
    using Players;
    using Resources.UI;

    /// <summary>
    /// Represents team view model
    /// </summary>
    public class TeamViewModel
    {
        /// <summary>
        /// Gets or sets the team Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name of the team
        /// </summary>
        [Display(Name = "TeamName", ResourceType = typeof(ViewModelResources))]
        [Required(
            ErrorMessageResourceName = "TeamNameRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(
            Constants.Team.MAX_NAME_LENGTH,
            ErrorMessageResourceName = "TeamMaxLengthErrorMessage",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets coach of the team
        /// </summary>
        [Display(Name = "TeamCoach", ResourceType = typeof(ViewModelResources))]
        [StringLength(
            Constants.Team.MAX_COACH_NAME_LENGTH,
            ErrorMessageResourceName = "TeamMaxLengthErrorMessage",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [RegularExpression(
            ViewModelConstants.NAME_VALIDATION_REGEX,
            ErrorMessageResourceName = "TeamCoachNameInvalidEntriesError",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Coach { get; set; }

        /// <summary>
        /// Gets or sets team achievements
        /// </summary>
        [Display(Name = "TeamAchievements", ResourceType = typeof(ViewModelResources))]
        [StringLength(
            Constants.Team.MAX_ACHIEVEMENTS_LENGTH,
            ErrorMessageResourceName = "MaxLengthErrorMessage",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Achievements { get; set; }

        /// <summary>
        /// Gets or sets the captain of the team
        /// </summary>
        [Display(Name = "TeamCaptain", ResourceType = typeof(ViewModelResources))]
        [Required(
            ErrorMessageResourceName = "TeamCaptainRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public PlayerNameViewModel Captain { get; set; }

        /// <summary>
        /// Gets or sets the roster of the team
        /// </summary>
        [Display(Name = "TeamRoster", ResourceType = typeof(ViewModelResources))]
        public ICollection<PlayerNameViewModel> Roster { get; set; }

        /// <summary>
        /// Gets or sets the photo of the team
        /// </summary>
        public string PhotoPath { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="team"><see cref="Team"/> domain entity.</param>
        /// <param name="captain">Captain of the team.</param>
        /// <param name="roster">Roster of team's players.</param>
        /// <returns> View model object </returns>
        public static TeamViewModel Map(Team team, Player captain, IEnumerable<Player> roster)
        {
            var teamViewModel = new TeamViewModel {
                Id = team.Id,
                Name = team.Name,
                Coach = team.Coach,
                Achievements = team.Achievements
            };

            if (captain != null)
            {
                teamViewModel.Captain = PlayerNameViewModel.Map(captain);
            }

            if (roster != null)
            {
                teamViewModel.Roster = new List<PlayerNameViewModel>();
                foreach (var player in roster)
                {
                    teamViewModel.Roster.Add(PlayerNameViewModel.Map(player));
                }
            }

            return teamViewModel;
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        public Team ToDomain()
        {
            var domainTeam = new Team {
                Id = Id,
                Name = Name,
                CaptainId = Captain.Id,
                Coach = Coach,
                Achievements = Achievements
            };
            return domainTeam;
        }
        #endregion
    }
}