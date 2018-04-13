namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using Contracts;
    using Data.Queries.Common;
    using Data.Queries.Team;
    using Domain.TeamsAggregate;
    using Entities;

    /// <summary>
    /// Provides Query Object implementation for Player entity
    /// </summary>
    public class TeamQueries : IQuery<Team, FindByIdCriteria>,
                               IQuery<ICollection<Team>, GetAllCriteria>,
                               IQuery<Team, FindByCaptainIdCriteria>,
                               IQuery<ICollection<TeamTournamentDto>, FindByTournamentIdCriteria>,
                               IQuery<ICollection<Team>, FindTeamsByGroupIdCriteria>,
                               IQuery<ICollection<List<Team>>, FindTeamsInDivisionsByTournamentIdCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;
        private readonly DbSet<DivisionEntity> _dalDivisions;
        private readonly DbSet<GroupEntity> _dalGroups;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork"> The unit of work. </param>
        public TeamQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalDivisions = _unitOfWork.Context.Divisions;
            _dalGroups = _unitOfWork.Context.Groups;
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
            var teams = _unitOfWork.Context.Teams.Where(t => t.Id == criteria.Id).ToList();
            return teams
                .Select(MapTeam)
                .SingleOrDefault();
        }

        /// <summary>
        /// Finds Teams by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Team"/>. </returns>
        public ICollection<Team> Execute(GetAllCriteria criteria)
        {
            var teams = _unitOfWork.Context.Teams.ToList();
            return teams.Select(MapTeam).ToList();
        }

        /// <summary>
        /// Finds Teams by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Team"/>. </returns>
        public Team Execute(FindByCaptainIdCriteria criteria)
        {
            return _unitOfWork.Context.Teams
                .Where(t => t.CaptainId == criteria.CaptainId)
                .Select(MapTeam)
                .SingleOrDefault();
        }


        public ICollection<TeamTournamentDto> Execute(FindByTournamentIdCriteria criteria)
        {
            var result = (from div in _dalDivisions
                          join grp in _dalGroups on div.Id equals grp.DivisionId
                          where div.TournamentId == criteria.TournamentId
                          select grp)
                 .SelectMany(grp =>
                             grp.Teams.Select(
                                 t => new TeamTournamentDto {
                                     DivisionId = grp.DivisionId,
                                     DivisionName = grp.Division.Name,
                                     GroupId = grp.Id,
                                     GroupName = grp.Name,
                                     TeamId = t.Id,
                                     TeamName = t.Name
                                 }))
                 .ToList();

            return result;
        }

        public ICollection<Team> Execute(FindTeamsByGroupIdCriteria criteria)
        {
            return _unitOfWork.Context.Groups
                                      .Where(g => g.Id == criteria.GroupId)
                                      .SelectMany(g => g.Teams)
                                      .Select(MapTeam)
                                      .ToList();
        }

        public ICollection<List<Team>> Execute(FindTeamsInDivisionsByTournamentIdCriteria criteria)
        {
            return _unitOfWork.Context.Tournaments
                                      .Where(t => t.Id == criteria.TournamentId)
                                      .Select(t => t.Divisions)
                                      .SelectMany(d => d.Select(g => g.Groups))
                                      .Select(g => g.SelectMany(t => t.Teams))
                                      .Select(c => c.Select(MapTeam).ToList())
                                      .ToList();
        }

        #endregion

        #region Mapping

        private static Team MapTeam(TeamEntity t)
        {
            return new Team(t.Id,
                            t.Name,
                            t.Coach,
                            t.Achievements,
                            new PlayerId(t.CaptainId),
                            t.Players.Select(p => new PlayerId(p.Id)));
        }

        #endregion
    }
}
