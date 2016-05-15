namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Generic;

    /// <summary>
    /// Represents TeamViewModel and referrer link.
    /// </summary>
    public class TeamRefererViewModel : RefererViewModel<TeamViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamRefererViewModel" /> class.
        /// </summary>
        /// <param name="team">Domain team model.</param>
        /// <param name="referrer">Referrer controller name.</param>
        public TeamRefererViewModel(TeamViewModel team, string referrer)
            : base(team, referrer)
        {
        }
    }
}