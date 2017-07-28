namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Contracts;

    public class GroupTeamViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>TeamId of team.</value>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>GroupId of group.</value>
        public int GroupId { get; set; }

        /// <summary>
        /// Maps  GroupTeam to GroupTeamViewModel
        /// </summary>
        /// <param name="groupTeam">Domain group and team</param>
        /// <returns>View model object</returns>
        public static GroupTeamViewModel Map(TeamToGroupInsert groupTeam)
        {
            return new GroupTeamViewModel
            {
                GroupId = groupTeam.GroupId,
                TeamId = groupTeam.TeamId
            };
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns>Domain object</returns>
        public TeamToGroupInsert ToDomain()
        {
            return new TeamToGroupInsert
            {
                GroupId = GroupId,
                TeamId = TeamId
            };
        }
    }
}