namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.GameResultsAggregate;

    /// <summary>
    /// Game result view model.
    /// </summary>
    public class GameResultViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultViewModel"/> class.
        /// </summary>
        public GameResultViewModel()
        {
            this.SetScores = Enumerable.Repeat(new Score(), Constants.GameResult.MAX_SETS_COUNT).ToList();
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
        /// Gets or sets name of the home team which played the game.
        /// </summary>
        public string HomeTeamName { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the away team which played the game.
        /// </summary>
        public int AwayTeamId { get; set; }

        /// <summary>
        /// Gets or sets name of the away team which played the game.
        /// </summary>
        public string AwayTeamName { get; set; }

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
        /// Maps domain entity to <see cref="GameResultViewModel"/>.
        /// </summary>
        /// <param name="domainEntity">Domain entity</param>
        /// <returns><see cref="GameResultViewModel"/> entity.</returns>
        public static GameResultViewModel Map(GameResult domainEntity)
        {
            var gameResultViewModel = new GameResultViewModel()
            {
                Id = domainEntity.Id,
                TournamentId = domainEntity.TournamentId,
                HomeTeamId = domainEntity.HomeTeamId,
                AwayTeamId = domainEntity.AwayTeamId,
                SetsScore = new Score(domainEntity.SetsScore.Home, domainEntity.SetsScore.Away),
                IsTechnicalDefeat = domainEntity.IsTechnicalDefeat,
            };
            gameResultViewModel.SetScores.Clear();
            domainEntity.SetScores.ForEach(sc => gameResultViewModel.SetScores.Add(new Score(sc.Home, sc.Away)));
            return gameResultViewModel;
        }

        /// <summary>
        /// Convert to domain model
        /// </summary>
        /// <returns>Domain <see cref="GameResult"/> entity.</returns>
        public GameResult ToDomain()
        {
            var gameResult = new GameResult()
            {
                Id = this.Id,
                TournamentId = this.TournamentId,
                HomeTeamId = this.HomeTeamId,
                AwayTeamId = this.AwayTeamId,
                SetsScore = new Score(this.SetsScore.Home, this.SetsScore.Away),
                IsTechnicalDefeat = this.IsTechnicalDefeat,
            };
            SetScores.ForEach(sc => gameResult.SetScores.Add(new Score(sc.Home, sc.Away)));
            return gameResult;
        }
    }
}