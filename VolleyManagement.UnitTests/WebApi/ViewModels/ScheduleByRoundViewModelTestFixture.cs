namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Domain.GamesAggregate;
    using UI.Areas.WebApi.ViewModels.Games;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ScheduleByRoundViewModelTestFixture
    {
        private const string DATE_A = "2016-04-03T10:00:00+03:00";

        private const string DATE_B = "2016-04-07T10:00:00+03:00";

        private const string DATE_C = "2016-04-10T10:00:00+03:00";

        /// <summary>
        /// Holds collection of games
        /// </summary>
        private List<ScheduleByRoundViewModel> _games = new List<ScheduleByRoundViewModel>();

        /// <summary>
        /// Adds games to collection
        /// </summary>
        /// <returns>Builder object with collection of games</returns>
        public ScheduleByRoundViewModelTestFixture TestGamesWithoutResult()
        {
            _games.Add(
                new ScheduleByRoundViewModel
                {
                    Round = 1,
                    GameResults = new List<GameViewModel>
                    {
                        new GameViewModel()
                        {
                            Id = 1,
                            HomeTeamName = "TeamNameA",
                            AwayTeamName = "TeamNameB",
                            GameDate = DATE_A,
                            Result = null
                        }
                    }
                });
            _games.Add(
                new ScheduleByRoundViewModel
                {
                    Round = 2,
                    GameResults = new List<GameViewModel>
                    {
                        new GameViewModel()
                        {
                            Id = 2,
                            HomeTeamName = "TeamNameA",
                            AwayTeamName = "TeamNameC",
                            GameDate = DATE_B,
                            Result = null
                        }
                    }
                });

            return this;
        }

        public ScheduleByRoundViewModelTestFixture TestGames()
        {
            _games.Add(new ScheduleByRoundViewModel
            {
                Round = 1,
                GameResults = new List<GameViewModel>
                {
                    new GameViewModel()
                    {
                        Id = 1,
                        HomeTeamName = "TeamNameA",
                        AwayTeamName = "TeamNameB",
                        GameDate = DATE_A,
                        Result = new GameViewModel.GameResult
                        {
                            TotalScore = new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(3, 2),
                            SetScores = new List<UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel>()
                            {
                                new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 20),
                                new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(24, 26),
                                new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(28, 30),
                                new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 22),
                                new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(27, 25)
                            },
                            IsTechnicalDefeat = false,
                        },
                        Round = 1
                    }
                }
            });
            _games.Add(new ScheduleByRoundViewModel
            {
                Round = 2,
                GameResults = new List<GameViewModel>
                {
                    new GameViewModel()
                    {
                        Id = 2,
                        HomeTeamName = "TeamNameA",
                        AwayTeamName = "TeamNameC",
                        GameDate = DATE_B,
                        Result = new GameViewModel.GameResult
                        {
                            TotalScore = new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(3, 1),
                            SetScores = new List<UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel>()
                            {
                                new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(26, 28),
                                new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 15),
                                new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 21),
                                new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(29, 27),
                                new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0)
                            },
                            IsTechnicalDefeat = false,
                        },
                        Round = 2
                    }
                }
            });
            _games.Add(new ScheduleByRoundViewModel
            {
                Round = 3,
                GameResults = new List<GameViewModel>
                        {
                            new GameViewModel()
                            {
                                Id = 3,
                                HomeTeamName = "TeamNameB",
                                AwayTeamName = "TeamNameC",
                                GameDate = DATE_C,
                                Result = new GameViewModel.GameResult
                                {
                                    TotalScore = new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 3),
                                    SetScores = new List<UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel>()
                                    {
                                        new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 25),
                                        new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 25),
                                        new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 25),
                                        new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0),
                                        new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0)
                                    },
                                    IsTechnicalDefeat = true,
                                },
                                Round = 3
                            }
                        }
            });

            return this;
        }

        /// <summary>
        /// Add game to collection.
        /// </summary>
        /// <param name="newGame">Game to add.</param>
        /// <returns>Builder object with collection of games.</returns>
        public ScheduleByRoundViewModelTestFixture AddGame(GameViewModel newGame)
        {
            _games[0].GameResults.Add(newGame);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Game collection</returns>
        public List<ScheduleByRoundViewModel> Build()
        {
            return _games;
        }
    }
}
