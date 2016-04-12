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
        public Score SetsScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }

        /// <summary>
        /// Gets or sets the set scores.
        /// </summary>
        public List<Score> SetScores { get; set; }

        /// <summary>
        /// Initialize Result object with default scores
        /// </summary>       
        private void InitializeEmptyResult()
        {
            this.SetsScore = new Score(
                Constants.GameResult.EMPTY_SCORE,
                Constants.GameResult.EMPTY_SCORE);
            this.IsTechnicalDefeat = Constants.GameResult.DEFAULT_TECHNICAL_DEFEAT;
            for (int i = 0; i < Constants.GameResult.MAX_SETS_COUNT; i++)
            {
                this.SetScores.Add(
                    new Score(
                       Constants.GameResult.EMPTY_SCORE,
                       Constants.GameResult.EMPTY_SCORE));
            }
        }
    }
}
