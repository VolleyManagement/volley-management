namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a view model for <see cref="PivotGameResultsViewModel"/>.
    /// </summary>
    public class PivotGameResultsViewModel
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
        /// Gets or sets the final score of the game for the home team.
        /// </summary>
        public byte? HomeSetsScore { get; set; }

        /// <summary>
        /// Gets or sets the final score of the game for the away team.
        /// </summary>
        public byte? AwaySetsScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }

        /// <summary>
        /// Gets or sets a cascade style sheets class name with style settings.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Maps domain model of game results to view model of game results for pivot table.
        /// </summary>
        /// <param name="game">Domain model of short game result.</param>
        /// <returns>View model of game result for pivot table.</returns>
        public static PivotGameResultsViewModel GetPivotGameResultsViewModelMapper(ShortGameResultDto game)
        {
            return new PivotGameResultsViewModel
            {
                HomeTeamId = game.HomeTeamId,
                AwayTeamId = game.AwayTeamId,
                HomeSetsScore = game.HomeSetsScore,
                AwaySetsScore = game.AwaySetsScore,
                IsTechnicalDefeat = game.IsTechnicalDefeat,
                CssClass = PivotTableViewModel.SetCssClass(game.HomeSetsScore, game.AwaySetsScore)
            };
        }
    }
}
