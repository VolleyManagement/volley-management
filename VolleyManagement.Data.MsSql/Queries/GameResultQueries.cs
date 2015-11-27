namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.GameResultsAggregate;

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
                HomeSetsScore = gr.HomeSetsScore,
                AwaySetsScore = gr.AwaySetsScore,
                IsTechnicalDefeat = gr.IsTechnicalDefeat,
                HomeSet1Score = gr.HomeSet1Score,
                AwaySet1Score = gr.AwaySet1Score,
                HomeSet2Score = gr.HomeSet2Score,
                AwaySet2Score = gr.AwaySet2Score,
                HomeSet3Score = gr.HomeSet3Score,
                AwaySet3Score = gr.AwaySet3Score,
                HomeSet4Score = gr.HomeSet4Score,
                AwaySet4Score = gr.AwaySet4Score,
                HomeSet5Score = gr.HomeSet5Score,
                AwaySet5Score = gr.AwaySet5Score
            };
        }

        #endregion
    }
}
