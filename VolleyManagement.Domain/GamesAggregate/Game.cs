namespace VolleyManagement.Domain.GamesAggregate
{
    using System;

    /// <summary>
    /// Represents a domain model of game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Gets or sets the identifier of game.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the tournament where game belongs.
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
        /// Gets or sets the result of the game.
        /// </summary>
        public Result Result { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the game.
        /// </summary>
        public DateTime GameDate { get; set; }

        /// <summary>
        /// Gets or sets the round of the game in the tournament.
        /// </summary>
        public byte Round { get; set; }
    }
}