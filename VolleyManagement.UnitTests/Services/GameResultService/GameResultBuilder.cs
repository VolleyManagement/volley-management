namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Represents builder for unit tests for <see cref="GameResult"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GameResultBuilder
    {
        #region Fields

        private GameResult _gameResult;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultBuilder"/> class.
        /// </summary>
        public GameResultBuilder()
        {
            _gameResult = new GameResult
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1,
                AwayTeamId = 2,
                SetsScore = new Score(3, 2),
                IsTechnicalDefeat = false,
                SetScores = new List<Score>
                {
                    new Score(25, 20),
                    new Score(24, 26),
                    new Score(28, 30),
                    new Score(25, 22),
                    new Score(27, 25)
                }
            };
        }

        #endregion

        #region Main setter methods

        /// <summary>
        /// Sets the identifier of the game result.
        /// </summary>
        /// <param name="id">Identifier of the game result.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithId(int id)
        {
            _gameResult.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the tournament where game result belongs.
        /// </summary>
        /// <param name="id">Identifier of the tournament.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithTournamentId(int id)
        {
            _gameResult.TournamentId = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the home team which played the game.
        /// </summary>
        /// <param name="id">Identifier of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithHomeTeamId(int id)
        {
            _gameResult.HomeTeamId = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the away team which played the game.
        /// </summary>
        /// <param name="id">Identifier of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithAwayTeamId(int id)
        {
            _gameResult.AwayTeamId = id;
            return this;
        }

        /// <summary>
        /// Sets the final score of the game.
        /// </summary>
        /// <param name="score">Final score of the game.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithSetsScore(Score score)
        {
            _gameResult.SetsScore = score;
            return this;
        }

        /// <summary>
        /// Sets a value of technical defeat to true.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithTechnicalDefeat()
        {
            _gameResult.IsTechnicalDefeat = true;
            return this;
        }

        /// <summary>
        /// Sets a value of technical defeat to false.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithNoTechnicalDefeat()
        {
            _gameResult.IsTechnicalDefeat = false;
            return this;
        }

        /// <summary>
        /// Sets set score by the specified set number.
        /// </summary>
        /// <param name="setNumber">Set number.</param>
        /// <param name="score">Set score.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithSetScore(byte setNumber, Score score)
        {
            _gameResult.SetScores[setNumber - 1] = score;
            return this;
        }

        /// <summary>
        /// Sets the set scores of the game.
        /// </summary>
        /// <param name="scores">Set scores.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithSetScores(IEnumerable<Score> scores)
        {
            _gameResult.SetScores.Clear();
            _gameResult.SetScores.AddRange(scores);
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="GameResultBuilder"/>.
        /// </summary>
        /// <returns>Instance of <see cref="GameResult"/>.</returns>
        public GameResult Build()
        {
            return _gameResult;
        }

        #endregion

        #region Helper setter methods

        /// <summary>
        /// Sets the same home and away teams.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithTheSameTeams()
        {
            _gameResult.HomeTeamId = 1;
            _gameResult.AwayTeamId = 1;
            return this;
        }

        /// <summary>
        /// Sets the final score of the game in a way that it does not match set scores.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithSetsScoreNoMatchSetScores()
        {
            _gameResult.SetsScore = new Score(4, 1);
            _gameResult.SetScores = new List<Score>
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
        /// Sets the final score of the game in a way that it is invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithInvalidSetsScore()
        {
            _gameResult.SetsScore = new Score(1, 0);
            _gameResult.SetScores = new List<Score>
            {
                new Score(25, 20),
                new Score(),
                new Score(),
                new Score(),
                new Score()
            };

            return this;
        }

        /// <summary>
        /// Sets the required set scores in a way that they are invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithInvalidRequiredSetScores()
        {
            _gameResult.SetsScore = new Score(1, 2);
            _gameResult.SetScores = new List<Score>
            {
                new Score(10, 0),
                new Score(0, 10),
                new Score(0, 10),
                new Score(0, 0),
                new Score(0, 0)
            };

            return this;
        }

        /// <summary>
        /// Sets the optional set scores in a way that they are invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithInvalidOptionalSetScores()
        {
            _gameResult.SetsScore = new Score(3, 2);
            _gameResult.SetScores = new List<Score>
            {
                new Score(25, 20),
                new Score(24, 26),
                new Score(28, 30),
                new Score(10, 0),
                new Score(10, 0)
            };

            return this;
        }

        #endregion
    }
}
