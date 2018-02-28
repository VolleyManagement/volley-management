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
                            Date = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 1,
                                    Name = "Division Name",
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
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_A_1).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 0),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(0, 0),
                                            new  ScoreViewModel(0, 0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_A_1),
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
                            Date = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 1,
                                    Name = "Division Name",
                                    Rounds = new List<string>
                                    {
                                        "Тур 1",
                                    },
                                },
                                new DivisionTitleViewModel
                                {
                                    Id = 2,
                                    Name = "Division Name1",
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
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_A_1).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 0),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(0, 0),
                                            new  ScoreViewModel(0, 0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_A_1),
                                    UrlToGameVideo = URL_A,
                                },
                                new GameViewModel
                                {
                                    Id = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_A_1).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(1, 3),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(0, 0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round = 2,
                                    DivisionId = 2,
                                    DivisionName = "Division Name1",
                                    GroupId = 2,
                                    Date = DateTime.Parse(DATE_A_1),
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
                            Date = DateTime.Parse(DATE_D),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 2,
                                    Name = "Division Name1",
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
                                    Id = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_D).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(1, 3),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(0, 0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round = 2,
                                    DivisionId = 2,
                                    DivisionName = "Division Name1",
                                    GroupId = 2,
                                    Date = DateTime.Parse(DATE_D),
                                    UrlToGameVideo = URL_B,
                                },
                            },
                        },
                        new ScheduleDayViewModel
                        {
                            Date = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 1,
                                    Name = "Division Name",
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
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_A_1).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 0),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(0, 0),
                                            new  ScoreViewModel(0, 0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_A_1),
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
                            Date = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 1,
                                    Name = "Division Name",
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
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_A_1).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 0),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(0, 0),
                                            new  ScoreViewModel(0, 0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_A_1),
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
                            Date = DateTime.Parse(DATE_B),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 2,
                                    Name = "Division Name1",
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
                                    Id = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_B).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(1, 3),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(0, 0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round = 2,
                                    DivisionId = 2,
                                    DivisionName = "Division Name1",
                                    GroupId = 2,
                                    Date = DateTime.Parse(DATE_B),
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
                            Date = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 1,
                                    Name = "Division Name",
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
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_A_1).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 0),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(0, 0),
                                            new  ScoreViewModel(0, 0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_A_1),
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
                            Date = DateTime.Parse(DATE_B),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 2,
                                    Name = "Division Name1",
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
                                    Id = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_B).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(1, 3),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(0, 0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round = 3,
                                    DivisionId = 2,
                                    DivisionName = "Division Name1",
                                    GroupId = 2,
                                    Date = DateTime.Parse(DATE_B),
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
                            Date = DateTime.Parse(DATE_E),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 1,
                                    Name = "Division Name",
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
                                    Id = 3,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameA",
                                    GameDate = DateTime.Parse(DATE_E).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 2),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(0, 25),
                                            new  ScoreViewModel(0, 25),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(15, 0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round = 2,
                                    DivisionId = 1,
                                    DivisionName = "Division Name",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_E),
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
                            Date = DateTime.Parse(DATE_A_1),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 1,
                                    Name = "Division Name",
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
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_A_1).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 0),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(0, 0),
                                            new  ScoreViewModel(0, 0),
                                        },
                                        IsTechnicalDefeat = true,
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_A_1),
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
                            Date = DateTime.Parse(DATE_E),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 1,
                                    Name = "Division Name1",
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
                                    Id = 3,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameA",
                                    GameDate = DateTime.Parse(DATE_E).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 2),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(0, 25),
                                            new  ScoreViewModel(0, 25),
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(15, 0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round = 2,
                                    DivisionId = 1,
                                    DivisionName = "Division Name1",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_E),
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
                            Date = DateTime.Parse(DATE_F),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 2,
                                    Name = "Division Name2",
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
                                    Id = 2,
                                    HomeTeamName = "TeamNameC",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_F).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(1, 3),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 0),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(5, 25),
                                            new  ScoreViewModel(0, 0),
                                        },
                                        IsTechnicalDefeat = false,
                                    },
                                    Round = 3,
                                    DivisionId = 2,
                                    DivisionName = "Division Name2",
                                    GroupId = 2,
                                    Date = DateTime.Parse(DATE_F),
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
                            Date = DateTime.Parse(DATE_A_2),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 1,
                                    Name = "Division Name1",
                                    Rounds = new List<string>
                                    {
                                        "Тур 1",
                                    },
                                },
                                new DivisionTitleViewModel
                                {
                                    Id = 2,
                                    Name = "Division Name2",
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
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_A_2).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 2),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 20),
                                            new  ScoreViewModel(24, 26),
                                            new  ScoreViewModel(28, 30),
                                            new  ScoreViewModel(25, 22),
                                            new  ScoreViewModel(27, 25),
                                        },
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name1",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_A_2),
                                    UrlToGameVideo = URL_A,
                                },
                                new GameViewModel
                                {
                                    Id = 2,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameC",
                                    GameDate = DateTime.Parse(DATE_A_3).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 1),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(26, 28),
                                            new  ScoreViewModel(25, 15),
                                            new  ScoreViewModel(25, 21),
                                            new  ScoreViewModel(29, 27),
                                            new  ScoreViewModel(0, 0),
                                        },
                                    },
                                    Round = 1,
                                    DivisionId = 2,
                                    DivisionName = "Division Name2",
                                    GroupId = 2,
                                    Date = DateTime.Parse(DATE_A_3),
                                    UrlToGameVideo = URL_B,
                                },
                                new GameViewModel
                                {
                                    Id = 3,
                                    HomeTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_A_1).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = null,
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name1",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_A_1),
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

            _schedule.Schedule = new List<WeekViewModel>
            {
                new WeekViewModel
                {
                    Days = new List<ScheduleDayViewModel>
                    {
                        new ScheduleDayViewModel
                        {
                            Date = DateTime.Parse(DATE_A_2),
                            Divisions = new List<DivisionTitleViewModel>
                            {
                                new DivisionTitleViewModel
                                {
                                    Id = 1,
                                    Name = "Division Name1",
                                    Rounds = new List<string>
                                    {
                                        "Тур 1",
                                    },
                                },
                                new DivisionTitleViewModel
                                {
                                    Id = 2,
                                    Name = "Division Name2",
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
                                    Id = 1,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_A_2).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 2),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(25, 20),
                                            new  ScoreViewModel(24, 26),
                                            new  ScoreViewModel(28, 30),
                                            new  ScoreViewModel(25, 22),
                                            new  ScoreViewModel(27, 25),
                                        },
                                    },
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name1",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_A_2),
                                    UrlToGameVideo = URL_A,
                                },
                                new GameViewModel
                                {
                                    Id = 2,
                                    HomeTeamName = "TeamNameA",
                                    AwayTeamName = "TeamNameC",
                                    GameDate = DateTime.Parse(DATE_A_3).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = new GameViewModel.GameResult
                                    {
                                        TotalScore = new  ScoreViewModel(3, 1),
                                        SetScores = new List< ScoreViewModel>
                                        {
                                            new  ScoreViewModel(26, 28),
                                            new  ScoreViewModel(25, 15),
                                            new  ScoreViewModel(25, 21),
                                            new  ScoreViewModel(29, 27),
                                            new  ScoreViewModel(0, 0),
                                        },
                                    },
                                    Round = 1,
                                    DivisionId = 2,
                                    DivisionName = "Division Name2",
                                    GroupId = 2,
                                    Date = DateTime.Parse(DATE_A_3),
                                    UrlToGameVideo = URL_B,
                                },
                                new GameViewModel
                                {
                                    Id = 3,
                                    HomeTeamName = "TeamNameB",
                                    GameDate = DateTime.Parse(DATE_A_1).ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                    Result = null,
                                    Round = 1,
                                    DivisionId = 1,
                                    DivisionName = "Division Name1",
                                    GroupId = 1,
                                    Date = DateTime.Parse(DATE_A_1),
                                    UrlToGameVideo = URL_C,
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