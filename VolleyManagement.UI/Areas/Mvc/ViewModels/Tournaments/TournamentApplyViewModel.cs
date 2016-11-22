using System.Collections.Generic;
using System.Web.Mvc;
using VolleyManagement.Domain.TeamsAggregate;
using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;

namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    public class TournamentApplyViewModel
    {
        /// <summary>
        /// Gets or sets the tournament identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the team identifier.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the tournament title.
        /// </summary>
        public string TournamentTitle { get; set; }

        /// <summary>
        /// Gets or sets the teams list.
        /// </summary>
        public IEnumerable<TeamNameViewModel> Teams { get; set; }

        public int SelectedTeamId { set; get; }

    }
}