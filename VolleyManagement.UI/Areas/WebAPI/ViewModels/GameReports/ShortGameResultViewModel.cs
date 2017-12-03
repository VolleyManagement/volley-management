namespace VolleyManagement.UI.Areas.WebApi.ViewModels.GameReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Domain.GameReportsAggregate;

    /// <summary>
    /// Represents a result part of view model for <see cref="ShortGameResultDto"/>.
    /// </summary>
    public class ShortGameResultViewModel
    {
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
        /// Maps domain model of <see cref="ShortGameResultDto"/> to view model <see cref="ShortGameResultViewModel"/>.
        /// </summary>
        /// <param name="gameResult">Domain model of <see cref="ShortGameResultDto"/>.</param>
        /// <returns>View model <see cref="ShortGameResultViewModel"/>.</returns>
        public static ShortGameResultViewModel Map(ShortGameResultDto gameResult)
        {
            return new ShortGameResultViewModel
            {
                HomeSetsScore = gameResult.HomeGameScore,
                AwaySetsScore = gameResult.AwayGameScore,
                IsTechnicalDefeat = gameResult.IsTechnicalDefeat
            };
        }
    }
}