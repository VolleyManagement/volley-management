namespace VolleyManagement.Domain.GameResultsAggregate
{
    using System;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Game result domain class.
    /// </summary>
    public class GameResult
    {
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
        /// Gets or sets the final score of the game for home team.
        /// </summary>
        public byte HomeSetsScore { get; set; }

        /// <summary>
        /// Gets or sets the final score of the game for away team.
        /// </summary>
        public byte AwaySetsScore { get; set; }

        /// <summary>
        /// Gets or set a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }

        /// <summary>
        /// Gets or sets the set scores for home team.
        /// </summary>
        public byte[] HomeSetScores { get; set; }

        /// <summary>
        /// Gets or sets the set scores for away team.
        /// </summary>
        public byte[] AwaySetScores { get; set; }
    }
}
