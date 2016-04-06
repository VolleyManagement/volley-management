namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Domain.GamesAggregate;

    /// <summary>
    /// Provides implementation of game result queries.
    /// </summary>
    public class GameResultQueries : IQuery<GameResultDto, FindByIdCriteria>,
                                     IQuery<List<GameResultDto>, TournamentGameResultsCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;
        private readonly DbSet<GameResultEntity> _dalGameResults;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork">Instance of class which implements <see cref="IUnitOfWork"/>.</param>
        public GameResultQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalGameResults = _unitOfWork.Context.GameResults;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds game result by identifier criteria.
        /// </summary>
        /// <param name="criteria">Identifier criteria.</param>
        /// <returns>Domain model of game result.</returns>
        public GameResultDto Execute(FindByIdCriteria criteria)
        {
            return _dalGameResults.Where(gr => gr.Id == criteria.Id).Select(GetGameResultWithTeamNamesMapping()).SingleOrDefault();
        }

        /// <summary>
        /// Gets game results of the tournament by specified criteria.
        /// </summary>
        /// <param name="criteria">Tournament's game results criteria.</param>
        /// <returns>List of domain models of game result.</returns>
        public List<GameResultDto> Execute(TournamentGameResultsCriteria criteria)
        {
            return _dalGameResults.Where(gr => gr.TournamentId == criteria.TournamentId)
                .Select(GetGameResultWithTeamNamesMapping())
                .ToList();
        }

        #endregion

        #region Mapping

        private static Expression<Func<GameResultEntity, GameResultDto>> GetGameResultWithTeamNamesMapping()
        {
            return gr => new GameResultDto
            {
                Id = gr.Id,
                TournamentId = gr.TournamentId,
                HomeTeamId = gr.HomeTeamId,
                AwayTeamId = gr.AwayTeamId,
                HomeTeamName = gr.HomeTeam.Name,
                AwayTeamName = gr.AwayTeam.Name,
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
                AwaySet5Score = gr.AwaySet5Score,
                GameDate = gr.StartTime,
                Round = Convert.ToInt32(gr.RoundNumber)
            };
        }

        #endregion
    }
}
