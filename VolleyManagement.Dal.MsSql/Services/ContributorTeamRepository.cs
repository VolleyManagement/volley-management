namespace VolleyManagement.Dal.MsSql.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Text;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Dal.MsSql.Mappers;
    using Dal = VolleyManagement.Dal.MsSql;
    using VolleyManagement.Domain.ContributorTeams;

    /// <summary>
    /// Defines implementation of the IContributorRepository contract.
    /// </summary>
    internal class ContributorTeamRepository : IContributorTeamRepository
    {
        private const int START_DATABASE_ID_VALUE = 0;

        private readonly ObjectSet<Dal.ContributorTeam> _dalContributorTeam;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContributorTeamRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dalContributorTeam = unitOfWork.Context.CreateObjectSet<Dal.ContributorTeam>();
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Gets all contributors Team.
        /// </summary>
        /// <returns>Collection of domain contributor team.</returns>
        public IQueryable<Domain.ContributorTeam> Find()
        {
            return _dalContributorTeam.Select(c => new Domain.ContributorTeam
            {
                Id = c.Id,
                Name = c.Name,
                CourseDirection = c.CourseDirection
            });
        }

        /// <summary>
        /// Gets specified collection of contributor team.
        /// </summary>
        /// <param name="predicate">Condition to find contributors.</param>
        /// <returns>Collection of domain contributors.</returns>
        public IQueryable<Domain.ContributorTeam> FindWhere(System.Linq.Expressions.Expression<Func<Domain.ContributorTeam, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds new contributor.
        /// </summary>
        /// <param name="newEntity">The contributor team for adding.</param>
        public void Add(Domain.ContributorTeam newEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates specified contributor.
        /// </summary>
        /// <param name="oldEntity">The contributor team to update.</param>
        public void Update(Domain.ContributorTeam oldEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes contributor by id.
        /// </summary>
        /// <param name="id">The id of contributor team to remove.</param>
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
