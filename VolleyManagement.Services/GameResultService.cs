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

        /// <summary>
        /// Edits specified game result.
        /// </summary>
        /// <param name="gameResult">Updated game result.</param>
        public void Edit(GameResult gameResult)
        {
            if (gameResult == null)
            {
                throw new ArgumentNullException("gameResult");
            }

            ValidateGameResult(gameResult);
            _gameResultRepository.Update(gameResult);
            _gameResultRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Deletes game result by specified identifier.
        /// </summary>
        /// <param name="id">Identifier of game result.</param>
        public void Delete(int id)
        {
            _gameResultRepository.Remove(id);
            _gameResultRepository.UnitOfWork.Commit();
        }

        #endregion

        #region Private methods

        private void ValidateGameResult(GameResult gameResult)
        {
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
                    GameResultConstants.MARGIN_SET_POINTS,
                    GameResultConstants.POINTS_DELTA_TO_WIN,
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
                    GameResultConstants.MARGIN_SET_POINTS,
                    GameResultConstants.POINTS_DELTA_TO_WIN,
                    GameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE,
                    GameResultConstants.TECHNICAL_DEFEAT_SET_LOSER_SCORE));
            }
        }

        #endregion
    }
}
