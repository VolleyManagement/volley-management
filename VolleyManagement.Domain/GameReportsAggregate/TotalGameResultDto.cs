namespace VolleyManagement.Domain.GameReportsAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.Domain.GamesAggregate;

    /// <summary>
    /// Represents a data transfer object of game result with scores and technical defeat value.
    /// </summary>
    public class TotalGameResultDto
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
        public byte HomeSetsScore { get; set; }

        /// <summary>
        /// Gets or sets the final score of the game for the away team.
        /// </summary>
        public byte AwaySetsScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }
    }
}
