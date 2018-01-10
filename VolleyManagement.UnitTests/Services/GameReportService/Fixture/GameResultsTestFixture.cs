namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GamesAggregate;

    /// <summary>
    /// Generates test data for <see cref="GameResultDto"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class GameResultsTestFixture
    {
        private const string DATE_A = "2016-04-03 10:00";

        private const string DATE_B = "2016-04-07 10:00";

        private const string DATE_C = "2016-04-10 10:00";

        private const string DATE_D = "2016-04-13 10:00";

        private const string DATE_E = "2016-04-16 10:00";

        private const string DATE_F = "2016-04-19 10:00";

        private readonly List<GameResultDto> _gameResults = new List<GameResultDto>();

        public GameResultsTestFixture WithNoGameResults()
        {
            _gameResults.Clear();

            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                Result = null,
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
                Result = null,
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 1,
                HomeTeamId = 2,
                AwayTeamId = null,
                Result = null,
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 0
            });

            return this;
        }

        public GameResultsTestFixture WithNoGamesScheduled()
        {
            _gameResults.Clear();

            return this;
        }

        public GameResultsTestFixture WithAllPossibleScores()
        {
            _gameResults.Clear();

            return WithHomeTeamWinningScores()
                .WithAwayTeamWinningScores();
        }

        public GameResultsTestFixture WithHomeTeamWinningScores()
        {
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                Result = new Result
                {
                    GameScore = (3, 0),
                    SetScores = new List<Score>
                    {
                        (25, 15),
                        (25, 16),
                        (25, 19),
                        (0, 0),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 1
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
                Result = new Result
                {
                    GameScore = (3, 1),
                    SetScores = new List<Score>
                    {
                        (24, 26),
                        (25, 19),
                        (25, 18),
                        (25, 23),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 2
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = 2,
                AwayTeamId = 3,
                Result = new Result
                {
                    GameScore = (3, 2),
                    SetScores = new List<Score>
                    {
                        (18, 25),
                        (25, 10),
                        (22, 25),
                        (25, 15),
                        (25, 12)
                    }
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 3
            });

            return this;
        }

        public GameResultsTestFixture WithAwayTeamWinningScores()
        {
            _gameResults.Add(new GameResultDto
            {
                Id = 4,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                Result = new Result
                {
                    GameScore = (2, 3),
                    SetScores = new List<Score>
                    {
                        (25, 22),
                        (26, 24),
                        (23, 25),
                        (17, 25),
                        (13, 25)
                    }
                },
                GameDate = DateTime.Parse(DATE_D),
                Round = 4,
                GameNumber = 4
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 5,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
                Result = new Result
                {
                    GameScore = (1, 3),
                    SetScores = new List<Score>
                    {
                        (24, 26),
                        (25, 22),
                        (23, 25),
                        (13, 25),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_E),
                Round = 5,
                GameNumber = 5
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 6,
                TournamentId = 1,
                HomeTeamId = 2,
                AwayTeamId = 3,
                Result = new Result
                {
                    GameScore = (0, 3),
                    SetScores = new List<Score>
                    {
                        (14,25),
                        (27,29),
                        (22,25),
                        (0,0),
                        (0,0)
                    }
                },
                GameDate = DateTime.Parse(DATE_F),
                Round = 6,
                GameNumber = 6
            });

            return this;
        }

        public GameResultsTestFixture WithNoLostSetsForOneTeam()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                Result = new Result
                {
                    GameScore = (3, 0),
                    SetScores = new List<Score>
                    {
                        (25, 20),
                        (26, 24),
                        (30, 28),
                        (0, 0),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 1
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 2,
                Result = new Result
                {
                    GameScore = (3, 1),
                    SetScores = new List<Score>
                    {
                        (25, 20),
                        (26, 24),
                        (26, 28),
                        (25, 10),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 2
            });

            return this;
        }

        public GameResultsTestFixture WithNoLostSetsNoLostBallsForOneTeam()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                Result = new Result
                {
                    GameScore = (3, 0),
                    SetScores = new List<Score>
                    {
                        (25, 0),
                        (25, 0),
                        (25, 0),
                        (0, 0),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 1
            });

            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 2,
                Result = new Result
                {
                    GameScore = (3, 0),
                    SetScores = new List<Score>
                    {
                        (25, 20),
                        (26, 24),
                        (30, 28),
                        (0, 0),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 2,
                GameNumber = 2
            });

            return this;
        }

        public GameResultsTestFixture WithResultsForUniquePoints()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 1,
                HomeTeamName = "TeamNameC",
                AwayTeamName = "TeamNameA",
                Result = new Result
                {
                    GameScore = new Score
                    {
                        Home = 3,
                        Away = 1,
                        IsTechnicalDefeat = false
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 21,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 24,
                            Away = 26,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 28,
                            Away = 26,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 23,
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
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 2,
                AwayTeamId = 1,
                HomeTeamName = "TeamNameB",
                AwayTeamName = "TeamNameA",
                Result = new Result
                {
                    GameScore = new Score
                    {
                        Home = 3,
                        Away = 2,
                        IsTechnicalDefeat = false
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 19,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 23,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 29,
                            Away = 27,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 22,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 23,
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

        public GameResultsTestFixture WithResultsForSamePoints()
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
                    GameScore = (3, 0),
                    SetScores = new List<Score>
                    {
                        (29, 27),
                        (25, 23),
                        (25, 23),
                        (0, 0),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 1,
                GameNumber = 0
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
                    GameScore = (3, 2),
                    SetScores = new List<Score>
                    {
                        (25, 21),
                        (26, 28),
                        (25, 23),
                        (26, 28),
                        (15, 10)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameC",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (3, 2),
                    SetScores = new List<Score>
                    {
                        (25, 21),
                        (26, 28),
                        (25, 23),
                        (26, 28),
                        (15, 10)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 4,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (2, 3),
                    SetScores = new List<Score>
                    {
                        (25, 21),
                        (26, 28),
                        (25, 23),
                        (26, 28),
                        (10, 15)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
            });

            return this;
        }

        public GameResultsTestFixture WithResultsForSamePointsAndWonGames()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (3, 1),
                    SetScores = new List<Score>
                    {
                        (19, 25),
                        (29, 27),
                        (25, 23),
                        (25, 23),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameC",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (3, 0),
                    SetScores = new List<Score>
                    {
                        (25, 21),
                        (26, 28),
                        (25, 23),
                        (0, 0),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
            });

            return this;
        }

        public GameResultsTestFixture WithResultsForSamePointsWonGamesAndSetsRatio()
        {
            _gameResults.Clear();
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 3,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameC",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (3, 1),
                    SetScores = new List<Score>
                    {
                        (25, 21),
                        (26, 28),
                        (25, 23),
                        (25, 10),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 1,
                GameNumber = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameB",
                Result = new Result
                {
                    GameScore = (3, 1),
                    SetScores = new List<Score>
                    {
                        (25, 21),
                        (26, 28),
                        (25, 23),
                        (28, 26),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
            });

            return this;
        }

        public GameResultsTestFixture WithTechnicalDefeatInSet()
        {
            // Third game has set with technical defeat
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
                        (24, 60),
                        (28, 30),
                        (25, 22),
                        (27, 25)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
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
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 0
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
                    GameScore = (1, 3),
                    SetScores = new List<Score>
                    {
                        (10, 25),
                        (10, 25),
                        (25, 0, true),
                        (10, 25),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 0
            });

            return this;
        }

        public GameResultsTestFixture WithTechnicalDefeatInGame()
        {
            // Third game has set with technical defeat
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
                        (24, 60),
                        (28, 30),
                        (25, 22),
                        (27, 25)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
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
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 0
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
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 0
            });

            return this;
        }

        public GameResultsTestFixture WithHomeTeamPenalty()
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
                        (27, 25)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
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
                        (0, 0)
                    },
                    Penalty = new Penalty
                    {
                        IsHomeTeam = true,
                        Amount = 2,
                        Description = "Penalty reason"
                    }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 0
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
                    GameScore = (0, 3),
                    SetScores = new List<Score>
                    {
                        (10, 25),
                        (10, 25),
                        (0, 25),
                        (0, 0),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 0
            });

            return this;
        }

        public GameResultsTestFixture WithAwayTeamPenalty()
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
                        (27, 25)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
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
                        (0, 0)
                    },
                    Penalty = new Penalty
                    {
                        IsHomeTeam = false,
                        Amount = 2,
                        Description = "Penalty reason"
                    }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 0
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
                    GameScore = (0, 3),
                    SetScores = new List<Score>
                    {
                        (10, 25),
                        (10, 25),
                        (0, 25),
                        (0, 0),
                        (0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 0
            });

            return this;
        }

        public GameResultsTestFixture WithMultipleDivisionsAllPosibleScores()
        {
            AddDivision1Group1Results();
            AddDivision1Group2Results();
            AddDivision2Group1Results();

            return this;
        }

        private void AddDivision1Group1Results()
        {
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                Result = new Result
                {
                    GameScore = (3, 0),
                    SetScores = new List<Score> {(25, 15), (25, 16), (25, 19), (0, 0), (0, 0)}
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 1
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
                Result = new Result
                {
                    GameScore = (3, 1),
                    SetScores = new List<Score> {(24, 26), (25, 19), (25, 18), (25, 23), (0, 0)}
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 2
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 1,
                HomeTeamId = 2,
                AwayTeamId = 3,
                Result = new Result
                {
                    GameScore = (3, 2),
                    SetScores = new List<Score> {(18, 25), (25, 10), (22, 25), (25, 15), (25, 12)}
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 3
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 4,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                Result = new Result
                {
                    GameScore = (2, 3),
                    SetScores = new List<Score> {(25, 22), (26, 24), (23, 25), (17, 25), (13, 25)}
                },
                GameDate = DateTime.Parse(DATE_D),
                Round = 4,
                GameNumber = 4
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 5,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
                Result = new Result
                {
                    GameScore = (1, 3),
                    SetScores = new List<Score> {(24, 26), (25, 22), (23, 25), (13, 25), (0, 0)}
                },
                GameDate = DateTime.Parse(DATE_E),
                Round = 5,
                GameNumber = 5
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 6,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 1,
                HomeTeamId = 2,
                AwayTeamId = 3,
                Result = new Result
                {
                    GameScore = (0, 3),
                    SetScores = new List<Score> {(14, 25), (27, 29), (22, 25), (0, 0), (0, 0)}
                },
                GameDate = DateTime.Parse(DATE_F),
                Round = 6,
                GameNumber = 6
            });
        }

        private void AddDivision1Group2Results()
        {
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 2,
                HomeTeamId = 5,
                AwayTeamId = 6,
                Result = new Result
                {
                    GameScore = (3, 0),
                    SetScores = new List<Score> { (25, 15), (25, 16), (25, 19), (0, 0), (0, 0) }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 1
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 2,
                HomeTeamId = 5,
                AwayTeamId = 4,
                Result = new Result
                {
                    GameScore = (3, 1),
                    SetScores = new List<Score> { (24, 26), (25, 19), (25, 18), (25, 23), (0, 0) }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 2
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 2,
                HomeTeamId = 6,
                AwayTeamId = 4,
                Result = new Result
                {
                    GameScore = (3, 2),
                    SetScores = new List<Score> { (18, 25), (25, 10), (22, 25), (25, 15), (25, 12) }
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 3
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 4,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 2,
                HomeTeamId = 5,
                AwayTeamId = 6,
                Result = new Result
                {
                    GameScore = (2, 3),
                    SetScores = new List<Score> { (25, 22), (26, 24), (23, 25), (17, 25), (13, 25) }
                },
                GameDate = DateTime.Parse(DATE_D),
                Round = 4,
                GameNumber = 4
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 5,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 2,
                HomeTeamId = 5,
                AwayTeamId = 4,
                Result = new Result
                {
                    GameScore = (1, 3),
                    SetScores = new List<Score> { (24, 26), (25, 22), (23, 25), (13, 25), (0, 0) }
                },
                GameDate = DateTime.Parse(DATE_E),
                Round = 5,
                GameNumber = 5
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 6,
                TournamentId = 1,
                DivisionId = 1,
                GroupId = 2,
                HomeTeamId = 6,
                AwayTeamId = 4,
                Result = new Result
                {
                    GameScore = (0, 3),
                    SetScores = new List<Score> { (14, 25), (27, 29), (22, 25), (0, 0), (0, 0) }
                },
                GameDate = DateTime.Parse(DATE_F),
                Round = 6,
                GameNumber = 6
            });
        }

        private void AddDivision2Group1Results()
        {
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                DivisionId = 2,
                GroupId = 3,
                HomeTeamId = 7,
                AwayTeamId = 8,
                Result = new Result
                {
                    GameScore = (3, 0),
                    SetScores = new List<Score> { (25, 15), (25, 16), (25, 19), (0, 0), (0, 0) }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 1
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                DivisionId = 2,
                GroupId = 3,
                HomeTeamId = 7,
                AwayTeamId = 9,
                Result = new Result
                {
                    GameScore = (3, 1),
                    SetScores = new List<Score> { (24, 26), (25, 19), (25, 18), (25, 23), (0, 0) }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 2
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 3,
                TournamentId = 1,
                DivisionId = 2,
                GroupId = 3,
                HomeTeamId = 8,
                AwayTeamId = 9,
                Result = new Result
                {
                    GameScore = (3, 2),
                    SetScores = new List<Score> { (18, 25), (25, 10), (22, 25), (25, 15), (25, 12) }
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 3
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 4,
                TournamentId = 1,
                DivisionId = 2,
                GroupId = 3,
                HomeTeamId = 7,
                AwayTeamId = 8,
                Result = new Result
                {
                    GameScore = (2, 3),
                    SetScores = new List<Score> { (25, 22), (26, 24), (23, 25), (17, 25), (13, 25) }
                },
                GameDate = DateTime.Parse(DATE_D),
                Round = 4,
                GameNumber = 4
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 5,
                TournamentId = 1,
                DivisionId = 2,
                GroupId = 3,
                HomeTeamId = 7,
                AwayTeamId = 9,
                Result = new Result
                {
                    GameScore = (1, 3),
                    SetScores = new List<Score> { (24, 26), (25, 22), (23, 25), (13, 25), (0, 0) }
                },
                GameDate = DateTime.Parse(DATE_E),
                Round = 5,
                GameNumber = 5
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 6,
                TournamentId = 1,
                DivisionId = 2,
                GroupId = 3,
                HomeTeamId = 8,
                AwayTeamId = 9,
                Result = new Result
                {
                    GameScore = (0, 3),
                    SetScores = new List<Score> { (14, 25), (27, 29), (22, 25), (0, 0), (0, 0) }
                },
                GameDate = DateTime.Parse(DATE_F),
                Round = 6,
                GameNumber = 6
            });
        }

        public List<GameResultDto> Build()
        {
            return _gameResults;
        }
    }
}