﻿namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    using Generic;

    /// <summary>
    /// Represents TeamViewModel and referrer link.
    /// </summary>
    public class TeamRefererViewModel : RefererViewModel<TeamViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamRefererViewModel" /> class.
        /// </summary>
        /// <param name="team">Team view model.</param>
        /// <param name="referrer">Referrer controller name.</param>
        /// <param name="curReferrer">Current referrer controller name.</param>
        public TeamRefererViewModel(TeamViewModel team, string referrer, string curReferrer)
            : base(team, referrer)
        {
            CurrentReferrer = curReferrer;
        }

        /// <summary>
        /// Gets or sets current referrer controller name
        /// </summary>
        public string CurrentReferrer { get; set; }
    }
}