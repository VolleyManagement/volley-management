namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Group and team identificators view model
    /// </summary>
    public class TeamGroupViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamGroupViewModel"/> class
        /// </summary>
        public TeamGroupViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamGroupViewModel"/> class
        /// </summary>
        /// <param name="groupId">Group id</param>
        /// <param name="teamId">Team id</param>
        public TeamGroupViewModel(int groupId, int teamId)
        {
            GroupId = groupId;
            TeamId = teamId;
        }

        /// <summary>
        /// Gets or sets group id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets team id
        /// </summary>
        public int TeamId { get; set; }
    }
}