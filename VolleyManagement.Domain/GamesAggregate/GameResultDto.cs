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
        /// Gets or sets the final score of the game for the home team.
        /// </summary>
        public byte HomeSetsScore { get; set; }

        /// <summary>
        /// Gets or sets the final score of the game for the away team.
        /// </summary>
        public byte AwaySetsScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }

        /// <summary>
        /// Gets or sets the score of the first set for the home team.
        /// </summary>
        public byte HomeSet1Score { get; set; }

        /// <summary>
        /// Gets or sets the score of the first set for the away team.
        /// </summary>
        public byte AwaySet1Score { get; set; }

        /// <summary>
        /// Gets or sets the score of the second set for the home team.
        /// </summary>
        public byte HomeSet2Score { get; set; }

        /// <summary>
        /// Gets or sets the score of the second set for the away team.
        /// </summary>
        public byte AwaySet2Score { get; set; }

        /// <summary>
        /// Gets or sets the score of the third set for the home team.
        /// </summary>
        public byte HomeSet3Score { get; set; }

        /// <summary>
        /// Gets or sets the score of the third set for the away team.
        /// </summary>
        public byte AwaySet3Score { get; set; }

        /// <summary>
        /// Gets or sets the score of the fourth set for the home team.
        /// </summary>
        public byte HomeSet4Score { get; set; }

        /// <summary>
        /// Gets or sets the score of the fourth set for the away team.
        /// </summary>
        public byte AwaySet4Score { get; set; }

        /// <summary>
        /// Gets or sets the score of the fifth set for the home team.
        /// </summary>
        public byte HomeSet5Score { get; set; }

        /// <summary>
        /// Gets or sets the score of the fifth set for the away team.
        /// </summary>
        public byte AwaySet5Score { get; set; }

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
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsSet1TechnicalDefeat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsSet2TechnicalDefeat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsSet3TechnicalDefeat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsSet4TechnicalDefeat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsSet5TechnicalDefeat { get; set; }
    }
}
