namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Games;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;

    public class ScheduleViewModelTestFixture
    {
        private const string DATE_A = "2016-04-03T10:00:00+03:00";

        private const string DATE_B = "2016-04-02T10:00:00+03:00";

        private const string DATE_C = "2016-04-07T10:00:00+03:00";

        private readonly DateTime _date_A_For_Round = new DateTime(2016, 04, 03, 10, 00, 00);

        private readonly DateTime _date_B_For_Round = new DateTime(2016, 04, 02, 10, 00, 00);

        private readonly DateTime _date_C_For_Round = new DateTime(2016, 04, 07, 10, 00, 00);

        /// <summary>
        /// Holds collection of games
        /// </summary>
        private ScheduleViewModel _schedule = new ScheduleViewModel();

        public ScheduleViewModelTestFixture WithOneWeekOneDivisionOneGame()
        {
            _schedule.Schedule = new List<Week>
            {
                new Week
                {
                    Days = new List<ScheduleDay>
                    {
                        new ScheduleDay
                        {
                            Date = _date_A_For_Round,
                            Divisions = new List<DivisionTitle>
                            {
                                new DivisionTitle
                                {
                                    Id = 1,
                                    Name = "Division Name",
                                    Rounds = new List<int>
                                    {
                                        1
                                    }
                                }
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DATE_A,
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(3, 0),
                                        SetScores = new List<UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel>
                                        {
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0)
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name",
                                    GroupId = 1,
                                    Date = _date_A_For_Round
                                }
                            }
                        }
                    }
                }
            };
            return this;
        }

        public ScheduleViewModelTestFixture WithOneWeekOneGameDayTwoDivisionsTwoGames()
        {
            _schedule.Schedule = new List<Week>
            {
                new Week
                {
                    Days = new List<ScheduleDay>
                    {
                        new ScheduleDay
                        {
                            Date = _date_A_For_Round,
                            Divisions = new List<DivisionTitle>
                            {
                                new DivisionTitle
                                {
                                    Id = 1,
                                    Name = "Division Name",
                                    Rounds = new List<int>
                                    {
                                        1
                                    }
                                },
                                new DivisionTitle
                                {
                                    Id = 2,
                                    Name = "Division Name1",
                                    Rounds = new List<int>
                                    {
                                        2
                                    }
                                }
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DATE_A,
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(3, 0),
                                        SetScores = new List<UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel>
                                        {
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0)
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name",
                                    GroupId = 1,
                                    Date = _date_A_For_Round
                                },
                                new GameViewModel
                                {
                                    Id = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DATE_A,
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(1, 3),
                                        SetScores = new List<UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel>
                                        {
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(5, 25),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(5, 25),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(5, 25),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0)
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round = 2,
                                    DivisionId = 2,
                                    DivisionName = "Division Name1",
                                    GroupId = 2,
                                    Date = _date_A_For_Round
                                }
                            }
                        }
                    }
                }
            };
            return this;
        }

        public ScheduleViewModelTestFixture WithOneWeekTwoGameDaysTwoDivisionsTwoGames()
        {
            _schedule.Schedule = new List<Week>
            {
                new Week
                {
                    Days = new List<ScheduleDay>
                    {
                        new ScheduleDay
                        {
                            Date = _date_B_For_Round,
                            Divisions = new List<DivisionTitle>
                            {
                                new DivisionTitle
                                {
                                    Id = 2,
                                    Name = "Division Name1",
                                    Rounds = new List<int>
                                    {
                                        2
                                    }
                                }
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DATE_B,
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(1, 3),
                                        SetScores = new List<UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel>
                                        {
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(5, 25),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(5, 25),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(5, 25),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0)
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round = 2,
                                    DivisionId = 2,
                                    DivisionName = "Division Name1",
                                    GroupId = 2,
                                    Date = _date_B_For_Round
                                }
                            }
                        },
                          new ScheduleDay
                        {
                            Date = _date_A_For_Round,
                            Divisions = new List<DivisionTitle>
                            {
                                new DivisionTitle
                                {
                                    Id = 1,
                                    Name = "Division Name",
                                    Rounds = new List<int>
                                    {
                                        1
                                    }
                                }
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DATE_A,
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(3, 0),
                                        SetScores = new List<UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel>
                                        {
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0)
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name",
                                    GroupId = 1,
                                    Date = _date_A_For_Round
                                }
                            }
                        }
                    }
                }
            };
            return this;
        }

        public ScheduleViewModelTestFixture WithTwoWeeksTwoDivisionsTwoGames()
        {
            _schedule.Schedule = new List<Week>
            {
                new Week
                {
                    Days = new List<ScheduleDay>
                    {
                        new ScheduleDay
                        {
                            Date = _date_A_For_Round,
                            Divisions = new List<DivisionTitle>
                            {
                                new DivisionTitle
                                {
                                    Id = 1,
                                    Name = "Division Name",
                                    Rounds = new List<int>
                                    {
                                        1
                                    }
                                }
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DATE_A,
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(3, 0),
                                        SetScores = new List<UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel>
                                        {
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0)
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name",
                                    GroupId = 1,
                                    Date = _date_A_For_Round
                                }
                            }
                        }
                    }
                },
                new Week
                {
                    Days = new List<ScheduleDay>
                    {
                        new ScheduleDay
                        {
                            Date = _date_C_For_Round,
                            Divisions = new List<DivisionTitle>
                            {
                                new DivisionTitle
                                {
                                    Id = 2,
                                    Name = "Division Name1",
                                    Rounds = new List<int>
                                    {
                                        2
                                    }
                                }
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DATE_C,
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(1, 3),
                                        SetScores = new List<UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel>
                                        {
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(25, 0),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(5, 25),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(5, 25),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(5, 25),
                                            new UI.Areas.Mvc.ViewModels.GameResults.ScoreViewModel(0, 0)
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round = 2,
                                    DivisionId = 2,
                                    DivisionName = "Division Name1",
                                    GroupId = 2,
                                    Date = _date_C_For_Round
                                }
                            }
                        }
                    }
                }
            };
            return this;
        }

        public ScheduleViewModelTestFixture WithEmptyResult()
        {
            _schedule.Schedule = new List<Week>();
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Game collection</returns>
        public ScheduleViewModel Build()
        {
            return _schedule;
        }
    }
}