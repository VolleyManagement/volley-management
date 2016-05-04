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

        /// <summary>
        /// Maps domain model of <see cref="ShortGameResultDto"/> to view model of <see cref="PivotStandingsGameViewModel"/>.
        /// </summary>
        /// <param name="gameResult">Domain model of <see cref="ShortGameResultDto"/>.</param>
        /// <returns>View model of <see cref="PivotStandingsGameViewModel"/>.</returns>
        public static PivotStandingsGameViewModel Map(ShortGameResultDto gameResult)
        {
            return new PivotStandingsGameViewModel
            {
                HomeTeamId = gameResult.HomeTeamId,
                AwayTeamId = gameResult.AwayTeamId,
            };
        }
    }
}