namespace VolleyManagement.Domain.GamesAggregate
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a domain model of game result.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        public Result()
        {
            SetScores = new List<Score>();
            InitializeEmptyResult();
        }

        /// <summary>
        /// Gets or sets the final score of the game.
        /// </summary>
        public Score GameScore { get; set; }

        /// <summary>
        /// Gets or sets the set scores.
        /// </summary>
        public IList<Score> SetScores { get; set; }

        /// <summary>
        /// Gets or sets penalty for games
        /// </summary>
        public Penalty Penalty { get; set; }

        /// <summary>
        /// Initialize Result object with default scores.
        /// </summary>
        private void InitializeEmptyResult()
        {
            Penalty = null;
            GameScore = (
                Constants.GameResult.EMPTY_SCORE,
                Constants.GameResult.EMPTY_SCORE,
                Constants.GameResult.DEFAULT_TECHNICAL_DEFEAT);
            for (int i = 0; i < Constants.GameResult.MAX_SETS_COUNT; i++)
            {
                SetScores.Add((
                       Constants.GameResult.EMPTY_SCORE,
                       Constants.GameResult.EMPTY_SCORE));
            }
        }
    }
}
