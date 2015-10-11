namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Mappers;

    using Dal = VolleyManagement.Data.MsSql.Entities;
    using Domain = VolleyManagement.Domain.TeamsAggregate;

    /// <summary>
    /// Defines implementation of the ITeamRepository contract.
    /// </summary>
    internal class TeamRepository : Domain.ITeamRepository
    {
        private readonly DbSet<Dal.TeamEntity> _dalTeams;

        private readonly VolleyUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public TeamRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (VolleyUnitOfWork)unitOfWork;
            this._dalTeams = _unitOfWork.Context.Teams;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return this._unitOfWork; }
        }

        /// <summary>
        /// Gets all teams.
        /// </summary>
        /// <returns>Collection of domain teams.</returns>
        public IQueryable<VolleyManagement.Domain.TeamsAggregate.Team> Find()
        {
            return this._dalTeams.Select(t => new VolleyManagement.Domain.TeamsAggregate.Team
            {
                Id = t.Id,
                Name = t.Name,
                Coach = t.Coach,
                CaptainId = t.Captain.Id
            });
        }

        /// <summary>
        /// Gets specified collection of teams.
        /// </summary>
        /// <param name="predicate">Condition to find teams.</param>
        /// <returns>Collection of domain teams.</returns>
        public IQueryable<Domain.Team> FindWhere(System.Linq.Expressions.Expression<Func<Domain.Team, bool>> predicate)
        {
            return this.Find().Where(predicate);
        }

        /// <summary>
        /// Adds new team.
        /// </summary>
        /// <param name="newEntity">The team for adding.</param>
        public void Add(Domain.Team newEntity)
        {
            Dal.TeamEntity newTeam = DomainToDal.Map(newEntity);
            this._dalTeams.Add(newTeam);
            this._unitOfWork.Commit();

            newEntity.Id = newTeam.Id;
        }

        /// <summary>
        /// Removes team by id.
        /// </summary>
        /// <param name="id">The id of team to remove.</param>
        public void Remove(int id)
        {
            var dalToRemove = new Dal.TeamEntity { Id = id };
            this._dalTeams.Attach(dalToRemove);
            this._dalTeams.Remove(dalToRemove);
        }

        /// <summary>
        /// Updates specified team.
        /// </summary>
        /// <param name="oldEntity">The team to update</param>
        public void Update(VolleyManagement.Domain.TeamsAggregate.Team oldEntity)
        {
            throw new NotImplementedException();
        }
    }
}
