namespace VolleyManagement.Data.MsSql.Queries
{
    using Contracts;
    using Data.Queries.Common;
    using Data.Queries.Tournament;
    using Domain.TournamentsAggregate;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Queries.Division;
    using Data.Queries.Group;
    using System.Linq.Expressions;
    using System;
    using Entities;

    /// <summary>
    /// Provides Object Query implementation for Tournaments
    /// </summary>
    public class TournamentQueries : IQuery<Tournament, UniqueTournamentCriteria>,
                                     IQuery<ICollection<Tournament>, GetAllCriteria>,
                                     IQuery<Tournament, FindByIdCriteria>,
                                     IQuery<ICollection<Division>, TournamentDivisionsCriteria>,
                                     IQuery<ICollection<Group>, DivisionGroupsCriteria>,
                                     IQuery<TournamentScheduleDto, TournamentScheduleInfoCriteria>,
                                     IQuery<Tournament, TournamentByGroupCriteria>,
                                     IQuery<ICollection<Tournament>, OldTournamentsCriteria>
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
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
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
            return query
                .Select(GetTournamentMapping())
                .FirstOrDefault();
        }

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Tournament"/>. </returns>
        public Tournament Execute(TournamentByGroupCriteria criteria)
        {
            var tournament = from g in _unitOfWork.Context.Groups
                             join d in _unitOfWork.Context.Divisions on g.DivisionId equals d.Id
                             join t in _unitOfWork.Context.Tournaments on d.TournamentId equals t.Id
                             where g.Id == criteria.GroupId
                             select t;

            return tournament
                .Select(GetTournamentMapping())
                .FirstOrDefault();
        }

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Tournament"/>. </returns>
        public ICollection<Tournament> Execute(GetAllCriteria criteria)
        {
            return _unitOfWork.Context.Tournaments
                .Select(GetTournamentMapping())
                .ToList();
        }

        /// <summary>
        /// Finds List of Tournament by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Tournament"/>. </returns>
        public ICollection<Tournament> Execute(OldTournamentsCriteria criteria)
        {
            return _unitOfWork.Context.Tournaments
                                      .Where(t => t.IsArchived == false)
                                      .Where(t => t.GamesEnd <= criteria.CheckDate)
                                      .Select(GetTournamentMapping())
                                      .ToList();
        }

        /// <summary>
        /// Find Divisions by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Division"/>. </returns>
        public ICollection<Division> Execute(TournamentDivisionsCriteria criteria)
        {
            return _unitOfWork.Context.Divisions
                                      .Where(d => d.TournamentId == criteria.TournamentId)
                                      .Select(GetDivisionMapping())
                                      .ToList();
        }

        /// <summary>
        /// Find Groups by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Division"/>. </returns>
        public ICollection<Group> Execute(DivisionGroupsCriteria criteria)
        {
            return _unitOfWork.Context.Groups
                                      .Where(d => d.DivisionId == criteria.DivisionId)
                                      .Select(GetGroupMapping())
                                      .ToList();
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

        /// <summary>
        /// Finds tournament data transfer object by tournament id
        /// </summary>
        /// <param name="criteria">Tournament id criteria</param>
        /// <returns>The <see cref="TournamentScheduleDto"/></returns>
        public TournamentScheduleDto Execute(TournamentScheduleInfoCriteria criteria)
        {
            var query = from t in _unitOfWork.Context.Tournaments
                        where t.Id == criteria.TournamentId
                        join d in _unitOfWork.Context.Divisions
                            on t.Id equals d.TournamentId
                        join g in _unitOfWork.Context.Groups
                            on d.Id equals g.DivisionId
                        select new
                        {
                            TournamentId = t.Id,
                            TournamentName = t.Name,
                            StartDate = t.GamesStart,
                            EndDate = t.GamesEnd,
                            Scheme = t.Scheme,
                            DivisionId = d.Id,
                            DivisionName = d.Name,
                            GroupTeamCount = g.Teams.Count
                        };

            var result = query.ToList();

            return result.Select(p => new TournamentScheduleDto
            {
                Id = p.TournamentId,
                Name = p.TournamentName,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Scheme = (TournamentSchemeEnum)p.Scheme,
                Divisions = result.Select(d => new DivisionScheduleDto
                {
                    DivisionId = d.DivisionId,
                    DivisionName = d.DivisionName,
                    TeamCount = d.GroupTeamCount
                }).ToList()
            }).FirstOrDefault();
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
                                    .ToList(),
                    LastTimeUpdated = t.LastTimeUpdated,
                    IsArchived = t.IsArchived
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
                    IsEmpty = g.Teams.Count == 0
                };
        }

        #endregion
    }
}
