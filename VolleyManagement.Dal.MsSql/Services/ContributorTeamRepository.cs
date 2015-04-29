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
    using VolleyManagement.Domain.ContributorsAggregate;
    using Dal = VolleyManagement.Dal.MsSql;

    /// <summary>
    /// Defines implementation of the IContributorRepository contract.
    /// </summary>
    internal class ContributorTeamRepository : IContributorTeamRepository
    {
        private const int START_DATABASE_ID_VALUE = 0;

        private readonly ObjectSet<Dal.ContributorTeam> _teamsSet;
        private readonly ObjectSet<Dal.Contributor> _contribsSet;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorTeamRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContributorTeamRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _teamsSet = unitOfWork.Context.CreateObjectSet<Dal.ContributorTeam>();
            _contribsSet = unitOfWork.Context.CreateObjectSet<Dal.Contributor>();
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Gets all teams with contributors inside.
        /// </summary>
        /// <returns>Collection of teams with contributors</returns>
        public IQueryable<Domain.ContributorsAggregate.ContributorTeam> Find()
        {
            var result = _contribsSet.GroupBy(c => c.ContributorTeam)
                                    .Select(gr => new Domain.ContributorsAggregate.ContributorTeam
                                    {
                                        Id = gr.Key.Id,
                                        Name = gr.Key.Name,
                                        CourseDirection = gr.Key.CourseDirection,
                                        Contributors = gr.Select(c => new Domain.ContributorsAggregate.Contributor
                                        {
                                            Id = c.Id,
                                            Name = c.Name
                                        })
                                    });

            return result;
        }

        /// <summary>
        /// Removes contributor by id.
        /// </summary>
        /// <param name="id">The id of contributor team to remove.</param>
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets specified collection of contributor team.
        /// </summary>
        /// <param name="predicate">Condition to find contributors.</param>
        /// <returns>Collection of domain contributors.</returns>
        public IQueryable<ContributorTeam> FindWhere(System.Linq.Expressions.Expression<Func<ContributorTeam, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds new contributor.
        /// </summary>
        /// <param name="newEntity">The contributor team for adding.</param>
        public void Add(ContributorTeam newEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates specified contributor.
        /// </summary>
        /// <param name="oldEntity">The contributor team to update.</param>
        public void Update(ContributorTeam oldEntity)
        {
            throw new NotImplementedException();
        }
    }
}
