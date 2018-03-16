namespace VolleyManagement.UI.Areas.WebApi.ViewModels.Games
{
    using Domain;
    using Domain.GamesAggregate;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebAPI.ViewModels.Schedule;

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
        public string GameDate => Date.ToString("yyyy-MM-ddTHH:mm:sszzz");

        /// <summary>
        /// Gets or sets the date and time of the game.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the game result.
        /// </summary>
        public GameResult Result { get; set; }

        /// <summary>
        /// Gets or sets the round for the game.
        /// </summary>
        public int Round { get; set; }

        public byte GameNumber { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the division where game result belongs.
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the division where game result belongs.
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the пкщгз where game result belongs.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets a url to game video
        /// </summary>
        public string UrlToGameVideo { get; set; }

        /// <summary>
        /// Maps domain model of game result to view model of game.
        /// </summary>
        /// <param name="gameResult">Domain model of game.</param>
        /// <returns>View model of game.</returns>
        public static GameViewModel Map(GameResultDto gameResult)
        {
            var game = new GameViewModel
            {
                Id = gameResult.Id,
                HomeTeamName = gameResult.HomeTeamName,
                AwayTeamName = gameResult.AwayTeamName,
                Round = gameResult.Round,
                GameNumber = gameResult.GameNumber,
                Result = new GameResult
                {
                    TotalScore = new ScoreViewModel { Home = gameResult.Result.GameScore.Home, Away = gameResult.Result.GameScore.Away },
                    IsTechnicalDefeat = gameResult.Result.GameScore.IsTechnicalDefeat,
                    SetScores = gameResult.Result.SetScores.Select(item => new ScoreViewModel
                    {
                        Home = item.Home,
                        Away = item.Away,
                        IsTechnicalDefeat = item.IsTechnicalDefeat
                    }).ToList()
                },
                DivisionId = gameResult.DivisionId,
                DivisionName = gameResult.DivisionName,
                GroupId = gameResult.GroupId,
                Date = gameResult.GameDate.GetValueOrDefault(),
                UrlToGameVideo = gameResult.UrlToGameVideo
            };

            if (game.Result.TotalScore.IsEmpty)
            {
                game.Result = null;
            }

            return game;
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
            public IList<ScoreViewModel> SetScores { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether the technical defeat has taken place.
            /// </summary>
            public bool IsTechnicalDefeat { get; set; }
        }
    }
}
