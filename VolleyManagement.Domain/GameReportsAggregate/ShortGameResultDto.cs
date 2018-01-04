namespace VolleyManagement.Domain.GameReportsAggregate
{
    /// <summary>
    /// Represents a data transfer object of game result with scores and technical defeat value.
    /// </summary>
    public class ShortGameResultDto
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
        public byte HomeGameScore { get; set; }

        /// <summary>
        /// Gets or sets the final score of the game for the away team.
        /// </summary>
        public byte AwayGameScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }

        /// <summary>
        /// Gets or sets a number of round game scheduled to be played
        /// </summary>
        public byte RoundNumber { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if game has result or planned
        /// </summary>
        public bool WasPlayed { get; set; }
    }
}
