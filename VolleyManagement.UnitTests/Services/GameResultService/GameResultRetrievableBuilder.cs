namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Represents builder for unit tests for <see cref="GameResultService"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GameResultRetrievableBuilder
    {
        #region Fields

        private GameResultRetrievable _gameResult;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultRetrievableBuilder"/> class.
        /// </summary>
        public GameResultRetrievableBuilder()
        {
            _gameResult = new GameResultRetrievable
            {
                Id = 1,
                TournamentId = 1,
                HomeTeamId = 1, AwayTeamId = 2,
                HomeTeamName = "TeamNameA", AwayTeamName = "AwayTeamName",
                HomeSetsScore = 3, AwaySetsScore = 0,
                IsTechnicalDefeat = false,
                HomeSet1Score = 25, AwaySet1Score = 20,
                HomeSet2Score = 26, AwaySet2Score = 24,
                HomeSet3Score = 30, AwaySet3Score = 28,
                HomeSet4Score = 0, AwaySet4Score = 0,
                HomeSet5Score = 0, AwaySet5Score = 0,
            };
        }

        #endregion

        #region Main setter methods

        /// <summary>
        /// Sets the identifier of the game result.
        /// </summary>
        /// <param name="id">Identifier of the game result.</param>
        /// <returns>Instance of <see cref="GameResultRetrievableBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithId(int id)
        {
            _gameResult.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the tournament where game result belongs.
        /// </summary>
        /// <param name="id">Identifier of the tournament.</param>
        /// <returns>Instance of <see cref="GameResultRetrievableBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithTournamentId(int id)
        {
            _gameResult.TournamentId = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the home team which played the game.
        /// </summary>
        /// <param name="id">Identifier of the home team.</param>
        /// <returns>Instance of <see cref="GameResultRetrievableBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithHomeTeamId(int id)
        {
            _gameResult.HomeTeamId = id;
            return this;
        }

        /// <summary>
        /// Sets the identifier of the away team which played the game.
        /// </summary>
        /// <param name="id">Identifier of the away team.</param>
        /// <returns>Instance of <see cref="GameResultRetrievableBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithAwayTeamId(int id)
        {
            _gameResult.AwayTeamId = id;
            return this;
        }

        /// <summary>
        /// Sets the name of the home team which played the game.
        /// </summary>
        /// <param name="name">Name of the home team.</param>
        /// <returns>Instance of <see cref="GameResultRetrievableBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithHomeTeamName(string name)
        {
            _gameResult.HomeTeamName = name;
            return this;
        }

        /// <summary>
        /// Sets the name of the away team which played the game.
        /// </summary>
        /// <param name="name">Name of the away team.</param>
        /// <returns>Instance of <see cref="GameResultRetrievableBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithAwayTeamName(string name)
        {
            _gameResult.AwayTeamName = name;
            return this;
        }

        /// <summary>
        /// Sets the final score of the game (sets score) for the home team.
        /// </summary>
        /// <param name="score">Final score of the game for the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithHomeSetsScore(byte score)
        {
            _gameResult.HomeSetsScore = score;
            return this;
        }

        /// <summary>
        /// Sets the final score of the game (sets score) for the away team.
        /// </summary>
        /// <param name="score">Final score of the game for the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithAwaySetsScore(byte score)
        {
            _gameResult.AwaySetsScore = score;
            return this;
        }

        /// <summary>
        /// Sets a value of technical defeat to true.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultRetrievableBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithTechnicalDefeat()
        {
            _gameResult.IsTechnicalDefeat = true;
            return this;
        }

        /// <summary>
        /// Sets a value of technical defeat to false.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultRetrievableBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithNoTechnicalDefeat()
        {
            _gameResult.IsTechnicalDefeat = false;
            return this;
        }

        /// <summary>
        /// Sets the score of the first set for the home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithHomeSet1Score(byte score)
        {
            _gameResult.HomeSet1Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the first set for the away team.
        /// </summary>
        /// <param name="score">Set score of the the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithAwaySet1Score(byte score)
        {
            _gameResult.AwaySet1Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the second set for the home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithHomeSet2Score(byte score)
        {
            _gameResult.HomeSet2Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the second set for the away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithAwaySet2Score(byte score)
        {
            _gameResult.AwaySet2Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the third set for the home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithHomeSet3Score(byte score)
        {
            _gameResult.HomeSet3Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the third set for the away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithAwaySet3Score(byte score)
        {
            _gameResult.AwaySet3Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fourth set for the home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithHomeSet4Score(byte score)
        {
            _gameResult.HomeSet4Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fourth set for the away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithAwaySet4Score(byte score)
        {
            _gameResult.AwaySet4Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fifth set for the home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithHomeSet5Score(byte score)
        {
            _gameResult.HomeSet5Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fifth set for the away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultRetrievableBuilder WithAwaySet5Score(byte score)
        {
            _gameResult.AwaySet5Score = score;
            return this;
        }

        /// <summary>
        /// Builds instance of <see cref="GameResultRetrievableBuilder"/>.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultRetrievable"/>.</returns>
        public GameResultRetrievable Build()
        {
            return _gameResult;
        }

        #endregion
    }
}
