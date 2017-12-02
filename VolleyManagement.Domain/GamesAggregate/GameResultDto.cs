namespace VolleyManagement.Domain.GamesAggregate
{
    using System;

    /// <summary>
    /// Represents a data transfer object of game result with home and away team names.
    /// </summary>
    public class GameResultDto
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
        /// Gets or sets the identifier of the division where game result belongs.
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the пкщгз where game result belongs.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the home team which played the game.
        /// </summary>
        public int? HomeTeamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the away team which played the game.
        /// </summary>
        public int? AwayTeamId { get; set; }

        /// <summary>
        /// Gets or sets the name of the home team which played the game.
        /// </summary>
        public string HomeTeamName { get; set; }

        /// <summary>
        /// Gets or sets the name of the away team which played the game.
        /// </summary>
        public string AwayTeamName { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the game.
        /// </summary>
        public DateTime? GameDate { get; set; }

        /// <summary>
        /// Gets or sets the round of the game in the tournament.
        /// </summary>
        public byte Round { get; set; }

        /// <summary>
        /// Gets or sets the game number of the game in the tournament.
        /// </summary>
        public byte GameNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is allowed to edit game's result (for Playoff scheme)
        /// </summary>
        public bool AllowEditResult { get; set; }

        /// <summary>
        /// Gets or sets a value of game result
        /// </summary>
        public Result Result { get; set; }

        /// <summary>
        /// Gets a value indicating whether game has result.
        /// </summary>
        /// <returns>True if score is empty; otherwise, false.</returns>
        public bool HasResult => (!Result?.GameScore?.IsEmpty).GetValueOrDefault();
    }
}
