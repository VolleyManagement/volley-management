namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Domain.TeamsAggregate;
    using Domain.TournamentsAggregate;

    /// <summary>
    /// Represents team view model just with name and id
    /// </summary>
    public class TeamNameViewModel
    {
        /// <summary>
        /// Gets or sets the team Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the team name
        /// </summary>
        [Display(Name = "Team Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the division name where team is playing
        /// </summary>
        [Display(Name = "Division")]
        public string DivisionName { get; set; }

        /// <summary>
        /// Gets or sets the group name where team is playing
        /// </summary>
        [Display(Name = "Group")]
        public string GroupName { get; set; }

        /// <summary>
        /// Maps Team to TeamNameViewModel
        /// </summary>
        /// <param name="team">Domain team</param>
        /// <returns>View model object</returns>
        public static TeamNameViewModel Map(TeamTournamentDto team)
        {
            return new TeamNameViewModel
            {
                Id = team.TeamId,
                Name = team.TeamName,
                DivisionName = team.DivisionName,
                GroupName = team.GroupName
            };
        }

        /// <summary>
        /// Maps Team to TeamNameViewModel
        /// </summary>
        /// <param name="team">Domain team</param>
        /// <returns>View model object</returns>
        public static TeamNameViewModel Map(Team team)
        {
            return new TeamNameViewModel
            {
                Id = team.Id,
                Name = team.Name
            };
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns>Domain object</returns>
        public Team ToDomain()
        {
            return new Team
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
