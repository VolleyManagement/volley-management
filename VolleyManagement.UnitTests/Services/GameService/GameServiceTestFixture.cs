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

        private const string DATE_D = "2016-04-13 10:00";

        private const string DATE_E = "2016-04-16 10:00";

        private const string DATE_F = "2016-04-19 10:00";

        private List<GameResultDto> _gameResults = new List<GameResultDto>();

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
                            Home = 25,
                            Away = 20,
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
                            Away = 30,
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
                            Home = 27,
                            Away = 25,
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
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameC",
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
                            Home = 26,
                            Away = 28,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 15,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 21,
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
                    GameScore = new Score
                    {
                        Home = 0,
                        Away = 3,
                        IsTechnicalDefeat = true
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 0,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 25,
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
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 0
            });

            return this;
        }

        public GameServiceTestFixture Add(GameResultDto newGameResult)
        {
            _gameResults.Add(newGameResult);
            return this;
        }

        public GameServiceTestFixture WithAllPossibleScores()
        {
            _gameResults.Clear();

            return WithHomeTeamWinningScores()
                .WithAwayTeamWinningScores();
        }

        public GameServiceTestFixture WithHomeTeamWinningScores()
        {
            _gameResults.Add(new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                Result = new Result
                {
                    GameScore = new Score
                    {
                        Home = 3,
                        Away = 0,
                        IsTechnicalDefeat = false
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 15,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 16,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 19,
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
            _gameResults.Add(new GameResultDto
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
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
                            Home = 24,
                            Away = 26,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 19,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 18,
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
                            Home = 18,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 10,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 22,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 15,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 12,
                            IsTechnicalDefeat = false
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 0
            });

            return this;
        }

        public GameServiceTestFixture WithAwayTeamWinningScores()
        {
            _gameResults.Add(new GameResultDto
            {
                Id = 4,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                Result = new Result
                {
                    GameScore = new Score
                    {
                        Home = 2,
                        Away = 3,
                        IsTechnicalDefeat = false
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 22,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 26,
                            Away = 24,
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
                            Home = 17,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 13,
                            Away = 25,
                            IsTechnicalDefeat = false
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_D),
                Round = 4,
                GameNumber = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 5,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 3,
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
                            Home = 24,
                            Away = 26,
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
                            Home = 23,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 13,
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
                GameDate = DateTime.Parse(DATE_E),
                Round = 5,
                GameNumber = 0
            });
            _gameResults.Add(new GameResultDto
            {
                Id = 6,
                TournamentId = 1,
                HomeTeamId = 2,
                AwayTeamId = 3,
                Result = new Result
                {
                    GameScore = new Score
                    {
                        Home = 0,
                        Away = 3,
                        IsTechnicalDefeat = false
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 14,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 27,
                            Away = 29,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 22,
                            Away = 25,
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
                GameDate = DateTime.Parse(DATE_F),
                Round = 6,
                GameNumber = 0
            });

            return this;
        }

        public GameServiceTestFixture WithNoLostSetsForOneTeam()
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
                    GameScore = new Score
                    {
                        Home = 3,
                        Away = 0,
                        IsTechnicalDefeat = false
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 20,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 26,
                            Away = 24,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 30,
                            Away = 28,
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

            return this;
        }

        public GameServiceTestFixture WithNoLostSetsNoLostBallsForOneTeam()
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
                GameNumber = 0
            });

            return this;
        }

        public GameServiceTestFixture WithResultsForUniquePoints()
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

        public GameServiceTestFixture WithResultsForSamePoints()
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
                        (29,27),
                        (25,23),
                        (25,23),
                        (0,0),
                        (0,0)
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
                        (25,21),
                        (26,28),
                        (25,23),
                        (26,28),
                        (15,10)
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
                        (25,21),
                        (26,28),
                        (25,23),
                        (26,28),
                        (15,10)
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
                        (25,21),
                        (26,28),
                        (25,23),
                        (26,28),
                        (10,15)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
            });

            return this;
        }

        public GameServiceTestFixture WithResultsForSamePointsAndWonGames()
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
                        (19,25),
                        (29,27),
                        (25,23),
                        (25,23),
                        (0,0)
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
                        (25,21),
                        (26,28),
                        (25,23),
                        (0,0),
                        (0,0)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
            });

            return this;
        }

        public GameServiceTestFixture WithResultsForSamePointsWonGamesAndSetsRatio()
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
                        (25,21),
                        (26,28),
                        (25,23),
                        (25,10),
                        (0,0)
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
                        (25,21),
                        (26,28),
                        (25,23),
                        (28,26),
                        (0,0)
                    }
                },
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
            });

            return this;
        }

        public GameServiceTestFixture WithTechnicalDefeatInSet()
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
                            Home = 25,
                            Away = 20,
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
                            Away = 30,
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
                            Home = 27,
                            Away = 25,
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
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameC",
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
                            Home = 26,
                            Away = 28,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 15,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 21,
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
                    GameScore = new Score
                    {
                        Home = 0,
                        Away = 3,
                        IsTechnicalDefeat = false
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 10,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 10,
                            Away = 25,
                            IsTechnicalDefeat = false
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 25,
                            IsTechnicalDefeat = true
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
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 0
            });

            return this;
        }

        /// <summary>
        /// Generates <see cref="GameResultDto"/> objects filled with test data.
        /// </summary>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/>.</returns>
        public GameServiceTestFixture WithHomeTeamPenalty()
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
                    SetsScore = new Score
                    {
                        Home = 3,
                        Away = 2,
                        IsTechnicalDefeat = false,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 20,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 24,
                            Away = 26,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 28,
                            Away = 30,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 22,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 27,
                            Away = 25,
                            IsTechnicalDefeat = false,
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
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameC",
                Result = new Result
                {
                    SetsScore = new Score
                    {
                        Home = 3,
                        Away = 1,
                        IsTechnicalDefeat = false,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 26,
                            Away = 28,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 15,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 21,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 29,
                            Away = 27,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false,
                        }
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
                    SetsScore = new Score
                    {
                        Home = 0,
                        Away = 3,
                        IsTechnicalDefeat = false,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 10,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 10,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false,
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 0
            });

            return this;
        }

        /// <summary>
        /// Generates <see cref="GameResultDto"/> objects filled with test data.
        /// </summary>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/>.</returns>
        public GameServiceTestFixture WithAwayTeamPenalty()
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
                    SetsScore = new Score
                    {
                        Home = 3,
                        Away = 2,
                        IsTechnicalDefeat = false,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 20,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 24,
                            Away = 26,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 28,
                            Away = 30,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 22,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 27,
                            Away = 25,
                            IsTechnicalDefeat = false,
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
                HomeTeamId = 1,
                AwayTeamId = 3,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "TeamNameC",
                Result = new Result
                {
                    SetsScore = new Score
                    {
                        Home = 3,
                        Away = 1,
                        IsTechnicalDefeat = false,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 26,
                            Away = 28,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 15,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 21,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 29,
                            Away = 27,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false,
                        }
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
                    SetsScore = new Score
                    {
                        Home = 0,
                        Away = 3,
                        IsTechnicalDefeat = false,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 10,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 10,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false,
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_C),
                Round = 3,
                GameNumber = 0
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
    }
}
