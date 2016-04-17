namespace VolleyManagement.Domain.GameReportsAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GamesAggregate;

    /// <summary>
    /// Represents a single entry in pivot tournament's standings.
    /// </summary>
    public class PivotStandingsEntry
    {
        /// <summary>
        /// Gets or sets the team's identifier.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Gets or sets the team's name.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Gets or sets the number of point for the team.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Gets or sets list of team result set scores.
        /// </summary>
        public List<List<TotalResultDto>> ResultSetScores { get; set; }

        /// <summary>
        /// Gets or sets the ratio of number of sets the team won to number of sets the team lost.
        /// </summary>
        public float SetsRatio { get; set; }
    }
}
