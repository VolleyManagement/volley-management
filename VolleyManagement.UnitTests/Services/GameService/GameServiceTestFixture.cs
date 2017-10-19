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

        /// <summary>
        /// Generates <see cref="GameResultDto"/> objects filled with test data.
        /// </summary>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/>.</returns>
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
                        IsTechnicalDefeat = true,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 0,
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
        /// Adds <see cref="GameResultDto"/> object to collection.
        /// </summary>
        /// <param name="newGameResult"><see cref="GameResultDto"/> object to add.</param>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/>.</returns>
        public GameServiceTestFixture Add(GameResultDto newGameResult)
        {
            _gameResults.Add(newGameResult);
            return this;
        }

        /// <summary>
        /// Adds game results with all possible scores.
        /// to collection of <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/></returns>
        public GameServiceTestFixture WithAllPossibleScores()
        {
            _gameResults.Clear();

            return WithHomeTeamWinningScores()
                .WithAwayTeamWinningScores();
        }

        /// <summary>
        /// Adds game results where home team wins with all possible scores
        /// </summary>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/></returns>
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
                    SetsScore = new Score
                    {
                        Home = 3,
                        Away = 0,
                        IsTechnicalDefeat = false,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 15,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 16,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 19,
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
                            Home = 24,
                            Away = 26,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 19,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 18,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 23,
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
                            Home = 18,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 10,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 22,
                            Away = 25,
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
                            Away = 12,
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
        /// Adds game results where away team wins with all possible scores
        /// </summary>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/></returns>
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
                    SetsScore = new Score
                    {
                        Home = 2,
                        Away = 3,
                        IsTechnicalDefeat = false,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 22,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 26,
                            Away = 24,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 23,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 17,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 13,
                            Away = 25,
                            IsTechnicalDefeat = false,
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
                    SetsScore = new Score
                    {
                        Home = 1,
                        Away = 3,
                        IsTechnicalDefeat = false,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 24,
                            Away = 26,
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
                            Home = 23,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 13,
                            Away = 25,
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
                            Home = 14,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 27,
                            Away = 29,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 22,
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
                GameDate = DateTime.Parse(DATE_F),
                Round = 6,
                GameNumber = 0
            });

            return this;
        }

        /// <summary>
        /// Adds game result with no lost sets for one team to collection of <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/>.</returns>
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
                    SetsScore = new Score
                    {
                        Home = 3,
                        Away = 0,
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
                            Home = 26,
                            Away = 24,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 30,
                            Away = 28,
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
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
            });

            return this;
        }

        /// <summary>
        /// Adds game result with no lost sets and no lost balls for one team to collection of <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/>.</returns>
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
                    SetsScore = new Score
                    {
                        Home = 3,
                        Away = 0,
                        IsTechnicalDefeat = true,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 0,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 0,
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
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 0
            });

            return this;
        }

        /// <summary>
        /// Adds game results which result in unique points for the teams to collection of <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/>.</returns>
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
                            Home = 25,
                            Away = 21,
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
                            Away = 26,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 23,
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
                            Home = 19,
                            Away = 25,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 23,
                            Away = 25,
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
                            Home = 25,
                            Away = 22,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 23,
                            IsTechnicalDefeat = false,
                        }
                    }
                },
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 0
            });

            return this;
        }

        /// <summary>
        /// Adds game results which result in repetitive points for the teams to collection of <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/>.</returns>
        public GameServiceTestFixture WithResultsForRepetitivePoints()
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
                    SetsScore = new Score
                    {
                        Home = 3,
                        Away = 0,
                        IsTechnicalDefeat = false,
                    },
                    SetScores = new List<Score>
                    {
                        new Score
                        {
                            Home = 25,
                            Away = 21,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 28,
                            Away = 26,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 23,
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
                            Home = 19,
                            Away = 25,
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
                            Home = 25,
                            Away = 22,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 23,
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
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
                GameNumber = 0
            });

            return this;
        }

        /// <summary>
        /// Adds game results which result in repetitive points and sets ratio for the teams
        /// to collection of <see cref="GameResultDto"/> objects.
        /// </summary>
        /// <returns>Instance of <see cref="GameServiceTestFixture"/>.</returns>
        public GameServiceTestFixture WithResultsForRepetitivePointsAndSetsRatio()
        {
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
                            Home = 25,
                            Away = 21,
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
                            Away = 26,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 23,
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
                            Home = 19,
                            Away = 25,
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
                            Home = 25,
                            Away = 22,
                            IsTechnicalDefeat = false,
                        },
                        new Score
                        {
                            Home = 25,
                            Away = 23,
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
                GameDate = DateTime.Parse(DATE_B),
                Round = 2,
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
                        SetsScore = new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false,
                        },
                        SetScores = new List<Score>
                    {
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
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false,
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
                       SetsScore = new Score
                       {
                           Home = 0,
                           Away = 0,
                           IsTechnicalDefeat = false,
                       },
                       SetScores = new List<Score>
                    {
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
                        },
                        new Score
                        {
                            Home = 0,
                            Away = 0,
                            IsTechnicalDefeat = false,
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
                        TournamentId = 1,
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
