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
        /// Gets or sets the group ID where team is playing
        /// </summary>
        [Display(Name = "GroupId")]
        public int GroupId { get; set; }

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
                GroupId = team.GroupId                
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
        /// Returns team's group name
        /// </summary>
        /// <param name="team">Mapped team</param>
        /// <param name="divisions">Divisions list in the current tournament</param>
        /// <returns>String which contains group name</returns>
        public static string GetGroupName(TeamNameViewModel team, List<Division> divisions)
        {
            return GetGroups(team, divisions)
                   .First(group => group.Id == team.GroupId)
                   .Name;
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

        #region private

        /// <summary>
        /// Returns groups list in the division
        /// </summary>
        /// <param name="team">Mapped team</param>
        /// <param name="divisions">Divisions list in the current tournament</param>
        /// <returns>List of type Group</returns>
        private static List<Group> GetGroups(TeamNameViewModel team, List<Division> divisions)
        {
            return divisions
                   .Where(div => div.Name == team.DivisionName)
                   .Select(res => res.Groups)
                   .First();
        }

        #endregion
    }
}
