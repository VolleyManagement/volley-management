namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults
{
    using System;
    using Domain.GameResultsAggregate;

    /// <summary>
    /// Game result view model.
    /// </summary>
    public class GameResultViewModel
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
        /// Gets or sets the home team name which played the game.
        /// </summary>
        public string HomeTeamName { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the away team which played the game.
        /// </summary>
        public int AwayTeamId { get; set; }

        /// <summary>
        /// Gets or sets the away team name which played the game.
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
        /// Gets or set a value indicating whether the technical defeat has taken place.
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
        /// Maps domain entity to <see cref="GameResultViewModel"/>.
        /// </summary>
        /// <param name="domainEntity">Domain entity</param>
        /// <returns><see cref="GameResultViewModel"/> entity.</returns>
        public static GameResultViewModel Map(GameResult domainEntity)
        {
            return new GameResultViewModel()
            {
                Id = domainEntity.Id,
                TournamentId = domainEntity.TournamentId,
                HomeTeamId = domainEntity.HomeTeamId,
                AwayTeamId = domainEntity.AwayTeamId,
                HomeSetsScore = domainEntity.HomeSetsScore,
                AwaySetsScore = domainEntity.AwaySetsScore,
                IsTechnicalDefeat = domainEntity.IsTechnicalDefeat,
                HomeSet1Score = domainEntity.HomeSet1Score,
                AwaySet1Score = domainEntity.AwaySet1Score,
                HomeSet2Score = domainEntity.HomeSet2Score,
                AwaySet2Score = domainEntity.AwaySet2Score,
                HomeSet3Score = domainEntity.HomeSet3Score,
                AwaySet3Score = domainEntity.AwaySet3Score,
                HomeSet4Score = domainEntity.HomeSet4Score,
                AwaySet4Score = domainEntity.AwaySet4Score,
                HomeSet5Score = domainEntity.HomeSet5Score,
                AwaySet5Score = domainEntity.AwaySet5Score
            };
        }

        /// <summary>
        /// Convert to domain model
        /// </summary>
        /// <returns>Domain <see cref="GameResult"/> entity.</returns>
        public GameResult ToDomain()
        {
            return new GameResult()
            {
                TournamentId = this.TournamentId,
                HomeTeamId = this.HomeTeamId,
                AwayTeamId = this.AwayTeamId,
                HomeSetsScore = this.HomeSetsScore,
                AwaySetsScore = this.AwaySetsScore,
                IsTechnicalDefeat = this.IsTechnicalDefeat,
                HomeSet1Score = this.HomeSet1Score,
                AwaySet1Score = this.AwaySet1Score,
                HomeSet2Score = this.HomeSet2Score,
                AwaySet2Score = this.AwaySet2Score,
                HomeSet3Score = this.HomeSet3Score,
                AwaySet3Score = this.AwaySet3Score,
                HomeSet4Score = this.HomeSet4Score,
                AwaySet4Score = this.AwaySet4Score,
                HomeSet5Score = this.HomeSet5Score,
                AwaySet5Score = this.AwaySet5Score
            };
        }
    }
}