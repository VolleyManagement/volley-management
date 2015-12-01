namespace VolleyManagement.UnitTests.Services.GameResultService
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain;
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
                HomeSetsScore = 3,
                AwaySetsScore = 2,
                IsTechnicalDefeat = false,
                HomeSet1Score = 25,
                AwaySet1Score = 20,
                HomeSet2Score = 24,
                AwaySet2Score = 26,
                HomeSet3Score = 28,
                AwaySet3Score = 30,
                HomeSet4Score = 25,
                AwaySet4Score = 22,
                HomeSet5Score = 27,
                AwaySet5Score = 25
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
        /// Sets the final score of the game for home team.
        /// </summary>
        /// <param name="score">Final score of the game for home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithHomeSetsScore(byte score)
        {
            _gameResult.HomeSetsScore = score;
            return this;
        }

        /// <summary>
        /// Sets the final score of the game for away team.
        /// </summary>
        /// <param name="score">Final score of the game for away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithAwaySetsScore(byte score)
        {
            _gameResult.AwaySetsScore = score;
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
        /// Sets the score of the first set for home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithHomeSet1Score(byte score)
        {
            _gameResult.HomeSet1Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the second set for home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithHomeSet2Score(byte score)
        {
            _gameResult.HomeSet2Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the third set for home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithHomeSet3Score(byte score)
        {
            _gameResult.HomeSet3Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fourth set for home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithHomeSet4Score(byte score)
        {
            _gameResult.HomeSet4Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fifth set for home team.
        /// </summary>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithHomeSet5Score(byte score)
        {
            _gameResult.HomeSet5Score = score;
            return this;
        }

        /// <summary>
        /// Sets set score of the home team by the specified set number.
        /// </summary>
        /// <param name="setNumber">Set number.</param>
        /// <param name="score">Set score of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithHomeSetScore(byte setNumber, byte score)
        {
            switch (setNumber)
            {
                case 1:
                    _gameResult.HomeSet1Score = score;
                    break;
                case 2:
                    _gameResult.HomeSet2Score = score;
                    break;
                case 3:
                    _gameResult.HomeSet3Score = score;
                    break;
                case 4:
                    _gameResult.HomeSet4Score = score;
                    break;
                case 5:
                    _gameResult.HomeSet5Score = score;
                    break;
                default:
                    break;
            }

            return this;
        }

        /// <summary>
        /// Sets the set scores of the home team.
        /// </summary>
        /// <param name="homeScores">Set scores of the home team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithHomeSetScores(byte[] homeScores)
        {
            SetHomeSetScores(homeScores);
            return this;
        }

        /// <summary>
        /// Sets the score of the first set for away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithAwaySet1Score(byte score)
        {
            _gameResult.AwaySet1Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the second set for away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithAwaySet2Score(byte score)
        {
            _gameResult.AwaySet2Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the third set for away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithAwaySet3Score(byte score)
        {
            _gameResult.AwaySet3Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fourth set for away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithAwaySet4Score(byte score)
        {
            _gameResult.AwaySet4Score = score;
            return this;
        }

        /// <summary>
        /// Sets the score of the fifth set for away team.
        /// </summary>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithAwaySet5Score(byte score)
        {
            _gameResult.AwaySet5Score = score;
            return this;
        }

        /// <summary>
        /// Sets set score of the away team by the specified set number.
        /// </summary>
        /// <param name="setNumber">Set number.</param>
        /// <param name="score">Set score of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithAwaySetScore(byte setNumber, byte score)
        {
            switch (setNumber)
            {
                case 1:
                    _gameResult.AwaySet1Score = score;
                    break;
                case 2:
                    _gameResult.AwaySet2Score = score;
                    break;
                case 3:
                    _gameResult.AwaySet3Score = score;
                    break;
                case 4:
                    _gameResult.AwaySet4Score = score;
                    break;
                case 5:
                    _gameResult.AwaySet5Score = score;
                    break;
                default:
                    break;
            }

            return this;
        }

        /// <summary>
        /// Sets the set scores of the away team.
        /// </summary>
        /// <param name="awayScores">Set scores of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithAwaySetScores(byte[] awayScores)
        {
            SetAwaySetScores(awayScores);
            return this;
        }

        /// <summary>
        /// Sets the set scores of home and away teams.
        /// </summary>
        /// <param name="homeScores">Set scores of the home team.</param>
        /// <param name="awayScores">Set scores of the away team.</param>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithSetScores(byte[] homeScores, byte[] awayScores)
        {
            SetHomeSetScores(homeScores);
            SetAwaySetScores(awayScores);
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
            _gameResult.HomeSetsScore = 4;
            _gameResult.AwaySetsScore = 1;
            return this;
        }

        /// <summary>
        /// Sets the final score of the game in a way that it is invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithInvalidSetsScore()
        {
            _gameResult.HomeSetsScore = 1;
            _gameResult.AwaySetsScore = 0;
            _gameResult.HomeSet1Score = 25;
            _gameResult.AwaySet1Score = 20;
            _gameResult.HomeSet2Score = 0;
            _gameResult.AwaySet2Score = 0;
            _gameResult.HomeSet3Score = 0;
            _gameResult.AwaySet3Score = 0;
            _gameResult.HomeSet4Score = 0;
            _gameResult.AwaySet4Score = 0;
            _gameResult.HomeSet5Score = 0;
            _gameResult.AwaySet5Score = 0;
            return this;
        }

        /// <summary>
        /// Sets the score of the first set in a way that it is invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithInvalidSet1Score()
        {
            SetInvalidSetScore();
            return this;
        }

        /// <summary>
        /// Sets the score of the second set in a way that it is invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithInvalidSet2Score()
        {
            SetInvalidSetScore();
            return this;
        }

        /// <summary>
        /// Sets the score of the third set in a way that it is invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithInvalidSet3Score()
        {
            SetInvalidSetScore();
            return this;
        }

        /// <summary>
        /// Sets the score of the fourth set in a way that it is invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithInvalidSet4Score()
        {
            SetInvalidSetScore();
            return this;
        }

        /// <summary>
        /// Sets the score of the fifth set in a way that it is invalid.
        /// </summary>
        /// <returns>Instance of <see cref="GameResultBuilder"/>.</returns>
        public GameResultBuilder WithInvalidSet5Score()
        {
            SetInvalidSetScore();
            return this;
        }

        #endregion

        #region Private methods

        private void SetHomeSetScores(byte[] homeScores)
        {
            if (homeScores != null && homeScores.Length >= Constants.GameResult.MIN_SETS_COUNT)
            {
                _gameResult.HomeSet1Score = homeScores[0];
                _gameResult.HomeSet2Score = homeScores[1];
                _gameResult.HomeSet3Score = homeScores[2];

                switch (homeScores.Length)
                {
                    case Constants.GameResult.MIN_SETS_COUNT + 1:
                        _gameResult.HomeSet4Score = homeScores[3];
                        break;
                    default:
                        _gameResult.HomeSet4Score = homeScores[3];
                        _gameResult.HomeSet5Score = homeScores[4];
                        break;
                }
            }
        }

        private void SetAwaySetScores(byte[] awayScores)
        {
            if (awayScores != null && awayScores.Length >= Constants.GameResult.MIN_SETS_COUNT)
            {
                _gameResult.AwaySet1Score = awayScores[0];
                _gameResult.AwaySet2Score = awayScores[1];
                _gameResult.AwaySet3Score = awayScores[2];

                switch (awayScores.Length)
                {
                    case Constants.GameResult.MIN_SETS_COUNT + 1:
                        _gameResult.AwaySet4Score = awayScores[3];
                        break;
                    default:
                        _gameResult.AwaySet4Score = awayScores[3];
                        _gameResult.AwaySet5Score = awayScores[4];
                        break;
                }
            }
        }

        private void SetInvalidSetScore()
        {
            _gameResult.HomeSet1Score = 10;
            _gameResult.AwaySet1Score = 0;
        }

        #endregion
    }
}
