namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System;
    using System.Collections.Generic;
    using UI.Areas.WebApi.ViewModels.Games;
    using UI.Areas.WebAPI.ViewModels.Schedule;

    public class ScheduleViewModelTestFixture
    {
        private const string DATE_A_1 = "2016-04-03 10:00";
        private const string DATE_A_2 = "2016-04-03 12:00";
        private const string DATE_A_3 = "2016-04-03 14:00";

        private const string DATE_B = "2016-04-07 10:00";

        private const string DATE_C = "2016-04-10 10:00";

        private const string DATE_D = "2016-04-02 10:00";

        private const string DATE_E = "2016-04-22 10:00";

        private const string DATE_F = "2017-04-22 10:00";

        private const string DATE_PLAYOFF_START = "2018-02-26 02:00";

        private const string URL_A = "http://test-url-a.com";

        private const string URL_B = "http://test-url-b.com";

        private const string URL_C = "http://test-url-c.com";

        private const string URL_D = "http://test-url-d.com";

        /// <summary>
        /// Holds collection of games
        /// </summary>
        private ScheduleViewModel _schedule = new ScheduleViewModel();

        public ScheduleViewModelTestFixture WithOneWeekOneDivisionOneGame()
        {
            _schedule.Schedule = new List<WeekViewModel>
            {
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 1,
                                    Name   = "Division Name",
                                    Rounds = new List<string>
                                    {
                                        "Тур 1",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(3, 0),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(0,  0),
                                            new ScoreViewModel(0,  0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round          = 1,
                                    DivisionId     = 1,
                                    DivisionName   = "Division Name",
                                    GroupId        = 1,
                                    Date           = DateTime.Parse(DATE_A_1),
                                    UrlToGameVideo = URL_A,
                                },
                            },
                        },
                    },
                },
            };
            return this;
        }

        public ScheduleViewModelTestFixture WithOneWeekOneGameDayTwoDivisionsTwoGames()
        {
            _schedule.Schedule = new List<WeekViewModel>
            {
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 1,
                                    Name   = "Division Name",
                                    Rounds = new List<string>
                                    {
                                        "Тур 1",
                                    },
                                },
                                new DivisionTitleViewModel
                                {
                                    Id     = 2,
                                    Name   = "Division Name1",
                                    Rounds = new List<string>
                                    {
                                        "Тур 2",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(3, 0),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(0,  0),
                                            new ScoreViewModel(0,  0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round          = 1,
                                    DivisionId     = 1,
                                    DivisionName   = "Division Name",
                                    GroupId        = 1,
                                    Date           = DateTime.Parse(DATE_A_1),
                                    UrlToGameVideo = URL_A,
                                },
                                new GameViewModel
                                {
                                    Id           = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(1, 3),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(0,  0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round          = 2,
                                    DivisionId     = 2,
                                    DivisionName   = "Division Name1",
                                    GroupId        = 2,
                                    Date           = DateTime.Parse(DATE_A_1),
                                    UrlToGameVideo = URL_B,
                                },
                            },
                        },
                    },
                },
            };
            return this;
        }

        public ScheduleViewModelTestFixture WithOneWeekTwoGameDaysTwoDivisionsTwoGames()
        {
            _schedule.Schedule = new List<WeekViewModel>
            {
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_D),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 2,
                                    Name   = "Division Name1",
                                    Rounds = new List<string>
                                    {
                                        "Тур 2",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(1, 3),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(0,  0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round          = 2,
                                    DivisionId     = 2,
                                    DivisionName   = "Division Name1",
                                    GroupId        = 2,
                                    Date           = DateTime.Parse(DATE_D),
                                    UrlToGameVideo = URL_B,
                                },
                            },
                        },
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 1,
                                    Name   = "Division Name",
                                    Rounds = new List<string>
                                    {
                                        "Тур 1",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(3, 0),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(0,  0),
                                            new ScoreViewModel(0,  0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round          = 1,
                                    DivisionId     = 1,
                                    DivisionName   = "Division Name",
                                    GroupId        = 1,
                                    Date           = DateTime.Parse(DATE_A_1),
                                    UrlToGameVideo = URL_A,
                                },
                            },
                        },
                    },
                },
            };
            return this;
        }

        public ScheduleViewModelTestFixture WithTwoWeeksTwoDivisionsTwoGames()
        {
            _schedule.Schedule = new List<WeekViewModel>
            {
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 1,
                                    Name   = "Division Name",
                                    Rounds = new List<string>
                                    {
                                        "Тур 1",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(3, 0),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(0,  0),
                                            new ScoreViewModel(0,  0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round          = 1,
                                    DivisionId     = 1,
                                    DivisionName   = "Division Name",
                                    GroupId        = 1,
                                    Date           = DateTime.Parse(DATE_A_1),
                                    UrlToGameVideo = URL_A,
                                },
                            },
                        },
                    },
                },
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_B),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 2,
                                    Name   = "Division Name1",
                                    Rounds = new List<string>
                                    {
                                        "Тур 2",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(1, 3),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(0,  0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round          = 2,
                                    DivisionId     = 2,
                                    DivisionName   = "Division Name1",
                                    GroupId        = 2,
                                    Date           = DateTime.Parse(DATE_B),
                                    UrlToGameVideo = URL_B,
                                },
                            },
                        },
                    },
                },
            };
            return this;
        }

        public ScheduleViewModelTestFixture WithThreeWeeksTwoDivisionsThreeGames()
        {
            _schedule.Schedule = new List<WeekViewModel>
            {
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 1,
                                    Name   = "Division Name",
                                    Rounds = new List<string>
                                    {
                                        "Тур 1",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(3, 0),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(0,  0),
                                            new ScoreViewModel(0,  0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round          = 1,
                                    DivisionId     = 1,
                                    DivisionName   = "Division Name",
                                    GroupId        = 1,
                                    Date           = DateTime.Parse(DATE_A_1),
                                    UrlToGameVideo = URL_A,
                                },
                            },
                        },
                    },
                },
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_B),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 2,
                                    Name   = "Division Name1",
                                    Rounds = new List<string>
                                    {
                                        "Тур 3",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(1, 3),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(0,  0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round          = 3,
                                    DivisionId     = 2,
                                    DivisionName   = "Division Name1",
                                    GroupId        = 2,
                                    Date           = DateTime.Parse(DATE_B),
                                    UrlToGameVideo = URL_B,
                                },
                            },
                        },
                    },
                },
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_E),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 1,
                                    Name   = "Division Name",
                                    Rounds = new List<string>
                                    {
                                        "Тур 2",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 3,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameA",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(3, 2),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(0,  25),
                                            new ScoreViewModel(0,  25),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(15, 0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round          = 2,
                                    DivisionId     = 1,
                                    DivisionName   = "Division Name",
                                    GroupId        = 1,
                                    Date           = DateTime.Parse(DATE_E),
                                    UrlToGameVideo = URL_A,
                                },
                            },
                        },
                    },
                },
            };
            return this;
        }

        public ScheduleViewModelTestFixture WithGamesInSeveralYearsAndWeeks()
        {
            _schedule.Schedule = new List<WeekViewModel>
            {
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 1,
                                    Name   = "Division Name",
                                    Rounds = new List<string>
                                    {
                                        "Тур 1",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(3, 0),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(0,  0),
                                            new ScoreViewModel(0,  0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round          = 1,
                                    DivisionId     = 1,
                                    DivisionName   = "Division Name",
                                    GroupId        = 1,
                                    Date           = DateTime.Parse(DATE_A_1),
                                    UrlToGameVideo = URL_A,
                                },
                            },
                        },
                    },
                },
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_E),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 1,
                                    Name   = "Division Name1",
                                    Rounds = new List<string>
                                    {
                                        "Тур 2",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 3,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameA",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(3, 2),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(0,  25),
                                            new ScoreViewModel(0,  25),
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(15, 0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round          = 2,
                                    DivisionId     = 1,
                                    DivisionName   = "Division Name1",
                                    GroupId        = 1,
                                    Date           = DateTime.Parse(DATE_E),
                                    UrlToGameVideo = URL_A,
                                },
                            },
                        },
                    },
                },
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_F),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 2,
                                    Name   = "Division Name2",
                                    Rounds = new List<string>
                                    {
                                        "Тур 3",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(1, 3),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 0),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(5,  25),
                                            new ScoreViewModel(0,  0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round          = 3,
                                    DivisionId     = 2,
                                    DivisionName   = "Division Name2",
                                    GroupId        = 2,
                                    Date           = DateTime.Parse(DATE_F),
                                    UrlToGameVideo = URL_B,
                                },
                            },
                        },
                    },
                },
            };
            return this;
        }

        public ScheduleViewModelTestFixture WithEmptyResult()
        {
            _schedule.Schedule = new List<WeekViewModel>();
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


        public ScheduleViewModelTestFixture WithGamesInSeveralDivisionsAndFreeDayGameInOneDay()
        {
            _schedule.Schedule = new List<WeekViewModel>
            {
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date      = DateTime.Parse(DATE_A_2),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id     = 1,
                                    Name   = "Division Name1",
                                    Rounds = new List<string>
                                    {
                                        "Тур 1",
                                    },
                                },
                                new DivisionTitleViewModel
                                {
                                    Id     = 2,
                                    Name   = "Division Name2",
                                    Rounds = new List<string>
                                    {
                                        "Тур 1",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(3, 2),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(25, 20),
                                            new ScoreViewModel(24, 26),
                                            new ScoreViewModel(28, 30),
                                            new ScoreViewModel(25, 22),
                                            new ScoreViewModel(27, 25),
                                        },
                                    },
                                    Round          = 1,
                                    DivisionId     = 1,
                                    DivisionName   = "Division Name1",
                                    GroupId        = 1,
                                    Date           = DateTime.Parse(DATE_A_2),
                                    UrlToGameVideo = URL_A,
                                },
                                new GameViewModel
                                {
                                    Id           = 2,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameC",
                                    Result       = new GameViewModel.GameResult
                                    {
                                        TotalScore = new ScoreViewModel(3, 1),
                                        SetScores  = new List<ScoreViewModel>
                                        {
                                            new ScoreViewModel(26, 28),
                                            new ScoreViewModel(25, 15),
                                            new ScoreViewModel(25, 21),
                                            new ScoreViewModel(29, 27),
                                            new ScoreViewModel(0,  0),
                                        },
                                    },
                                    Round          = 1,
                                    DivisionId     = 2,
                                    DivisionName   = "Division Name2",
                                    GroupId        = 2,
                                    Date           = DateTime.Parse(DATE_A_3),
                                    UrlToGameVideo = URL_B,
                                },
                                new GameViewModel
                                {
                                    Id             = 3,
                                    HomeTeamName   = "TeamNameB",
                                    Result         = null,
                                    Round          = 1,
                                    DivisionId     = 1,
                                    DivisionName   = "Division Name1",
                                    GroupId        = 1,
                                    Date           = DateTime.Parse(DATE_A_1),
                                    UrlToGameVideo = URL_C,
                                },
                            },
                        },
                    },
                },
            };
            return this;
        }

        public ScheduleViewModelTestFixture With5RoundPlayoffGames()
        {
            var round1Date = DateTime.Parse(DATE_PLAYOFF_START);
            var round2Date = round1Date.AddDays(1);
            var round3Date = round1Date.AddDays(2);
            var round4Date = round1Date.AddDays(3);
            var round5Date = round1Date.AddDays(4);
            _schedule.Schedule = new List<WeekViewModel>
            {
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        // Day 1, Round of 32
                        new ScheduleDayViewModel
                        {
                            Date      = round1Date,
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Rounds = new List<string>
                                    {
                                        "Раунд 32",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date,
                                },
                                new GameViewModel
                                {
                                    Id           = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameD",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(1),
                                },
                                new GameViewModel
                                {
                                    Id           = 3,
                                    HomeTeamName = "TeamNameE",
                                    AwayTeamName = "TeamNameF",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(2),
                                },
                                new GameViewModel
                                {
                                    Id           = 4,
                                    HomeTeamName = "TeamNameG",
                                    AwayTeamName = "TeamNameH",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(3),
                                },
                                new GameViewModel
                                {
                                    Id           = 5,
                                    HomeTeamName = "TeamNameI",
                                    AwayTeamName = "TeamNameJ",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(4),
                                },
                                new GameViewModel
                                {
                                    Id           = 6,
                                    HomeTeamName = "TeamNameK",
                                    AwayTeamName = "TeamNameL",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(5),
                                },
                                new GameViewModel
                                {
                                    Id           = 7,
                                    HomeTeamName = "TeamNameM",
                                    AwayTeamName = "TeamNameN",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(6),
                                },
                                new GameViewModel
                                {
                                    Id           = 8,
                                    HomeTeamName = "TeamNameO",
                                    AwayTeamName = "TeamNameP",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(7),
                                },
                                new GameViewModel
                                {
                                    Id           = 9,
                                    HomeTeamName = "TeamNameR",
                                    AwayTeamName = "TeamNameS",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(8),
                                },
                                new GameViewModel
                                {
                                    Id           = 10,
                                    HomeTeamName = "TeamNameT",
                                    AwayTeamName = "TeamNameU",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(9),
                                },
                                new GameViewModel
                                {
                                    Id           = 11,
                                    HomeTeamName = "TeamNameV",
                                    AwayTeamName = "TeamNameX",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(10),
                                },
                                new GameViewModel
                                {
                                    Id           = 12,
                                    HomeTeamName = "TeamNameY",
                                    AwayTeamName = "TeamNameZ",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(11),
                                },
                                new GameViewModel
                                {
                                    Id           = 13,
                                    HomeTeamName = "TeamNameAA",
                                    AwayTeamName = "TeamNameAB",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(12),
                                },
                                new GameViewModel
                                {
                                    Id           = 14,
                                    HomeTeamName = "TeamNameAC",
                                    AwayTeamName = "TeamNameAD",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(13),
                                },
                                new GameViewModel
                                {
                                    Id           = 15,
                                    HomeTeamName = "TeamNameAE",
                                    AwayTeamName = "TeamNameAF",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(14),
                                },
                                new GameViewModel
                                {
                                    Id           = 16,
                                    HomeTeamName = "TeamNameAG",
                                    AwayTeamName = "TeamNameAH",
                                    Result       = null,
                                    Round        = 1,
                                    Date         = round1Date.AddHours(15),
                                },
                            },
                        },
                        // Day 2, Round of 16
                        new ScheduleDayViewModel
                        {
                            Date      = round2Date,
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Rounds = new List<string>
                                    {
                                        "Раунд 16",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 17,
                                    Result       = null,
                                    Round        = 2,
                                    Date         = round2Date,
                                },
                                new GameViewModel
                                {
                                    Id           = 18,
                                    Result       = null,
                                    Round        = 2,
                                    Date         = round2Date.AddHours(1),
                                },
                                new GameViewModel
                                {
                                    Id           = 19,
                                    Result       = null,
                                    Round        = 2,
                                    Date         = round2Date.AddHours(2),
                                },
                                new GameViewModel
                                {
                                    Id           = 20,
                                    Result       = null,
                                    Round        = 2,
                                    Date         = round2Date.AddHours(3),
                                },
                                new GameViewModel
                                {
                                    Id           = 21,
                                    Result       = null,
                                    Round        = 2,
                                    Date         = round2Date.AddHours(4),
                                },
                                new GameViewModel
                                {
                                    Id           = 22,
                                    Result       = null,
                                    Round        = 2,
                                    Date         = round2Date.AddHours(5),
                                },
                                new GameViewModel
                                {
                                    Id           = 23,
                                    Result       = null,
                                    Round        = 2,
                                    Date         = round2Date.AddHours(6),
                                },
                                new GameViewModel
                                {
                                    Id           = 24,
                                    Result       = null,
                                    Round        = 2,
                                    Date         = round2Date.AddHours(7),
                                },
                            },
                        },
                        // Day 3, Quarter finals
                        new ScheduleDayViewModel
                        {
                            Date      = round3Date,
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Rounds = new List<string>
                                    {
                                        "Четверть-финал",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 25,
                                    Result       = null,
                                    Round        = 3,
                                    Date         = round3Date,
                                },
                                new GameViewModel
                                {
                                    Id           = 26,
                                    Result       = null,
                                    Round        = 3,
                                    Date         = round3Date.AddHours(1),
                                },
                                new GameViewModel
                                {
                                    Id           = 27,
                                    Result       = null,
                                    Round        = 3,
                                    Date         = round3Date.AddHours(2),
                                },
                                new GameViewModel
                                {
                                    Id           = 28,
                                    Result       = null,
                                    Round = 3,
                                    Date         = round3Date.AddHours(3),
                                },
                            },
                        },
                        // Day 4, Semi finals
                        new ScheduleDayViewModel
                        {
                            Date      = round4Date,
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Rounds = new List<string>
                                    {
                                        "Полуфинал",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 29,
                                    Result       = null,
                                    Round = 4,
                                    Date         = round4Date,
                                },
                                new GameViewModel
                                {
                                    Id           = 30,
                                    Result       = null,
                                    Round = 4,
                                    Date         = round4Date.AddHours(1),
                                },
                            },
                        },
                        // Day 5, Finals
                        new ScheduleDayViewModel
                        {
                            Date      = round5Date,
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Rounds = new List<string>
                                    {
                                        "Финал",
                                    },
                                },
                            },
                            Games = new List<GameViewModel>
                            {
                                new GameViewModel
                                {
                                    Id           = 31,
                                    Result       = null,
                                    Round        = 5,
                                    Date         = round5Date,
                                },
                                new GameViewModel
                                {
                                    Id           = 32,
                                    Result       = null,
                                    Round        = 5,
                                    Date         = round5Date.AddHours(1),
                                },
                            },
                        },
                    },
                },
            };
            return this;
        }
    }
}