namespace VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a game part of view model for <see cref="ShortGameResultDto"/>.
    /// </summary>
    public class PivotStandingsGameViewModel
    {
        /// <summary>
        /// Gets or sets the identifier of the home team which played the game.
        /// </summary>
        public int HomeTeamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the away team which played the game.
        /// </summary>
        public int AwayTeamId { get; set; }

        /// <summary>
        /// Gets or sets collection of results of two teams
        /// </summary>
        public List<ShortGameResultViewModel> Results { get; set; }
    }
}
