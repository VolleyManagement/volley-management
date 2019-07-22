﻿namespace VolleyManagement.Domain.GameReportsAggregate
{
    /// <summary>
    /// Represents a single entry in tournament's standings.
    /// </summary>
    public class StandingsEntry
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
        /// Gets or sets the total number of games played by the team.
        /// </summary>
        public int GamesTotal { get; set; }

        /// <summary>
        /// Gets or sets the total number of games the team won.
        /// </summary>
        public int GamesWon { get; set; }

        /// <summary>
        /// Gets or sets the total number of games the team lost.
        /// </summary>
        public int GamesLost { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team won with the score three to nil (3:0).
        /// </summary>
        public int GamesWithScoreThreeNil { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team won with the score three to one (3:1).
        /// </summary>
        public int GamesWithScoreThreeOne { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team won with the score three to two (3:2).
        /// </summary>
        public int GamesWithScoreThreeTwo { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team lost with the score two to three (2:3).
        /// </summary>
        public int GamesWithScoreTwoThree { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team lost with the score one to three (1:3).
        /// </summary>
        public int GamesWithScoreOneThree { get; set; }

        /// <summary>
        /// Gets or sets the number of games the team lost with the score nil to three (0:3).
        /// </summary>
        public int GamesWithScoreNilThree { get; set; }

        /// <summary>
        /// Gets or sets the total number of sets the team won.
        /// </summary>
        public int SetsWon { get; set; }

        /// <summary>
        /// Gets or sets the total number of sets the team lost.
        /// </summary>
        public int SetsLost { get; set; }

        /// <summary>
        /// Gets or sets the ratio of number of sets the team won to number of sets the team lost.
        /// </summary>
        public float? SetsRatio => CalculateRatio(SetsWon, SetsLost);

        /// <summary>
        /// Gets or sets the total number of balls the team won.
        /// </summary>
        public int BallsWon { get; set; }

        /// <summary>
        /// Gets or sets the total number of balls the team lost.
        /// </summary>
        public int BallsLost { get; set; }

        /// <summary>
        /// Gets or sets the ratio of number of balls the team won to number of balls the team lost.
        /// </summary>
        public float? BallsRatio => CalculateRatio(BallsWon, BallsLost);

        private static float? CalculateRatio(int won, int lost)
        {
            var result = (float)won / lost;
            return float.IsNaN(result) ? (float?)null : result;
        }
    }
}
