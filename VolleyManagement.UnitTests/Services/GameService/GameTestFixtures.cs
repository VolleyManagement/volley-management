namespace VolleyManagement.UnitTests.Services.GameService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GamesAggregate;

    [ExcludeFromCodeCoverage]
    internal class GameTestFixtures
    {
        private const string DATE_A = "2016-04-03 10:00";

        private const string DATE_B = "2016-04-07 10:00";

        private const string DATE_C = "2016-04-10 10:00";

        private const string DATE_D = "2016-04-13 10:00";

        private const string DATE_E = "2016-04-16 10:00";

        private const string DATE_F = "2016-04-19 10:00";

        private List<Game> _gameResults = new List<Game>();

        public GameTestFixtures Add(Game game)
        {
            this._gameResults.Add(game);
            return this;
        }

        public GameTestFixtures TestEmptyGamePlayoffSchedule()
        {
            this._gameResults.Clear();
            _gameResults.AddRange(
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

        public GameTestFixtures TestMinimumEvenTeamsPlayOffSchedule()
        {
            this._gameResults.Clear();

            _gameResults.AddRange(
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

        public GameTestFixtures TestMinimumOddTeamsPlayOffSchedule()
        {
            this._gameResults.Clear();

            _gameResults.AddRange(
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
                            SetsScore = new Score
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

        public List<Game> Build()
        {
            return this._gameResults;
        }
    }
}
