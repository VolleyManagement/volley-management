namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using VolleyManagement.Contracts;
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
            ValidateSetsScoreMatchesSetScores(gameResult.SetsScore, gameResult.SetScores);
            ValidateSetsScore(gameResult.SetsScore, gameResult.IsTechnicalDefeat);
            ValidateSetScores(gameResult.SetScores, gameResult.IsTechnicalDefeat);
        }

        private void ValidateTeams(int homeTeamId, int awayTeamId)
        {
            if (GameResultValidation.AreTheSameTeams(homeTeamId, awayTeamId))
            {
                throw new ArgumentException(Resources.GameResultSameTeam);
            }
        }

        private void ValidateSetsScoreMatchesSetScores(Score setsScore, IList<Score> setScores)
        {
            if (!GameResultValidation.AreSetScoresMatched(setsScore, setScores))
            {
                throw new ArgumentException(Resources.GameResultSetsScoreNoMatchSetScores);
            }
        }

        private void ValidateSetsScore(Score setsScore, bool isTechnicalDefeat)
        {
            if (!GameResultValidation.IsSetsScoreValid(setsScore, isTechnicalDefeat))
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

        private void ValidateSetScores(IList<Score> setScores, bool isTechnicalDefeat)
        {
            for (int i = 0; i < setScores.Count; i++)
            {
                if (i <= GameResultConstants.MIN_SETS_COUNT)
                {
                    if (!GameResultValidation.IsRequiredSetScoreValid(setScores[i], isTechnicalDefeat))
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
                else
                {
                    if (!GameResultValidation.IsOptionalSetScoreValid(setScores[i], isTechnicalDefeat))
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
            }
        }

        #endregion
    }
}
