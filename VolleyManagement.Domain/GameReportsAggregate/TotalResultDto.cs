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
    public class TotalResultDto
    {
        /// <summary>
        /// Gets or sets the final score of the game.
        /// </summary>
        public Score SetsScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }
    }
}
