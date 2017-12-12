namespace VolleyManagement.UnitTests.Services.GameService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GamesAggregate;

    /// <summary>
    /// Represents a builder of <see cref="Game"/> objects for unit tests for <see cref="GameService"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GameBuilder
    {
        #region Fields

        private const string DATE = "2016-04-03 10:00";

        private const int MAX_SETS_COUNT = 5;

        private Game _game;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameBuilder"/> class.
        /// </summary>
        public GameBuilder()
        {
            _game = new Game
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                Result = new Result
                {
                    GameScore = new Score(3, 0, false),
                    SetScores = new List<Score>
                    {
                        new Score(25, 20),
                        new Score(26, 24),
                        new Score(30, 28),
                        new Score(0, 0),
                        new Score(0, 0)
                    }
                },
                GameDate = DateTime.Parse(DATE),
                Round = 1,
                GameNumber = 0
            };
        }

        #endregion

        #region Main setter methods

        /// <summary>
        /// Sets the identifier of the game.
        /// </summary>
        /// <param name="id">Identifier of the game.</param>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithId(int id)
        {
            _game.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the tournament where game belongs.
        /// </summary>
        /// <param name="id">Identifier of the tournament.</param>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithTournamentId(int id)
        {
            _game.TournamentId = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the home team which played the game.
        /// </summary>
        /// <param name="id">Identifier of the home team.</param>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithHomeTeamId(int? id)
        {
            _game.HomeTeamId = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the away team which played the game.
        /// </summary>
        /// <param name="id">Identifier of the away team.</param>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithAwayTeamId(int? id)
        {
            _game.AwayTeamId = id;
            return this;
        }

        /// <summary>
        /// Sets empty null result in game.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithNullResult()
        {
            _game.Result = null;
            return this;
        }

        /// <summary>
        /// Sets empty initialized result in game.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithDefaultResult()
        {
            _game.Result.GameScore = new Score(0, 0, false);
            foreach (var score in _game.Result.SetScores)
            {
                score.Away = 0;
                score.Home = 0;
            }

            return this;
        }

        /// <summary>
        /// Sets the final score of the game.
        /// </summary>
        /// <param name="score">Final score of the game.</param>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithSetsScore(Score score)
        {
            _game.Result.GameScore = score;
            return this;
        }

        /// <summary>
        /// Sets a value of technical defeat to true.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithTechnicalDefeat()
        {
            _game.Result.GameScore.IsTechnicalDefeat = true;
            return this;
        }

        /// <summary>
        /// Sets a value of technical defeat to false.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithNoTechnicalDefeat()
        {
            _game.Result.GameScore.IsTechnicalDefeat = false;
            return this;
        }

        /// <summary>
        /// Sets set score by the specified set number.
        /// </summary>
        /// <param name="setNumber">Set number.</param>
        /// <param name="score">Set score.</param>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithSetScore(byte setNumber, Score score)
        {
            _game.Result.SetScores[setNumber - 1] = score;
            return this;
        }

        /// <summary>
        /// Sets the set scores of the game.
        /// </summary>
        /// <param name="scores">Set scores.</param>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithSetScores(IEnumerable<Score> scores)
        {
            _game.Result.SetScores.Clear();
            _game.Result.SetScores.AddRange(scores);
            return this;
        }

        /// <summary>
        /// Sets the game date to given date
        /// </summary>
        /// <param name="date">Date to set</param>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithStartDate(DateTime date)
        {
            _game.GameDate = date;
            return this;
        }

        public GameBuilder WithNoStartDate()
        {
            _game.GameDate = null;
            return this;
        }

        public GameBuilder WithGameNumber(byte gameNumber)
        {
            _game.GameNumber = gameNumber;
            return this;
        }

        public GameBuilder WithAPenalty()
        {
            _game.Result.Penalty = new Penalty
            {
                IsHomeTeam = true,
                Amount = 2,
                Description = "Penalty reason"
            };
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="GameBuilder"/>.
        /// </summary>
        /// <returns>Instance of <see cref="Game"/>.</returns>
        public Game Build()
        {
            return _game;
        }

        #endregion

        #region Helper setter methods

        /// <summary>
        /// Sets the same home and away teams.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithTheSameTeams()
        {
            _game.HomeTeamId = 1;
            _game.AwayTeamId = 1;
            return this;
        }

        /// <summary>
        /// Sets the final score of the game in a way that it is invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithInvalidSetsScore()
        {
            _game.Result.GameScore = new Score(1, 0);
            return this;
        }

        /// <summary>
        /// Sets the final score of the game in a way that it does not match set scores.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithSetsScoreNoMatchSetScores()
        {
            _game.Result.GameScore = new Score(3, 1);
            _game.Result.SetScores = new List<Score>
            {
                new Score(25, 20),
                new Score(24, 26),
                new Score(28, 30),
                new Score(25, 22),
                new Score(27, 25)
            };

            return this;
        }

        /// <summary>
        /// Sets the required set scores in a way that they are invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithInvalidRequiredSetScores()
        {
            _game.Result.SetScores = new List<Score>
            {
                new Score(10, 0),
                new Score(10, 0),
                new Score(10, 0),
                new Score(0, 0),
                new Score(0, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the optional set scores in a way that they are invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithInvalidOptionalSetScores()
        {
            _game.Result.GameScore = new Score(3, 2);
            _game.Result.SetScores = new List<Score>
            {
                new Score(25, 20),
                new Score(24, 26),
                new Score(28, 30),
                new Score(10, 0),
                new Score(10, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the previous optional set score to 0:0.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithPreviousOptionalSetUnplayed()
        {
            _game.Result.GameScore = new Score(3, 1);
            _game.Result.SetScores = new List<Score>
            {
                new Score(25, 23),
                new Score(20, 25),
                new Score(25, 21),
                new Score(0, 0),
                new Score(25, 23)
            };

            return this;
        }

        /// <summary>
        /// Sets the set scores in a way that they are not listed in the correct order.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithSetScoresUnorderedForHomeTeam()
        {
            _game.Result.GameScore = new Score(3, 1);
            _game.Result.SetScores = new List<Score>
            {
                new Score(25, 20),
                new Score(25, 21),
                new Score(25, 18),
                new Score(23, 25),
                new Score(0, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the round number for game
        /// </summary>
        /// <param name="roundNumber">number of the round</param>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithRound(byte roundNumber)
        {
            _game.Round = roundNumber;
            return this;
        }

        public GameBuilder TestRoundGame()
        {
            _game.TournamentId = 1;
            _game.HomeTeamId = 1;
            _game.AwayTeamId = 2;
            _game.Round = 1;

            return this;
        }

        public GameBuilder TestRoundGameSwithedTeams()
        {
            _game.TournamentId = 1;
            _game.HomeTeamId = 2;
            _game.AwayTeamId = 1;
            _game.Round = 1;

            return this;
        }

        public GameBuilder TestFreeDayGame()
        {
            _game.HomeTeamId = 1;
            _game.AwayTeamId = null;
            _game.Round = 1;

            return this;
        }

        /// <summary>
        /// Sets the set scores in a way that they are not listed in the correct order.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithSetScoresUnorderedForAwayTeam()
        {
            _game.Result.GameScore = new Score(2, 3);
            _game.Result.SetScores = new List<Score>
            {
                new Score(0, 25),
                new Score(0, 25),
                new Score(0, 25),
                new Score(25, 0),
                new Score(15, 0),
            };

            return this;
        }

        /// <summary>
        /// Sets the set scores for home team win with a technical win in a way that they are valid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithTechnicalDefeatValidSetScoresHomeTeamWin()
        {
            _game.Result.GameScore = new Score(3, 0, true);
            _game.Result.SetScores = new List<Score>
            {
                new Score(25, 0),
                new Score(25, 0),
                new Score(25, 0),
                new Score(0, 0),
                new Score(0, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the set scores for away team win with a technical win in a way that they are valid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithTechnicalDefeatValidSetScoresAwayTeamWin()
        {
            _game.Result.GameScore = new Score(0, 3, true);
            _game.Result.SetScores = new List<Score>
            {
                new Score(0, 25),
                new Score(0, 25),
                new Score(0, 25),
                new Score(0, 0),
                new Score(0, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the sets score for home team win with a technical win in a way that they are invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithTechnicalDefeatInvalidSetsScore()
        {
            _game.Result.GameScore = new Score(2, 0, true);
            _game.Result.SetScores = new List<Score>
            {
                new Score(25, 0),
                new Score(25, 0),
                new Score(0, 0),
                new Score(0, 0),
                new Score(0, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the set scores for home team win with a technical win in a way that they are invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithTechnicalDefeatInvalidSetScores()
        {
            _game.Result.GameScore = new Score(3, 0, true);
            _game.Result.SetScores = new List<Score>
            {
                new Score(25, 0),
                new Score(25, 0),
                new Score(24, 0),
                new Score(0, 0),
                new Score(0, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the set scores for home team win with a technical win in a way that they are valid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithTechnicalDefeatValidOptional()
        {
            _game.Result.GameScore = new Score(3, 0, true);
            _game.Result.SetScores = new List<Score>
            {
                new Score(0, 0),
                new Score(0, 0),
                new Score(0, 0),
                new Score(0, 0),
                new Score(0, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the set scores for away team win in a way that they are invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithSetScoresNull()
        {
            _game.Result.GameScore = new Score(0, 3, true);
            _game.Result.SetScores = null;

            return this;
        }

        /// <summary>
        /// Sets the set scores in a way that they are invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithOrdinarySetsScoreInvalid()
        {
            _game.Result.GameScore = new Score(4, 1);
            _game.Result.SetScores = new List<Score>
            {
                new Score(0, 25),
                new Score(0, 25),
                new Score(0, 25),
                new Score(0, 25),
                new Score(15, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the fifth set scores in a way that they are invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithFifthSetScoreAsUsualSetScore()
        {
            _game.Result.GameScore = new Score(3, 2);
            _game.Result.SetScores = new List<Score>
            {
                new Score(0, 25),
                new Score(0, 25),
                new Score(25, 0),
                new Score(25, 0),
                new Score(25, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the fifth set scores in a way that they are valid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithFifthSetScoreMoreThanMaxWithValidDifference()
        {
            _game.Result.GameScore = new Score(3, 2);
            _game.Result.SetScores = new List<Score>
            {
                new Score(0, 25),
                new Score(0, 25),
                new Score(25, 0),
                new Score(25, 0),
                new Score(25, 23)
            };

            return this;
        }

        /// <summary>
        /// Sets the fifth set scores in a way that they are invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithFifthSetScoreMoreThanMaxWithInvalidDifference()
        {
            _game.Result.GameScore = new Score(3, 2);
            _game.Result.SetScores = new List<Score>
            {
                new Score(0, 25),
                new Score(0, 25),
                new Score(25, 0),
                new Score(25, 0),
                new Score(25, 13)
            };

            return this;
        }

        /// <summary>
        /// Sets the fifth set scores in a way that they are invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithFifthSetScoreLessThanMax()
        {
            _game.Result.GameScore = new Score(3, 2);
            _game.Result.SetScores = new List<Score>
            {
                new Score(0, 25),
                new Score(0, 25),
                new Score(25, 0),
                new Score(25, 0),
                new Score(13, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the set scores in a way that they are valid.
        /// </summary>
        /// <returns>Instance of <see cref="GameBuilder"/>.</returns>
        public GameBuilder WithFifthSetScoreValid()
        {
            _game.Result.GameScore = new Score(3, 2);
            _game.Result.SetScores = new List<Score>
            {
                new Score(0, 25),
                new Score(0, 25),
                new Score(25, 0),
                new Score(25, 0),
                new Score(15, 13)
            };

            return this;
        }
        #endregion
    }
}
