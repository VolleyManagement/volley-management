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

        #region Query Objects

        private readonly IQuery<GameResult, FindByIdCriteria> _getByIdQuery;

        private readonly IQuery<List<GameResult>, GetAllCriteria> _getAllQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultService"/> class.
        /// </summary>
        /// <param name="gameResultRepository">Instance of class that implements <see cref="IGameResultRepository"/>.</param>
        /// <param name="getByIdQuery">Query which gets <see cref="GameResult"/> object by its identifier.</param>
        /// <param name="getAllQuery">Query which gets all <see cref="GameResult"/> objects.</param>
        public GameResultService(
            IGameResultRepository gameResultRepository,
            IQuery<GameResult, FindByIdCriteria> getByIdQuery,
            IQuery<List<GameResult>, GetAllCriteria> getAllQuery)
        {
            _gameResultRepository = gameResultRepository;
            _getByIdQuery = getByIdQuery;
            _getAllQuery = getAllQuery;
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

        /// <summary>
        /// Gets all game results.
        /// </summary>
        /// <returns>List of all game results.</returns>
        public List<GameResult> Get()
        {
            return _getAllQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Gets game result by specified identifier.
        /// </summary>
        /// <param name="id">Identifier of game result.</param>
        /// <returns>Instance of <see cref="GameResult"/> or null if nothing is found.</returns>
        public GameResult Get(int id)
        {
            return _getByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        #endregion

        #region Private methods

        private void ValidateGameResult(GameResult gameResult)
        {
            ValidateTeams(gameResult.HomeTeamId, gameResult.AwayTeamId);
            ValidateSetsScore(gameResult.SetsScore, gameResult.IsTechnicalDefeat);
            ValidateSetScores(gameResult.SetScores, gameResult.IsTechnicalDefeat);
            ValidateSetsScoreMatchesSetScores(gameResult.SetsScore, gameResult.SetScores);
        }

        private void ValidateTeams(int homeTeamId, int awayTeamId)
        {
            if (GameResultValidation.AreTheSameTeams(homeTeamId, awayTeamId))
            {
                throw new ArgumentException(Resources.GameResultSameTeam);
            }
        }

        private void ValidateSetsScore(Score setsScore, bool isTechnicalDefeat)
        {
            if (!GameResultValidation.IsSetsScoreValid(setsScore, isTechnicalDefeat))
            {
                throw new ArgumentException(
                    string.Format(
                    Resources.GameResultSetsScoreInvalid,
                    GameResultConstants.VALID_SETS_SCORES,
                    GameResultConstants.TECHNICAL_DEFEAT_SETS_WINNER_SCORE,
                    GameResultConstants.TECHNICAL_DEFEAT_SETS_LOSER_SCORE));
            }
        }

        private void ValidateSetScores(IList<Score> setScores, bool isTechnicalDefeat)
        {
            bool isPreviousOptionalSetUnplayed = false;

            for (int i = 0; i < setScores.Count; i++)
            {
                if (i < GameResultConstants.SETS_COUNT_TO_WIN)
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
                            GameResultConstants.UNPLAYED_SET_HOME_SCORE,
                            GameResultConstants.UNPLAYED_SET_AWAY_SCORE));
                    }

                    if (isPreviousOptionalSetUnplayed)
                    {
                        if (!GameResultValidation.IsSetUnplayed(setScores[i]))
                        {
                            throw new ArgumentException(Resources.GameResultPreviousOptionalSetUnplayed);
                        }
                    }

                    isPreviousOptionalSetUnplayed = GameResultValidation.IsSetUnplayed(setScores[i]);
                }
            }
        }

        private void ValidateSetsScoreMatchesSetScores(Score setsScore, IList<Score> setScores)
        {
            if (!GameResultValidation.AreSetScoresMatched(setsScore, setScores))
            {
                throw new ArgumentException(Resources.GameResultSetsScoreNoMatchSetScores);
            }
        }

        #endregion
    }
}
