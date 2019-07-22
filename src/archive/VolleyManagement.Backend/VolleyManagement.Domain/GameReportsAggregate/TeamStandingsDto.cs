﻿namespace VolleyManagement.Domain.GameReportsAggregate
{
    /// <summary>
    /// Represents a data transfer object of team with total score and statistics values.
    /// </summary>
    public class TeamStandingsDto
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
        /// Gets or sets the ratio of number of sets the team won to number of sets the team lost.
        /// </summary>
        public float? SetsRatio { get; set; }

        /// <summary>
        /// Gets or sets the ratio of number of balls the team won to number of balls the team lost.
        /// </summary>
        public float? BallsRatio { get; set; }
    }
}
