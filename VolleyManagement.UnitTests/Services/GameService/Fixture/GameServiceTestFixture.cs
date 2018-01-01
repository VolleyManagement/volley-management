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
        private const string DATE_A = "2016-04-03 10:00";

        private const string DATE_B = "2016-04-07 10:00";

        private const string DATE_C = "2016-04-10 10:00";

        private const string DATE_D = "2016-04-02 10:00";

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
                        (25, 20), (24, 26), (28, 30), (25, 22), (27, 25),
                    },
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
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
                        (26, 28), (25, 15), (25, 21), (29, 27), (0, 0),
                    },
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
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
                        (0, 25), (0, 25), (0, 25), (0, 0), (0, 0),
                    },
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
            });

            return this;
        }

        public GameServiceTestFixture TestGamesForDuplicateSchemeOne()
        {
            _gameResults.Clear();

            _gameResults.Add(new GameResultDto()
            {
                Id = 1,
                GameDate = DateTime.Parse(DATE_A),
                HomeTeamId = 1,
                AwayTeamId = 2,
                TournamentId = 1,
                Round = 1,
                GameNumber = 0
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
                    GameDate = DateTime.Parse(DATE_A),
                    HomeTeamId = 1,
                    AwayTeamId = 2,
                    TournamentId = 1,
                    Round = 1,
                    GameNumber = 0
                });
            _gameResults.Add(
               new GameResultDto()
               {
                   Id = 2,
                   GameDate = DateTime.Parse(DATE_A),
                   HomeTeamId = 3,
                   AwayTeamId = 4,
                   TournamentId = 1,
                   Round = 1,
                   GameNumber = 0
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
                    GameDate = DateTime.Parse(DATE_A),
                    HomeTeamId = 1,
                    AwayTeamId = 2,
                    TournamentId = 1,
                    Round = 1,
                    GameNumber = 0
                });
            _gameResults.Add(
               new GameResultDto()
               {
                   Id = 2,
                   GameDate = DateTime.Parse(DATE_A),
                   HomeTeamId = 2,
                   AwayTeamId = 1,
                   TournamentId = 1,
                   Round = 2,
                   GameNumber = 0
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
                    GameDate = DateTime.Parse(DATE_A),
                    HomeTeamId = 1,
                    AwayTeamId = null,
                    TournamentId = 1,
                    Round = 1,
                    GameNumber = 0
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
                    GameDate = DateTime.Parse(DATE_A),
                    HomeTeamId = 1,
                    AwayTeamId = null,
                    TournamentId = 1,
                    Round = 1,
                    GameNumber = 0
                });
            _gameResults.Add(
               new GameResultDto()
               {
                   Id = 2,
                   GameDate = DateTime.Parse(DATE_A),
                   HomeTeamId = 1,
                   AwayTeamId = null,
                   TournamentId = 1,
                   Round = 2,
                   GameNumber = 0
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
                            IsTechnicalDefeat = false
                        },
                        SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        }
                    }
                    },
                    GameDate = DateTime.Parse(DATE_A),
                    Round = 1,
                    GameNumber = 0
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
                       GameScore = new Score
                       {
                           Home = 0,
                           Away = 0,
                           IsTechnicalDefeat = false
                       },
                       SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        }
                    }
                   },
                   GameDate = DateTime.Parse(DATE_B),
                   Round = 2,
                   GameNumber = 0
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
                    GameScore = new Score
                    {
                        Home = 3,
                        Away = 0,
                        IsTechnicalDefeat = true
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
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
                    GameScore = new Score
                    {
                        Home = 1,
                        Away = 3,
                        IsTechnicalDefeat = false
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 5,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 5,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 5,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 0,
                GroupId = 2,
                DivisionId = 2,
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
                    GameScore = new Score
                    {
                        Home = 3,
                        Away = 0,
                        IsTechnicalDefeat = true
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
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
                    GameScore = new Score
                    {
                        Home = 1,
                        Away = 3,
                        IsTechnicalDefeat = false
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 5,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 5,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 5,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_D),
                Round = 2,
                GameNumber = 0,
                GroupId = 2,
                DivisionId = 2,
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
                    GameScore = new Score
                    {
                        Home = 3,
                        Away = 0,
                        IsTechnicalDefeat = true
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
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
                    GameScore = new Score
                    {
                        Home = 1,
                        Away = 3,
                        IsTechnicalDefeat = false
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 5,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 5,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 5,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 2,
                GameNumber = 0,
                GroupId = 2,
                DivisionId = 2,
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
                        Id = 1,
                        HomeTeamId = 1,
                        AwayTeamId = 2,
                        GameNumber = 1,
                        Round = 1,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 2,
                        HomeTeamId = 3,
                        AwayTeamId = 4,
                        GameNumber = 2,
                        Round = 1,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 3,
                        HomeTeamId = 5,
                        AwayTeamId = 6,
                        Round = 1,
                        GameNumber = 3,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 4,
                        Round = 1,
                        HomeTeamId = 7,
                        AwayTeamId = 8,
                        GameNumber = 4,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 5,
                        Round = 2,
                        GameNumber = 5,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 6,
                        Round = 2,
                        GameNumber = 6,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 7,
                        Round = 3,
                        GameNumber = 7,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 8,
                        Round = 3,
                        GameNumber = 8,
                        TournamentId = 1
                    }
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
                        Id = 1,
                        HomeTeamId = 1,
                        AwayTeamId = 2,
                        GameNumber = 1,
                        Round = 1,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 2,
                        HomeTeamId = 3,
                        AwayTeamId = 4,
                        GameNumber = 2,
                        Round = 1,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 3,
                        Round = 2,
                        GameNumber = 3,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 4,
                        Round = 2,
                        GameNumber = 4,
                        TournamentId = 1
                    }
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
                        Id = 1,
                        HomeTeamId = 3,
                        AwayTeamId = null,
                        GameNumber = 2,
                        Round = 1,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 2,
                        HomeTeamId = 1,
                        AwayTeamId = 2,
                        GameNumber = 1,
                        Round = 1,
                        TournamentId = 1
                    },
                    new GameResultDto
                    {
                        Id = 3,
                        Round = 2,
                        HomeTeamId = 1,
                        AwayTeamId = 3,
                        GameNumber = 3,
                        TournamentId = 1
                    }
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
                    GameScore = new Score
                    {
                        Home = 3,
                        Away = 0,
                        IsTechnicalDefeat = true
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0,
                GroupId = 1,
                DivisionId = 1,
            });

            return this;
        }
    }
}
