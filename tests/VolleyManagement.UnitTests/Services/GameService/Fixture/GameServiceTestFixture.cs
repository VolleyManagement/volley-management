namespace VolleyManagement.UnitTests.Services.GameService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GamesAggregate;

    /// <summary>
    /// Generates test data for <see cref="GameResultDto"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameServiceTestFixture
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

        private readonly List<GameResultDto> _gameResults = new List<GameResultDto>();

        public GameServiceTestFixture TestGameResults()
        {
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (3, 2),
                    SetScores = new List<Score>
                    {
                        (25, 20),
                        (24, 26),
                        (28, 30),
                        (25, 22),
                        (27, 25),
                    },
                },
                GameDate = DateTime.Parse(DATE_A_1),
                Round = 1,
                UrlToGameVideo = URL_A,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameC",
                Result = new Result
                {
                    GameScore = (3, 1),
                    SetScores = new List<Score>
                    {
                        (26, 28),
                        (25, 15),
                        (25, 21),
                        (29, 27),
                        (0, 0),
                    },
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                UrlToGameVideo = URL_B,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = 2,
                AwayTeamId = 3,
                HomeTeamName = "TeamNameB",
                AwayTeamName = "TeamNameC",
                Result = new Result
                {
                    GameScore = (0, 3, true),
                    SetScores = new List<Score>
                    {
                        (0, 25),
                        (0, 25),
                        (0, 25),
                        (0, 0),
                        (0, 0),
                    },
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                UrlToGameVideo = URL_C,
            });

            return this;
        }

        public GameServiceTestFixture TestGamesForDuplicateSchemeOne()
        {
            _gameResults.Clear();

            _gameResults.Add(new GameResultDto()
            {
                Id = 1,
                GameDate = DateTime.Parse(DATE_A_1),
                HomeTeamId = 1,
                HomeTeamName = "Team A",
                AwayTeamId = 2,
                AwayTeamName = "Team B",
                TournamentId = 1,
                Round = 1,
                GameNumber = 0,
                UrlToGameVideo = URL_A,
            });

            return this;
        }

        public GameServiceTestFixture TestGamesForDuplicateSchemeTwo()
        {
            _gameResults.Clear();
            _gameResults.Add(
                new GameResultDto()
                {
                    Id = 1,
                    GameDate = DateTime.Parse(DATE_A_1),
                    HomeTeamId = 1,
                    AwayTeamId = 2,
                    TournamentId = 1,
                    Round = 1,
                    GameNumber = 0,
                    UrlToGameVideo = URL_A,
                });
            _gameResults.Add(
                new GameResultDto()
                {
                    Id = 2,
                    GameDate = DateTime.Parse(DATE_A_1),
                    HomeTeamId = 3,
                    AwayTeamId = 4,
                    TournamentId = 1,
                    Round = 1,
                    GameNumber = 0,
                    UrlToGameVideo = URL_B,
                });

            return this;
        }

        public GameServiceTestFixture TestGamesSameTeamsSwitchedOrderTournamentSchemTwo()
        {
            _gameResults.Clear();
            _gameResults.Add(
                new GameResultDto()
                {
                    Id = 1,
                    GameDate = DateTime.Parse(DATE_A_1),
                    HomeTeamId = 1,
                    AwayTeamId = 2,
                    TournamentId = 1,
                    Round = 1,
                    GameNumber = 0,
                    UrlToGameVideo = URL_A,
                });
            _gameResults.Add(
                new GameResultDto()
                {
                    Id = 2,
                    GameDate = DateTime.Parse(DATE_A_1),
                    HomeTeamId = 2,
                    AwayTeamId = 1,
                    TournamentId = 1,
                    Round = 2,
                    GameNumber = 0,
                    UrlToGameVideo = URL_B,
                });

            return this;
        }

        public GameServiceTestFixture TestGamesWithFreeDay()
        {
            _gameResults.Clear();
            _gameResults.Add(
                new GameResultDto()
                {
                    Id = 1,
                    GameDate = DateTime.Parse(DATE_A_1),
                    HomeTeamId = 1,
                    AwayTeamId = null,
                    TournamentId = 1,
                    Round = 1,
                    GameNumber = 0,
                    UrlToGameVideo = URL_A,
                });

            return this;
        }

        public GameServiceTestFixture TestGamesWithTwoFreeDays()
        {
            _gameResults.Clear();
            _gameResults.Add(
                new GameResultDto()
                {
                    Id = 1,
                    GameDate = DateTime.Parse(DATE_A_1),
                    HomeTeamId = 1,
                    AwayTeamId = null,
                    TournamentId = 1,
                    Round = 1,
                    GameNumber = 0,
                    UrlToGameVideo = URL_A,
                });
            _gameResults.Add(
                new GameResultDto()
                {
                    Id = 2,
                    GameDate = DateTime.Parse(DATE_A_1),
                    HomeTeamId = 1,
                    AwayTeamId = null,
                    TournamentId = 1,
                    Round = 2,
                    GameNumber = 0,
                    UrlToGameVideo = URL_B,
                });

            return this;
        }

        public GameServiceTestFixture TestGamesWithoutResult()
        {
            _gameResults.Clear();
            _gameResults.Add(
                new GameResultDto()
                {
                    Id = 1,
                    TournamentId = 1,
                    HomeTeamId = 1,
                    AwayTeamId = 2,
                    HomeTeamName = "TeamNameA",
                    AwayTeamName = "TeamNameB",
                    Result = new Result
                    {
                        GameScore = new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false,
                        },
                        SetScores = new List<Score>
                        {
                            new Score(0, 0),
                            new Score(0, 0),
                            new Score(0, 0),
                            new Score(0, 0),
                            new Score(0, 0),
                        },
                    },
                    GameDate = DateTime.Parse(DATE_A_1),
                    Round = 1,
                    GameNumber = 0,
                    UrlToGameVideo = URL_A,
                });
            _gameResults.Add(
                new GameResultDto()
                {
                    Id = 2,
                    TournamentId = 1,
                    HomeTeamId = 1,
                    AwayTeamId = 3,
                    HomeTeamName = "TeamNameA",
                    AwayTeamName = "TeamNameC",
                    Result = new Result
                    {
                        GameScore = new Score(0, 0),
                        SetScores = new List<Score>
                        {
                            new Score(0, 0),
                            new Score(0, 0),
                            new Score(0, 0),
                            new Score(0, 0),
                            new Score(0, 0),
                        },
                    },
                    GameDate = DateTime.Parse(DATE_B),
                    Round = 2,
                    GameNumber = 0,
                    UrlToGameVideo = URL_B,
                });

            return this;
        }

        public GameServiceTestFixture TestGamesWithResultInTwoWeeksTwoDivisionsTwoGames()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = new Score(3, 0, true),
                    SetScores = new List<Score>
                    {
                        new Score(25, 0),
                        new Score(25, 0),
                        new Score(25, 0),
                        new Score(0,  0),
                        new Score(0,  0),
                    },
                },
                GameDate = DateTime.Parse(DATE_A_1),
                Round = 1,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
                DivisionName = "Division Name",
                UrlToGameVideo = URL_A,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameC",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = new Score(1, 3),
                    SetScores = new List<Score>
                    {
                        new Score(25, 0),
                        new Score(5,  25),
                        new Score(5,  25),
                        new Score(5,  25),
                        new Score(0,  0),
                    },
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 0,
                GroupId = 2,
                DivisionId = 2,
                DivisionName = "Division Name1",
                UrlToGameVideo = URL_B,
            });

            return this;
        }

        public GameServiceTestFixture TestGamesWithResultInOneWeekTwoGameDaysTwoDivisionsTwoGames()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = new Score(3, 0, true),
                    SetScores = new List<Score>
                    {
                        new Score(25, 0),
                        new Score(25, 0),
                        new Score(25, 0),
                        new Score(0,  0),
                        new Score(0,  0),
                    },
                },
                GameDate = DateTime.Parse(DATE_A_1),
                Round = 1,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
                DivisionName = "Division Name",
                UrlToGameVideo = URL_A,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameC",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = new Score(1, 3),
                    SetScores = new List<Score>
                    {
                        new Score(25, 0),
                        new Score(5,  25),
                        new Score(5,  25),
                        new Score(5,  25),
                        new Score(0,  0),
                    },
                },
                GameDate = DateTime.Parse(DATE_D),
                Round = 2,
                GameNumber = 0,
                GroupId = 2,
                DivisionId = 2,
                DivisionName = "Division Name1",
                UrlToGameVideo = URL_B,
            });

            return this;
        }

        public GameServiceTestFixture TestGamesWithResultInOneWeekOneGameDayTwoDivisionsTwoGames()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = new Score(3, 0, true),
                    SetScores = new List<Score>
                    {
                        new Score(25, 0),
                        new Score(25, 0),
                        new Score(25, 0),
                        new Score(0,  0),
                        new Score(0,  0),
                    },
                },
                GameDate = DateTime.Parse(DATE_A_1),
                Round = 1,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
                DivisionName = "Division Name",
                UrlToGameVideo = URL_A,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameC",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = new Score(1, 3),
                    SetScores = new List<Score>
                    {
                        new Score(25, 0),
                        new Score(5,  25),
                        new Score(5,  25),
                        new Score(5,  25),
                        new Score(0,  0),
                    },
                },
                GameDate = DateTime.Parse(DATE_A_1),
                Round = 2,
                GameNumber = 0,
                GroupId = 2,
                DivisionId = 2,
                DivisionName = "Division Name1",
                UrlToGameVideo = URL_B,
            });

            return this;
        }

        public GameServiceTestFixture TestGamesWithResultInThreeWeeksTwoDivisionsThreeGames()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (3, 0, true),
                    SetScores = new List<Score> { (25, 0), (25, 0), (25, 0), (0, 0), (0, 0), },
                },
                GameDate = DateTime.Parse(DATE_A_1),
                Round = 1,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
                DivisionName = "Division Name",
                UrlToGameVideo = URL_A,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 1,
                HomeTeamName = "TeamNameC",
                AwayTeamName = "TeamNameA",
                Result = new Result
                {
                    GameScore = (3, 2),
                    SetScores = new List<Score>
                    {
                        (25, 0),
                        (0, 25),
                        (0, 25),
                        (25, 0),
                        (15, 0),
                    },
                },
                GameDate = DateTime.Parse(DATE_E),
                Round = 2,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
                DivisionName = "Division Name",
                UrlToGameVideo = URL_A,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameC",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (1, 3),
                    SetScores = new List<Score>
                    {
                        (25, 0),
                        (5, 25),
                        (5, 25),
                        (5, 25),
                        (0, 0),
                    },
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 3,
                GameNumber = 0,
                GroupId = 2,
                DivisionId = 2,
                DivisionName = "Division Name1",
                UrlToGameVideo = URL_B,
            });

            return this;
        }

        public GameServiceTestFixture TestGamesInSeveralYearsAndWeeks()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (3, 0, true),
                    SetScores = new List<Score> { (25, 0), (25, 0), (25, 0), (0, 0), (0, 0), },
                },
                GameDate = DateTime.Parse(DATE_A_1),
                Round = 1,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
                DivisionName = "Division Name",
                UrlToGameVideo = URL_A,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 1,
                HomeTeamName = "TeamNameC",
                AwayTeamName = "TeamNameA",
                Result = new Result
                {
                    GameScore = (3, 2),
                    SetScores = new List<Score>
                    {
                        (25, 0),
                        (0, 25),
                        (0, 25),
                        (25, 0),
                        (15, 0),
                    },
                },
                GameDate = DateTime.Parse(DATE_E),
                Round = 2,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
                DivisionName = "Division Name1",
                UrlToGameVideo = URL_A,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameC",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (1, 3),
                    SetScores = new List<Score>
                    {
                        (25, 0),
                        (5, 25),
                        (5, 25),
                        (5, 25),
                        (0, 0),
                    },
                },
                GameDate = DateTime.Parse(DATE_F),
                Round = 3,
                GameNumber = 0,
                GroupId = 2,
                DivisionId = 2,
                DivisionName = "Division Name2",
                UrlToGameVideo = URL_B,
            });

            return this;
        }

        public GameServiceTestFixture TestGamesForSeveralDivisionsAndFreeDayGameInOneDay()
        {
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                DivisionId = 1,
                DivisionName = "Division Name1",
                GroupId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (3, 2),
                    SetScores = new List<Score>
                    {
                        (25, 20),
                        (24, 26),
                        (28, 30),
                        (25, 22),
                        (27, 25),
                    },
                },
                GameDate = DateTime.Parse(DATE_A_2),
                Round = 1,
                UrlToGameVideo = URL_A,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                DivisionId = 2,
                DivisionName = "Division Name2",
                GroupId = 2,
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameC",
                Result = new Result
                {
                    GameScore = (3, 1),
                    SetScores = new List<Score>
                    {
                        (26, 28),
                        (25, 15),
                        (25, 21),
                        (29, 27),
                        (0, 0),
                    },
                },
                GameDate = DateTime.Parse(DATE_A_3),
                Round = 1,
                UrlToGameVideo = URL_B,
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                DivisionId = 1,
                DivisionName = "Division Name1",
                GroupId = 1,
                HomeTeamId = 2,
                AwayTeamId = null,
                HomeTeamName = "TeamNameB",
                AwayTeamName = null,
                Result = new Result(),
                GameDate = DateTime.Parse(DATE_A_1),
                Round = 1,
                UrlToGameVideo = URL_C,
            });

            return this;
        }

        public GameServiceTestFixture TestEmptyGamesInPlayoff()
        {
            _gameResults.Clear();

            _gameResults.AddRange(
                new List<GameResultDto>
                {
                    new GameResultDto
                    {
                        Id             = 1,
                        HomeTeamId     = 1,
                        AwayTeamId     = 2,
                        GameNumber     = 1,
                        Round          = 1,
                        TournamentId   = 1,
                        UrlToGameVideo = URL_A,
                    },
                    new GameResultDto
                    {
                        Id             = 2,
                        HomeTeamId     = 3,
                        AwayTeamId     = 4,
                        GameNumber     = 2,
                        Round          = 1,
                        TournamentId   = 1,
                        UrlToGameVideo = URL_B,
                    },
                    new GameResultDto
                    {
                        Id             = 3,
                        HomeTeamId     = 5,
                        AwayTeamId     = 6,
                        Round          = 1,
                        GameNumber     = 3,
                        TournamentId   = 1,
                        UrlToGameVideo = URL_C,
                    },
                    new GameResultDto
                    {
                        Id             = 4,
                        Round          = 1,
                        HomeTeamId     = 7,
                        AwayTeamId     = 8,
                        GameNumber     = 4,
                        TournamentId   = 1,
                        UrlToGameVideo = URL_D,
                    },
                    new GameResultDto
                    {
                        Id           = 5,
                        Round        = 2,
                        GameNumber   = 5,
                        TournamentId = 1,
                    },
                    new GameResultDto
                    {
                        Id           = 6,
                        Round        = 2,
                        GameNumber   = 6,
                        TournamentId = 1,
                    },
                    new GameResultDto
                    {
                        Id           = 7,
                        Round        = 3,
                        GameNumber   = 7,
                        TournamentId = 1,
                    },
                    new GameResultDto
                    {
                        Id           = 8,
                        Round        = 3,
                        GameNumber   = 8,
                        TournamentId = 1,
                    },
                });

            return this;
        }

        public GameServiceTestFixture TestPlayoffWith5Rounds()
        {
            _gameResults.Clear();

            #region Round 1 (Round of 32)

            var round1Date = DateTime.Parse(DATE_PLAYOFF_START);
            _gameResults.AddRange(
                new List<GameResultDto>
                {
                    new GameResultDto
                    {
                        Id         = 1,
                        GameNumber = 1,
                        HomeTeamId = 1,
                        HomeTeamName = "TeamNameA",
                        AwayTeamId = 2,
                        AwayTeamName = "TeamNameB",
                        Round      = 1,
                        GameDate = round1Date,
                    },
                    new GameResultDto
                    {
                        Id         = 2,
                        GameNumber = 2,
                        HomeTeamId = 3,
                        HomeTeamName = "TeamNameC",
                        AwayTeamId = 4,
                        AwayTeamName = "TeamNameD",
                        Round      = 1,
                        GameDate = round1Date.AddHours(1),
                    },
                    new GameResultDto
                    {
                        Id         = 3,
                        GameNumber = 3,
                        HomeTeamId = 5,
                        HomeTeamName = "TeamNameE",
                        AwayTeamId = 6,
                        AwayTeamName = "TeamNameF",
                        Round      = 1,
                        GameDate = round1Date.AddHours(2),
                    },
                    new GameResultDto
                    {
                        Id         = 4,
                        GameNumber = 4,
                        HomeTeamId = 7,
                        HomeTeamName = "TeamNameG",
                        AwayTeamId = 8,
                        AwayTeamName = "TeamNameH",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(3),
                    },
                    new GameResultDto
                    {
                        Id         = 5,
                        GameNumber = 5,
                        HomeTeamId = 9,
                        HomeTeamName = "TeamNameI",
                        AwayTeamId = 10,
                        AwayTeamName = "TeamNameJ",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(4),
                    },
                    new GameResultDto
                    {
                        Id         = 6,
                        GameNumber = 6,
                        HomeTeamId = 11,
                        HomeTeamName = "TeamNameK",
                        AwayTeamId = 12,
                        AwayTeamName = "TeamNameL",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(5),
                    },
                    new GameResultDto
                    {
                        Id         = 7,
                        GameNumber = 7,
                        HomeTeamId = 13,
                        HomeTeamName = "TeamNameM",
                        AwayTeamId = 14,
                        AwayTeamName = "TeamNameN",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(6),
                    },
                    new GameResultDto
                    {
                        Id         = 8,
                        GameNumber = 8,
                        HomeTeamId = 15,
                        HomeTeamName = "TeamNameO",
                        AwayTeamId = 16,
                        AwayTeamName = "TeamNameP",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(7),
                    },
                    new GameResultDto
                    {
                        Id         = 9,
                        GameNumber = 9,
                        HomeTeamId = 17,
                        HomeTeamName = "TeamNameR",
                        AwayTeamId = 18,
                        AwayTeamName = "TeamNameS",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(8),
                    },
                    new GameResultDto
                    {
                        Id         = 10,
                        GameNumber = 10,
                        HomeTeamId = 19,
                        HomeTeamName = "TeamNameT",
                        AwayTeamId = 20,
                        AwayTeamName = "TeamNameU",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(9),
                    },
                    new GameResultDto
                    {
                        Id         = 11,
                        GameNumber = 11,
                        HomeTeamId = 21,
                        HomeTeamName = "TeamNameV",
                        AwayTeamId = 22,
                        AwayTeamName = "TeamNameX",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(10),
                    },
                    new GameResultDto
                    {
                        Id         = 12,
                        GameNumber = 12,
                        HomeTeamId = 23,
                        HomeTeamName = "TeamNameY",
                        AwayTeamId = 24,
                        AwayTeamName = "TeamNameZ",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(11),
                    },
                    new GameResultDto
                    {
                        Id         = 13,
                        GameNumber = 13,
                        HomeTeamId = 25,
                        HomeTeamName = "TeamNameAA",
                        AwayTeamId = 26,
                        AwayTeamName = "TeamNameAB",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(12),
                    },
                    new GameResultDto
                    {
                        Id         = 14,
                        GameNumber = 14,
                        HomeTeamId = 27,
                        HomeTeamName = "TeamNameAC",
                        AwayTeamId = 28,
                        AwayTeamName = "TeamNameAD",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(13),
                    },
                    new GameResultDto
                    {
                        Id         = 15,
                        GameNumber = 15,
                        HomeTeamId = 29,
                        HomeTeamName = "TeamNameAE",
                        AwayTeamId = 30,
                        AwayTeamName = "TeamNameAF",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(14),
                    },
                    new GameResultDto
                    {
                        Id         = 16,
                        GameNumber = 16,
                        HomeTeamId = 31,
                        HomeTeamName = "TeamNameAG",
                        AwayTeamId = 32,
                        AwayTeamName = "TeamNameAH",
                        Round      = 1,
                        GameDate   = round1Date.AddHours(15),
                    },
                });
            #endregion

            #region Round 2 (Round of 16)

            var round2Date = round1Date.AddDays(1);
            _gameResults.AddRange(
                new List<GameResultDto>
                {
                    new GameResultDto
                    {
                        Id         = 17,
                        GameNumber = 17,
                        Round      = 2,
                        GameDate = round2Date,
                    },
                    new GameResultDto
                    {
                        Id         = 18,
                        GameNumber = 18,
                        Round      = 2,
                        GameDate   = round2Date.AddHours(1),
                    },
                    new GameResultDto
                    {
                        Id         = 19,
                        GameNumber = 19,
                        Round      = 2,
                        GameDate   = round2Date.AddHours(2),
                    },
                    new GameResultDto
                    {
                        Id         = 20,
                        GameNumber = 20,
                        Round      = 2,
                        GameDate   = round2Date.AddHours(3),
                    },
                    new GameResultDto
                    {
                        Id         = 21,
                        GameNumber = 21,
                        Round      = 2,
                        GameDate   = round2Date.AddHours(4),
                    },
                    new GameResultDto
                    {
                        Id         = 22,
                        GameNumber = 22,
                        Round      = 2,
                        GameDate   = round2Date.AddHours(5),
                    },
                    new GameResultDto
                    {
                        Id         = 23,
                        GameNumber = 23,
                        Round      = 2,
                        GameDate   = round2Date.AddHours(6),
                    },
                    new GameResultDto
                    {
                        Id         = 24,
                        GameNumber = 24,
                        Round      = 2,
                        GameDate   = round2Date.AddHours(7),
                    },
                });

            #endregion

            #region Round 3 (Quarterfinals)

            var round3Date = round1Date.AddDays(2);
            _gameResults.AddRange(
                new List<GameResultDto>
                {
                    new GameResultDto
                    {
                        Id         = 25,
                        GameNumber = 25,
                        Round      = 3,
                        GameDate = round3Date,
                    },
                    new GameResultDto
                    {
                        Id         = 26,
                        GameNumber = 26,
                        Round      = 3,
                        GameDate   = round3Date.AddHours(1),
                    },
                    new GameResultDto
                    {
                        Id         = 27,
                        GameNumber = 27,
                        Round      = 3,
                        GameDate   = round3Date.AddHours(2),
                    },
                    new GameResultDto
                    {
                        Id         = 28,
                        GameNumber = 28,
                        Round      = 3,
                        GameDate   = round3Date.AddHours(3),
                    },
                });

            #endregion

            #region Round 4 (Semifinals)

            var round4Date = round1Date.AddDays(3);
            _gameResults.AddRange(
                new List<GameResultDto>
                {
                    new GameResultDto
                    {
                        Id         = 29,
                        GameNumber = 29,
                        Round      = 4,
                        GameDate   = round4Date,
                    },
                    new GameResultDto
                    {
                        Id         = 30,
                        GameNumber = 30,
                        Round      = 4,
                        GameDate   = round4Date.AddHours(1),
                    }
                });

            #endregion

            #region Round 5 (Finals)

            var round5Date = round1Date.AddDays(4);
            _gameResults.AddRange(
                new List<GameResultDto>
                {
                    new GameResultDto
                    {
                        Id         = 31,
                        GameNumber = 31,
                        Round      = 5,
                        GameDate   = round5Date,
                    },
                    new GameResultDto
                    {
                        Id         = 32,
                        GameNumber = 32,
                        Round      = 5,
                        GameDate   = round5Date.AddHours(1),
                    }
                });

            #endregion

            // Setup common properties
            _gameResults.ForEach(gr =>
            {
                gr.TournamentId = 1;
                gr.DivisionId = 1;
                gr.DivisionName = "Division 1";
                gr.GroupId = 1;
                gr.Result = new Result();
            });

            return this;
        }

        public GameServiceTestFixture TestPlayoffWithFirstRoundScheduledOnly()
        {
            _gameResults.Clear();

            var round1Date = DateTime.Parse(DATE_PLAYOFF_START);
            _gameResults.AddRange(
                new List<GameResultDto>
                {
                    new GameResultDto
                    {
                        Id           = 1,
                        GameNumber   = 1,
                        HomeTeamId   = 1,
                        HomeTeamName = "TeamNameA",
                        AwayTeamId   = 2,
                        AwayTeamName = "TeamNameB",
                        Round        = 1,
                        GameDate     = round1Date,
                    },
                    new GameResultDto
                    {
                        Id           = 2,
                        GameNumber   = 2,
                        HomeTeamId   = 3,
                        HomeTeamName = "TeamNameC",
                        AwayTeamId   = 4,
                        AwayTeamName = "TeamNameD",
                        Round        = 1,
                        GameDate     = round1Date.AddHours(1),
                    },
                    new GameResultDto
                    {
                        Id           = 3,
                        GameNumber   = 3,
                        HomeTeamId   = 5,
                        HomeTeamName = "TeamNameE",
                        AwayTeamId   = 6,
                        AwayTeamName = "TeamNameF",
                        Round        = 1,
                        GameDate     = round1Date.AddHours(2),
                    },
                    new GameResultDto
                    {
                        Id           = 4,
                        GameNumber   = 4,
                        HomeTeamId   = 7,
                        HomeTeamName = "TeamNameG",
                        AwayTeamId   = 8,
                        AwayTeamName = "TeamNameH",
                        Round        = 1,
                        GameDate     = round1Date.AddHours(3),
                    },
                });

            _gameResults.AddRange(
                new List<GameResultDto>
                {
                    new GameResultDto
                    {
                        Id         = 29,
                        GameNumber = 29,
                        Round      = 4,
                    },
                    new GameResultDto
                    {
                        Id         = 30,
                        GameNumber = 30,
                        Round      = 4,
                    }
                });

            _gameResults.AddRange(
                new List<GameResultDto>
                {
                    new GameResultDto
                    {
                        Id         = 31,
                        GameNumber = 31,
                        Round      = 5,
                    },
                    new GameResultDto
                    {
                        Id         = 32,
                        GameNumber = 32,
                        Round      = 5,
                    }
                });

            // Setup common properties
            _gameResults.ForEach(gr =>
            {
                gr.TournamentId = 1;
                gr.DivisionId = 1;
                gr.DivisionName = "Division 1";
                gr.GroupId = 1;
                gr.Result = new Result();
            });

            return this;
        }

        public GameServiceTestFixture TestMinimumEvenEmptyGamesPlayoff()
        {
            _gameResults.Clear();

            _gameResults.AddRange(
                new List<GameResultDto>
                {
                    new GameResultDto
                    {
                        Id             = 1,
                        HomeTeamId     = 1,
                        AwayTeamId     = 2,
                        GameNumber     = 1,
                        Round          = 1,
                        TournamentId   = 1,
                        UrlToGameVideo = URL_A,
                    },
                    new GameResultDto
                    {
                        Id             = 2,
                        HomeTeamId     = 3,
                        AwayTeamId     = 4,
                        GameNumber     = 2,
                        Round          = 1,
                        TournamentId   = 1,
                        UrlToGameVideo = URL_B,
                    },
                    new GameResultDto
                    {
                        Id           = 3,
                        Round        = 2,
                        GameNumber   = 3,
                        TournamentId = 1,
                    },
                    new GameResultDto
                    {
                        Id           = 4,
                        Round        = 2,
                        GameNumber   = 4,
                        TournamentId = 1,
                    },
                });

            return this;
        }

        public GameServiceTestFixture TestMinimumOddTeamsPlayOffSchedule()
        {
            _gameResults.Clear();

            _gameResults.AddRange(
                new List<GameResultDto>
                {
                    new GameResultDto
                    {
                        Id             = 1,
                        HomeTeamId     = 3,
                        AwayTeamId     = null,
                        GameNumber     = 2,
                        Round          = 1,
                        TournamentId   = 1,
                        UrlToGameVideo = URL_A,
                    },
                    new GameResultDto
                    {
                        Id             = 2,
                        HomeTeamId     = 1,
                        AwayTeamId     = 2,
                        GameNumber     = 1,
                        Round          = 1,
                        TournamentId   = 1,
                        UrlToGameVideo = URL_B,
                    },
                    new GameResultDto
                    {
                        Id             = 3,
                        Round          = 2,
                        HomeTeamId     = 1,
                        AwayTeamId     = 3,
                        GameNumber     = 3,
                        TournamentId   = 1,
                        UrlToGameVideo = URL_C,
                    },
                });

            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="GameServiceTestFixture"/>.
        /// </summary>
        /// <returns>Collection of <see cref="GameResultDto"/> objects filled with test data.</returns>
        public List<GameResultDto> Build()
        {
            return _gameResults;
        }

        public GameServiceTestFixture WithOneWeekOneDivisionOneGame()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = new Score(3, 0, true),
                    SetScores = new List<Score>
                    {
                        new Score(25, 0),
                        new Score(25, 0),
                        new Score(25, 0),
                        new Score(0,  0),
                        new Score(0,  0),
                    },
                },
                GameDate = DateTime.Parse(DATE_A_1),
                Round = 1,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
                DivisionName = "Division Name",
                UrlToGameVideo = URL_A,
            });

            return this;
        }
    }
}