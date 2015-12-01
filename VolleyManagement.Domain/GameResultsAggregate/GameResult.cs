namespace VolleyManagement.Domain.GameResultsAggregate
{
    using System.Collections.Generic;

    /// <summary>
    /// Game result domain class.
    /// </summary>
    public class GameResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameResult"/> class.
        /// </summary>
        public GameResult()
        {
            this.SetScores = new List<Score>();
        }

        /// <summary>
        /// Gets or sets the identifier of game result.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the tournament where game result belongs.
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the home team which played the game.
        /// </summary>
        public int HomeTeamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the away team which played the game.
        /// </summary>
        public int AwayTeamId { get; set; }

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
    }
}
