namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.Tournaments;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Provides Object Query implementation for Tournaments
    /// </summary>
    public class TournamentQueries : IQuery<Tournament, UniqueTournamentCriteria>,
                                     IQuery<List<Tournament>, GetAllCriteria>,
                                     IQuery<Tournament, FindByIdCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork"> The unit of work. </param>
        public TournamentQueries(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (VolleyUnitOfWork)unitOfWork;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Tournament"/>. </returns>
        public Tournament Execute(UniqueTournamentCriteria criteria)
        {
            var query = _unitOfWork.Context.Tournaments.Where(t => t.Name == criteria.Name);
            if (criteria.EntityId.HasValue)
            {
                var id = criteria.EntityId.GetValueOrDefault();
                query = query.Where(t => t.Id != id);
            }

            // ToDo: Use Automapper to substitute Select clause
            return query.Select(GetTournamentMapping()).FirstOrDefault();
        }

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Tournament"/>. </returns>
        public List<Tournament> Execute(GetAllCriteria criteria)
        {
            return _unitOfWork.Context.Tournaments.Select(GetTournamentMapping()).ToList();
        }

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Tournament"/>. </returns>
        public Tournament Execute(FindByIdCriteria criteria)
        {
            return _unitOfWork.Context.Tournaments
                                      .Where(t => t.Id == criteria.Id)
                                      .Select(GetTournamentMapping())
                                      .SingleOrDefault();
        }

        #endregion

        #region Mapping

        private static Expression<Func<TournamentEntity, Tournament>> GetTournamentMapping()
        {
            return
                t =>
                new Tournament
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    RegulationsLink = t.RegulationsLink,
                    Scheme = (TournamentSchemeEnum)t.Scheme,
                    Season = (short)(ValidationConstants.Tournament.SCHEMA_STORAGE_OFFSET + t.Season),
                    GamesStart = t.GamesStart,
                    GamesEnd = t.GamesEnd,
                    ApplyingPeriodStart = t.ApplyingPeriodStart,
                    ApplyingPeriodEnd = t.ApplyingPeriodEnd,
                    TransferEnd = t.TransferEnd,
                    TransferStart = t.TransferStart,
                    Divisions = t.Divisions
                                    .AsQueryable()
                                    .Where(d => d.TournamentId == t.Id)
                                    .Select(GetDivisionMapping())
                                    .ToList()
                };
        }

        private static Expression<Func<DivisionEntity, Division>> GetDivisionMapping()
        {
            return
                d =>
                new Division
                {
                    Id = d.Id,
                    Name = d.Name,
                    TournamentId = d.TournamentId,
                    Groups = d.Groups
                                .AsQueryable()
                                .Where(g => g.DivisionId == d.Id)
                                .Select(GetGroupMapping())
                                .ToList()
                };
        }

        private static Expression<Func<GroupEntity, Group>> GetGroupMapping()
        {
            return
                g =>
                new Group
                {
                    Id = g.Id,
                    Name = g.Name,
                    DivisionId = g.DivisionId,
                };
        }

        #endregion
    }
}