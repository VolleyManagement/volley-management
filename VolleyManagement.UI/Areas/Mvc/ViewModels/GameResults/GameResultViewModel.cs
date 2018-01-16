namespace VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts.Authorization;
    using Domain;
    using Domain.GamesAggregate;

    /// <summary>
    /// Represents a view model for game result.
    /// </summary>
    public class GameResultViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultViewModel"/> class.
        /// </summary>
        public GameResultViewModel()
        {
            GameScore = new ScoreViewModel();
            SetScores = Enumerable.Repeat(new ScoreViewModel(), Constants.GameResult.MAX_SETS_COUNT).ToList();
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
        /// Gets or sets the final score of the game.
        /// </summary>
        public ScoreViewModel GameScore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the technical defeat has taken place.
        /// </summary>
        public bool IsTechnicalDefeat { get; set; }

        /// <summary>
        /// Gets or sets the set scores.
        /// </summary>
        public List<ScoreViewModel> SetScores { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the game.
        /// </summary>
        public DateTime? GameDate { get; set; }

        /// <summary>
        /// Gets or sets the round of the game in the tournament.
        /// </summary>
        public int Round { get; set; }

        /// <summary>
        /// Gets or sets the number of the game in the tournament
        /// </summary>
        public byte GameNumber { get; set; }

        public bool HasPenalty { get; set; }

        public bool IsHomeTeamPenalty { get; set; }

        public byte PenaltyAmount { get; set; }

        public string PenaltyDescrition { get; set; }

        /// <summary>
        /// Gets or sets a url to game video
        /// </summary>
        public string UrlToGameVideo { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether it is allowed to edit game's result (for Playoff scheme)
        /// </summary>
        public bool AllowEditResult { get; set; }

        /// <summary>
        /// Gets a value indicating whether gets an identifier whether this game is a first round game.
        /// </summary>
        public bool IsFirstRoundGame
        {
            get
            {
                return Round == 1;
            }
        }

        /// <summary>
        /// Gets the format of game date
        /// </summary>
        public string ShortGameDate
        {
            get
            {
                return GameDate.HasValue ? GameDate.Value.ToString("d MMM dddd H:mm") : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets instance of <see cref="AllowedOperations"/> create object
        /// </summary>
        public AllowedOperations AllowedOperations { get; set; }

        /// <summary>
        /// Maps domain model of game result to view model of game result.
        /// </summary>
        /// <param name="gameResult">Domain model of game result.</param>
        /// <returns>View model of game result.</returns>
        public static GameResultViewModel Map(GameResultDto gameResult)
        {
            var result = new GameResultViewModel
            {
                Id = gameResult.Id,
                TournamentId = gameResult.TournamentId,
                HomeTeamId = gameResult.HomeTeamId,
                AwayTeamId = gameResult.AwayTeamId,
                HomeTeamName = gameResult.HomeTeamName,
                AwayTeamName = gameResult.AwayTeamName,
                GameDate = gameResult.GameDate,
                GameNumber = gameResult.GameNumber,
                Round = gameResult.Round,
                UrlToGameVideo = gameResult.UrlToGameVideo,
                GameScore = new ScoreViewModel { Home = gameResult.Result.GameScore.Home, Away = gameResult.Result.GameScore.Away },
                IsTechnicalDefeat = gameResult.Result.GameScore.IsTechnicalDefeat,
                AllowEditResult = gameResult.AllowEditResult,
                SetScores = gameResult.Result.SetScores.Select(item => new ScoreViewModel
                {
                    Home = item.Home,
                    Away = item.Away,
                    IsTechnicalDefeat = item.IsTechnicalDefeat
                }).ToList()
            };

            if (gameResult.HasResult && gameResult.Result.Penalty != null)
            {
                var penalty = gameResult.Result.Penalty;
                result.HasPenalty = true;
                result.IsHomeTeamPenalty = penalty.IsHomeTeam;
                result.PenaltyAmount = penalty.Amount;
                result.PenaltyDescrition = penalty.Description;
            }

            return result;
        }

        /// <summary>
        /// Maps view model of game result to domain model of game.
        /// </summary>
        /// <returns>Domain model of game.</returns>
        public Game ToDomain()
        {
            var penalty = MapPenalty();
            return new Game
            {
                Id = Id,
                TournamentId = TournamentId,
                HomeTeamId = HomeTeamId,
                AwayTeamId = AwayTeamId,
                Round = Convert.ToByte(Round),
                GameDate = GameDate,
                GameNumber = GameNumber,
                Result = new Result
                {
                    GameScore = GameScore.ToDomain(),
                    SetScores = SetScores.Select(item => item.ToDomain()).ToList(),
                    Penalty = penalty
                },
                UrlToGameVideo = UrlToGameVideo
            };
        }

        private Penalty MapPenalty()
        {
            Penalty result = null;

            if (HasPenalty)
            {
                result = new Penalty
                {
                    IsHomeTeam = IsHomeTeamPenalty,
                    Amount = PenaltyAmount,
                    Description = PenaltyDescrition
                };
            }

            return result;
        }
    }
}
