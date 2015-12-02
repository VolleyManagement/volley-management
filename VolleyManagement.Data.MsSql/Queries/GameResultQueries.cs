namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Provides Query Object implementation for GameResult entity
    /// </summary>
    public class GameResultQueries : IQuery<GameResult, FindByIdCriteria>,
                                     IQuery<List<GameResult>, GetAllCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public GameResultQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds game result by identifier criteria.
        /// </summary>
        /// <param name="criteria">Identifier criteria.</param>
        /// <returns>Domain model of game result.</returns>
        public GameResult Execute(FindByIdCriteria criteria)
        {
            return _unitOfWork.Context.GameResults
                .Where(gr => gr.Id == criteria.Id)
                .Select(GetGameResultMapping())
                .SingleOrDefault();
        }

        /// <summary>
        /// Get all game results.
        /// </summary>
        /// <param name="criteria">Get all results criteria.</param>
        /// <returns>List of domain models of game result.</returns>
        public List<GameResult> Execute(GetAllCriteria criteria)
        {
            return _unitOfWork.Context.GameResults.Select(GetGameResultMapping()).ToList();
        }

        #endregion

        #region Mapping

        private static Expression<Func<GameResultEntity, GameResult>> GetGameResultMapping()
        {
            return gr => new GameResult
            {
                Id = gr.Id,
                TournamentId = gr.TournamentId,
                HomeTeamId = gr.HomeTeamId,
                AwayTeamId = gr.AwayTeamId,
                SetsScore = new Score { Home = gr.HomeSetsScore, Away = gr.AwaySetsScore },
                IsTechnicalDefeat = gr.IsTechnicalDefeat,
                SetScores = new List<Score>
                {
                    new Score { Home = gr.HomeSet1Score, Away = gr.AwaySet1Score },
                    new Score { Home = gr.HomeSet2Score, Away = gr.AwaySet2Score },
                    new Score { Home = gr.HomeSet3Score, Away = gr.AwaySet3Score },
                    new Score { Home = gr.HomeSet4Score, Away = gr.AwaySet4Score },
                    new Score { Home = gr.HomeSet5Score, Away = gr.AwaySet5Score }
                }
            };
        }

        #endregion
    }
}
