namespace VolleyManagement.UnitTests.Services.GameService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GamesAggregate;

    /// <summary>
    /// Represents a builder of <see cref="GameResultDto"/> objects for unit tests for <see cref="GameService"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GameResultDtoBuilder
    {
        #region Fields

        private const string DATE = "2016-04-03 10:00";

        private GameResultDto _gameResult;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultDtoBuilder"/> class.
        /// </summary>
        public GameResultDtoBuilder()
        {
            _gameResult = new GameResultDto
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                HomeTeamName = "TeamNameA",
                AwayTeamName = "AwayTeamName",
                Result = new Result
                {
                    GameScore = new Score
                    {
                        Home = 3,
                        Away = 0,
                        IsTechnicalDefeat = false,
                    },
                    SetScores = new System.Collections.Generic.List<Score>
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
                GameDate = DateTime.Parse(DATE),
                Round = 1,
                GameNumber = 0
            };
        }

        #endregion

        #region Main setter methods

        /// <summary>
        /// Sets the identifier of the game result.
        /// </summary>
        /// <param name="id">Identifier of the game result.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithId(int id)
        {
            _gameResult.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the tournament where game result belongs.
        /// </summary>
        /// <param name="id">Identifier of the tournament.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithTournamentId(int id)
        {
            _gameResult.TournamentId = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the home team which played the game.
        /// </summary>
        /// <param name="id">Identifier of the home team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithHomeTeamId(int id)
        {
            _gameResult.HomeTeamId = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the away team which played the game.
        /// </summary>
        /// <param name="id">Identifier of the away team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithAwayTeamId(int id)
        {
            _gameResult.AwayTeamId = id;
            return this;
        }

        /// <summary>
        /// Sets the name of the home team which played the game.
        /// </summary>
        /// <param name="name">Name of the home team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithHomeTeamName(string name)
        {
            _gameResult.HomeTeamName = name;
            return this;
        }

        /// <summary>
        /// Sets the name of the away team which played the game.
        /// </summary>
        /// <param name="name">Name of the away team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithAwayTeamName(string name)
        {
            _gameResult.AwayTeamName = name;
            return this;
        }

        /// <summary>
        /// Sets the final score of the game (sets score) for the home team.
        /// </summary>
        /// <param name="score">Final score of the game for the home team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithHomeSetsScore(byte score)
        {
            _gameResult.Result.GameScore.Home = score;
            return this;
        }

        /// <summary>
        /// Sets the final score of the game (sets score) for the away team.
        /// </summary>
        /// <param name="score">Final score of the game for the away team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithAwaySetsScore(byte score)
        {
            _gameResult.Result.GameScore.Away = score;
            return this;
        }

        /// <summary>
        /// Sets a value of technical defeat to true.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithTechnicalDefeat()
        {
            _gameResult.Result.GameScore.IsTechnicalDefeat = true;
            return this;
        }

        /// <summary>
        /// Sets a value of technical defeat to false.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithNoTechnicalDefeat()
        {
            _gameResult.Result.GameScore.IsTechnicalDefeat = false;
            return this;
        }

        /// <summary>
        /// Sets the score of the first set for the home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithHomeSet1Score(byte score)
        {
            _gameResult.Result.SetScores[0].Home = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the first set for the away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithAwaySet1Score(byte score)
        {
            _gameResult.Result.SetScores[0].Away = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the second set for the home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithHomeSet2Score(byte score)
        {
            _gameResult.Result.SetScores[1].Home = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the second set for the away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithAwaySet2Score(byte score)
        {
            _gameResult.Result.SetScores[1].Away = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the third set for the home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithHomeSet3Score(byte score)
        {
            _gameResult.Result.SetScores[2].Home = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the third set for the away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithAwaySet3Score(byte score)
        {
            _gameResult.Result.SetScores[2].Away = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fourth set for the home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithHomeSet4Score(byte score)
        {
            _gameResult.Result.SetScores[3].Home = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fourth set for the away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithAwaySet4Score(byte score)
        {
            _gameResult.Result.SetScores[3].Away = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fifth set for the home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithHomeSet5Score(byte score)
        {
            _gameResult.Result.SetScores[4].Home = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fifth set for the away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithAwaySet5Score(byte score)
        {
            _gameResult.Result.SetScores[4].Away = score;
            return this;
        }

        /// <summary>
        /// Sets the date of game.
        /// </summary>
        /// <param name="date">Game's date.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithDate(DateTime date)
        {
            _gameResult.GameDate = date;
            return this;
        }

        /// <summary>
        /// Sets the round of game.
        /// </summary>
        /// <param name="round">Game's round.</param>
        /// <returns>Instance of <see cref="GameResultDtoBuilder"/>.</returns>
        public GameResultDtoBuilder WithRound(byte round)
        {
            _gameResult.Round = round;
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="GameResultDtoBuilder"/>.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultDto"/>.</returns>
        public GameResultDto Build()
        {
            return _gameResult;
        }

        #endregion
    }
}
