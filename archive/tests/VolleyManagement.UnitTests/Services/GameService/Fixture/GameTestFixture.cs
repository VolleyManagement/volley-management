namespace VolleyManagement.UnitTests.Services.GameService
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GamesAggregate;

    [ExcludeFromCodeCoverage]
    public class GameTestFixture
    {
        private const string DATE_A = "2016-04-03 10:00";

        private const string DATE_B = "2016-04-07 10:00";

        private const string DATE_C = "2016-04-10 10:00";

        private const string DATE_D = "2016-04-13 10:00";

        private const string DATE_PLAYOFF_START = "2018-02-26 02:00";

        private List<Game> _games = new List<Game>();

        /// <summary>
        /// Generates <see cref="Game"/> objects filled with test data.
        /// </summary>
        /// <returns>Instance of <see cref="GameTestFixture"/>.</returns>
        public GameTestFixture TestGames()
        {
            _games.Add(new Game 
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = null,
                AwayTeamId = null,
                Result = new Result(),
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 1
            });
            _games.Add(new Game 
            {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = null,
                AwayTeamId = null,
                Result = new Result(),
                GameDate = DateTime.Parse(DATE_B),
                Round = 1,
                GameNumber = 2
            });
            _games.Add(new Game 
            {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = null,
                AwayTeamId = null,
                Result = new Result(),
                GameDate = DateTime.Parse(DATE_C),
                Round = 2,
                GameNumber = 3
            });
            _games.Add(new Game 
            {
                Id = 4,
                TournamentId = 1,
                HomeTeamId = null,
                AwayTeamId = null,
                Result = new Result(),
                GameDate = DateTime.Parse(DATE_D),
                Round = 2,
                GameNumber = 4
            });

            return this;
        }

        /// <summary>
        /// Generates <see cref="Game"/> objects filled with test data.
        /// </summary>
        /// <returns>Instance of <see cref="GameTestFixture"/>.</returns>
        public GameTestFixture TestGamesWithResults()
        {
            var r=new Result(){GameScore = new Score(3,2)};
            _games.Add(new Game {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = null,
                AwayTeamId = null,
                Result = r,
                GameDate = DateTime.Parse(DATE_A),
                Round = 1,
                GameNumber = 1
            });
            _games.Add(new Game {
                Id = 2,
                TournamentId = 1,
                HomeTeamId = null,
                AwayTeamId = null,
                Result = r,
                GameDate = DateTime.Parse(DATE_B),
                Round = 1,
                GameNumber = 2
            });
            _games.Add(new Game {
                Id = 3,
                TournamentId = 1,
                HomeTeamId = null,
                AwayTeamId = null,
                Result = r,
                GameDate = DateTime.Parse(DATE_C),
                Round = 2,
                GameNumber = 3
            });
            _games.Add(new Game {
                Id = 4,
                TournamentId = 1,
                HomeTeamId = null,
                AwayTeamId = null,
                Result = r,
                GameDate = DateTime.Parse(DATE_D),
                Round = 2,
                GameNumber = 4
            });

            return this;
        }

        public GameTestFixture Add(Game game)
        {
            _games.Add(game);
            return this;
        }

        public GameTestFixture TestEmptyGamePlayoffSchedule()
        {
            _games.Clear();
            _games.AddRange(
                new List<Game>
                {
                    new Game
                    {
                        Id = 1,
                        HomeTeamId = 1,
                        AwayTeamId = 2,
                        GameNumber = 1,
                        Round = 1,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 2,
                        HomeTeamId = 3,
                        AwayTeamId = 4,
                        GameNumber = 2,
                        Round = 1,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 3,
                        HomeTeamId = 5,
                        AwayTeamId = 6,
                        Round = 1,
                        GameNumber = 3,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 4,
                        Round = 1,
                        HomeTeamId = 7,
                        AwayTeamId = 8,
                        GameNumber = 4,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 5,
                        Round = 2,
                        GameNumber = 5,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 6,
                        Round = 2,
                        GameNumber = 6,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 7,
                        Round = 3,
                        GameNumber = 7,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 8,
                        Round = 3,
                        GameNumber = 8,
                        TournamentId = 1,
                        Result = new Result()
                    }
                });

            return this;
        }

        public GameTestFixture TestMinimumEvenTeamsPlayOffSchedule()
        {
            _games.Clear();

            _games.AddRange(
                new List<Game>
                {
                    new Game
                    {
                        Id = 1,
                        HomeTeamId = 1,
                        AwayTeamId = 2,
                        GameNumber = 1,
                        Round = 1,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 2,
                        HomeTeamId = 3,
                        AwayTeamId = 4,
                        GameNumber = 2,
                        Round = 1,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 3,
                        Round = 2,
                        GameNumber = 3,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 4,
                        Round = 2,
                        GameNumber = 4,
                        TournamentId = 1,
                        Result = new Result()
                    }
                });

            return this;
        }

        public GameTestFixture SameTeamsInOneGamePlayOffScheme()
        {
            _games.Clear();

            _games.AddRange(
                new List<Game>
                {
                    new Game
                    {
                        Id = 1,
                        HomeTeamId = 1,
                        AwayTeamId = 1,
                        GameNumber = 1,
                        Round = 1,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 2,
                        HomeTeamId = 3,
                        AwayTeamId = 4,
                        GameNumber = 2,
                        Round = 1,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 3,
                        Round = 2,
                        GameNumber = 3,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 4,
                        Round = 2,
                        GameNumber = 4,
                        TournamentId = 1,
                        Result = new Result()
                    }
                });

            return this;
        }

        public GameTestFixture MinimalPlannedPlayOffWithPreliminaryStage()
        {
            _games.Clear();

            var round1Date = DateTime.Parse(DATE_PLAYOFF_START);
            _games.AddRange(
                new List<Game>
                {
                    new Game
                    {
                        Id = 1,
                        GameNumber = 1,
                        HomeTeamId = 1,
                        AwayTeamId = null,
                        Round = 1,
                        GameDate = round1Date,
                    },
                    new Game
                    {
                        Id = 2,
                        GameNumber = 2,
                        HomeTeamId = 3,
                        AwayTeamId = 4,
                        Round = 1,
                        GameDate = round1Date.AddHours(1),
                    },
                    new Game
                    {
                        Id = 3,
                        GameNumber = 3,
                        HomeTeamId = 5,
                        AwayTeamId = 6,
                        Round = 1,
                        GameDate = round1Date.AddHours(2),
                    },
                    new Game
                    {
                        Id = 4,
                        GameNumber = 4,
                        HomeTeamId = null,
                        AwayTeamId = 8,
                        Round = 1,
                        GameDate = round1Date.AddHours(3),
                    },
                });

            _games.AddRange(
                new List<Game>
                {
                    new Game
                    {
                        Id = 5,
                        GameNumber = 5,
                        Round = 4,
                    },
                    new Game
                    {
                        Id = 6,
                        GameNumber = 6,
                        Round = 4,
                    }
                });

            _games.AddRange(
                new List<Game>
                {
                    new Game
                    {
                        Id = 7,
                        GameNumber = 7,
                        Round = 5,
                    },
                    new Game
                    {
                        Id = 8,
                        GameNumber = 8,
                        Round = 5,
                    }
                });

            // Setup common properties
            _games.ForEach(gr =>
            {
                gr.TournamentId = 1;
                gr.Result = new Result();
            });

            return this;
        }

        public GameTestFixture TestMinimumOddTeamsPlayOffSchedule()
        {
            _games.Clear();

            _games.AddRange(
                new List<Game>
                {
                    new Game
                    {
                        Id = 1,
                        HomeTeamId = 3,
                        AwayTeamId = null,
                        GameNumber = 2,
                        Round = 1,
                        TournamentId = 1,
                        Result = new Result
                        {
                            GameScore = new Score
                            {
                                Home = 3,
                                Away = 0
                            }
                        }
                    },
                    new Game
                    {
                        Id = 2,
                        HomeTeamId = 1,
                        AwayTeamId = 2,
                        GameNumber = 1,
                        Round = 1,
                        TournamentId = 1,
                        Result = new Result()
                    },
                    new Game
                    {
                        Id = 3,
                        Round = 2,
                        GameNumber = 3,
                        TournamentId = 1,
                        Result = new Result()
                    }
                });

            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="GameTestFixture"/>.
        /// </summary>
        /// <returns>Collection of <see cref="Game"/> objects filled with test data.</returns>
        public List<Game> Build()
        {
            return _games;
        }

        /// <summary>
        /// Resets game to default state in playoff tournament
        /// </summary>
        public GameTestFixture ResetPlayoffGame(int gameId)
        {
            var game = _games.First(g => g.Id == gameId);

            game.AwayTeamId = null;
            game.HomeTeamId = null;

            return this;
        }
    }
}
