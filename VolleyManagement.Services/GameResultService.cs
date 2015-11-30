namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using VolleyManagement.Contracts;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.GameResultsAggregate;
    using VolleyManagement.Domain.Properties;
    using GameResultConstants = VolleyManagement.Domain.Constants.GameResult;

    /// <summary>
    /// Represents game result service.
    /// </summary>
    public class GameResultService : IGameResultService
    {
        #region Fields

        private readonly IGameResultRepository _gameResultRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultService"/> class.
        /// </summary>
        /// <param name="gameResultRepository">Instance of class that implements <see cref="IGameResultRepository"/>.</param>
        public GameResultService(IGameResultRepository gameResultRepository)
        {
            _gameResultRepository = gameResultRepository;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates new game result.
        /// </summary>
        /// <param name="gameResult">Game result to create.</param>
        public void Create(GameResult gameResult)
        {
            if (gameResult == null)
            {
                throw new ArgumentNullException("gameResult");
            }

            ValidateGameResult(gameResult);
            _gameResultRepository.Add(gameResult);
        }

        #endregion

        #region Private methods

        private void ValidateGameResult(GameResult gameResult)
        {
            ValidateTeams(gameResult.HomeTeamId, gameResult.AwayTeamId);

            var homeSetScores = new[]
            {
                gameResult.HomeSet1Score,
                gameResult.HomeSet2Score,
                gameResult.HomeSet3Score,
                gameResult.HomeSet4Score,
                gameResult.HomeSet5Score
            };
            var awaySetScores = new[]
            {
                gameResult.AwaySet1Score,
                gameResult.AwaySet2Score,
                gameResult.AwaySet3Score,
                gameResult.AwaySet4Score,
                gameResult.AwaySet5Score
            };

            ValidateSetsScoreMatchesSetScores(gameResult.HomeSetsScore, gameResult.AwaySetsScore, homeSetScores, awaySetScores);
            ValidateSetsScore(gameResult.HomeSetsScore, gameResult.AwaySetsScore, gameResult.IsTechnicalDefeat);
            ValidateRequiredSetScore(gameResult.HomeSet1Score, gameResult.AwaySet1Score, gameResult.IsTechnicalDefeat);
            ValidateRequiredSetScore(gameResult.HomeSet2Score, gameResult.AwaySet2Score, gameResult.IsTechnicalDefeat);
            ValidateRequiredSetScore(gameResult.HomeSet3Score, gameResult.AwaySet3Score, gameResult.IsTechnicalDefeat);
            ValidateOptionalSetScore(gameResult.HomeSet4Score, gameResult.AwaySet4Score, gameResult.IsTechnicalDefeat);
            ValidateOptionalSetScore(gameResult.HomeSet5Score, gameResult.AwaySet5Score, gameResult.IsTechnicalDefeat);
        }

        private void ValidateTeams(int homeTeamId, int awayTeamId)
        {
            if (GameResultValidation.AreTheSameTeams(homeTeamId, awayTeamId))
            {
                throw new ArgumentException(Resources.GameResultSameTeam);
            }
        }

        private void ValidateSetsScoreMatchesSetScores(byte homeSetsScore, byte awaySetsScore, byte[] homeSetScores, byte[] awaySetScores)
        {
            if (!GameResultValidation.AreSetScoresMatched(homeSetsScore, awaySetsScore, homeSetScores, awaySetScores))
            {
                throw new ArgumentException(Resources.GameResultSetsScoreNoMatchSetScores);
            }
        }

        private void ValidateSetsScore(byte homeSetsScore, byte awaySetsScore, bool isTechnicalDefeat)
        {
            if (!GameResultValidation.IsSetsScoreValid(homeSetsScore, awaySetsScore, isTechnicalDefeat))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.GameResultSetsScoreInvalid,
                    GameResultConstants.MIN_SETS_COUNT,
                    GameResultConstants.MAX_SETS_COUNT,
                    GameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE,
                    GameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE));
            }
        }

        private void ValidateRequiredSetScore(byte homeSetsScore, byte awaySetsScore, bool isTechnicalDefeat)
        {
            if (!GameResultValidation.IsRequiredSetScoreValid(homeSetsScore, awaySetsScore, isTechnicalDefeat))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.GameResultRequiredSetScores,
                    GameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                    GameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                    GameResultConstants.TECHNICAL_DEFEAT_SET_WINNER_SCORE,
                    GameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE));
            }
        }

        private void ValidateOptionalSetScore(byte homeSetsScore, byte awaySetsScore, bool isTechnicalDefeat)
        {
            if (!GameResultValidation.IsOptionalSetScoreValid(homeSetsScore, awaySetsScore, isTechnicalDefeat))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.GameResultOptionalSetScores,
                    GameResultConstants.SET_POINTS_MIN_VALUE_TO_WIN,
                    GameResultConstants.SET_POINTS_MIN_DELTA_TO_WIN,
                    GameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE,
                    GameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE));
            }
        }

        #endregion
    }
}
