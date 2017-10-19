namespace VolleyManagement.UI.Areas.WebApi.ViewModels.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.GamesAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// GameViewModel class.
    /// </summary>
    public class GameViewModel
    {
        /// <summary>
        /// Gets or sets the identifier of game result.
        /// </summary>
        public int Id { get; set; }

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
        public string GameDate { get; set; }

        /// <summary>
        /// Gets or sets the game result.
        /// </summary>
        public GameResult Result { get; set; }

        /// <summary>
        /// Gets or sets the round for the game.
        /// </summary>
        public int Round { get; set; }

        /// <summary>
        /// Maps domain model of game result to view model of game.
        /// </summary>
        /// <param name="gameResult">Domain model of game.</param>
        /// <returns>View model of game.</returns>
        public static GameViewModel Map(GameResultDto gameResult)
        {
            return new GameViewModel
            {
                Id = gameResult.Id,
                HomeTeamName = gameResult.HomeTeamName,
                AwayTeamName = gameResult.AwayTeamName,
                GameDate = gameResult.GameDate.HasValue ? gameResult.GameDate.Value.ToString("yyyy-MM-ddTHH:mm:sszzz") : string.Empty,
                Round = gameResult.Round,
                Result = new GameResult
                {
                    TotalScore = new ScoreViewModel { Home = gameResult.Result.SetsScore.Home, Away = gameResult.Result.SetsScore.Away },
                    IsTechnicalDefeat = gameResult.Result.SetsScore.IsTechnicalDefeat,
                    SetScores = gameResult.Result.SetScores.Select(item => new ScoreViewModel
                    {
                        Home = item.Home,
                        Away = item.Away,
                        IsTechnicalDefeat = item.IsTechnicalDefeat
                    }).ToList()
                }
            };
        }

        /// <summary>
        /// GameResult inner class.
        /// </summary>
        public class GameResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GameResult"/> class.
            /// </summary>
            public GameResult()
            {
                TotalScore = new ScoreViewModel();
                SetScores = Enumerable.Repeat(new ScoreViewModel(), Constants.GameResult.MAX_SETS_COUNT).ToList();
            }

            /// <summary>
            /// Gets or sets the final score of the game.
            /// </summary>
            public ScoreViewModel TotalScore { get; set; }

            /// <summary>
            /// Gets or sets the set scores.
            /// </summary>
            public List<ScoreViewModel> SetScores { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the technical defeat has taken place.
            /// </summary>
            public bool IsTechnicalDefeat { get; set; }
        }
    }
}
