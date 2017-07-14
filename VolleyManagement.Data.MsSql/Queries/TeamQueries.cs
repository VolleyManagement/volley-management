namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Contracts;
    using Data.Queries.Common;
    using Data.Queries.Team;
    using Domain.PlayersAggregate;
    using Domain.TeamsAggregate;
    using Entities;

    /// <summary>
    /// Provides Query Object implementation for Player entity
    /// </summary>
    public class TeamQueries : IQuery<Team, FindByIdCriteria>,
                               IQuery<List<Team>, GetAllCriteria>,
                               IQuery<Team, FindByCaptainIdCriteria>,
                               IQuery<List<Team>, FindByTournamentIdCriteria>,
                               IQuery<List<Team>, FindByGroupIdCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork"> The unit of work. </param>
        public TeamQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds Team by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Player"/>. </returns>
        public Team Execute(FindByIdCriteria criteria)
        {
            return _unitOfWork.Context.Teams.Where(t => t.Id == criteria.Id).Select(GetTeamMapping()).SingleOrDefault();
        }

        /// <summary>
        /// Finds Teams by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Team"/>. </returns>
        public List<Team> Execute(GetAllCriteria criteria)
        {
            return _unitOfWork.Context.Teams.Select(GetTeamMapping()).ToList();
        }

        /// <summary>
        /// Finds Teams by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Team"/>. </returns>
        public Team Execute(FindByCaptainIdCriteria criteria)
        {
            return _unitOfWork.Context.Teams.Where(t => t.CaptainId == criteria.CaptainId).Select(GetTeamMapping()).SingleOrDefault();
        }

        /// <summary>
        /// Find teams by given criteria
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>List of <see cref="Team"/>.</returns>
        public List<Team> Execute(FindByTournamentIdCriteria criteria)
        {
            return _unitOfWork.Context.Tournaments
                                      .Where(t => t.Id == criteria.TournamentId)
                                      .SelectMany(t => t.Divisions)
                                      .SelectMany(d => d.Groups)
                                      .SelectMany(g => g.Teams)
                                      .Select(GetTeamMapping())
                                      .ToList();
        }

        public List<Team> Execute(FindByGroupIdCriteria criteria)
        {
            return _unitOfWork.Context.Groups
                                      .Where(g => g.Id == criteria.GroupId)
                                      .SelectMany(g => g.Teams)
                                      .Select(GetTeamMapping())
                                      .ToList();
        }

        #endregion

        #region Mapping

        private static Expression<Func<TeamEntity, Team>> GetTeamMapping()
        {
            return t => new Team
            {
                Id = t.Id,
                Name = t.Name,
                Coach = t.Coach,
                CaptainId = t.CaptainId,
                Achievements = t.Achievements
            };
        }

        #endregion
    }
}
