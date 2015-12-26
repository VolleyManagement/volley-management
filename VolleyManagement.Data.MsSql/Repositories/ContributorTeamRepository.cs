﻿namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Domain.ContributorsAggregate;

    /// <summary>
    /// Defines implementation of the IContributorRepository contract.
    /// </summary>
    internal class ContributorTeamRepository : IContributorTeamRepository
    {
        private readonly DbSet<Entities.ContributorEntity> _contribsSet;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly VolleyUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorTeamRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContributorTeamRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (VolleyUnitOfWork)unitOfWork;
            this._contribsSet = _unitOfWork.Context.Contributors;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return this._unitOfWork; }
        }

        /// <summary>
        /// Gets all teams with contributors inside.
        /// </summary>
        /// <returns>Collection of teams with contributors</returns>
        public IQueryable<ContributorTeam> Find()
        {
            var result = this._contribsSet.GroupBy(c => c.Team)
                                     .Select(gr => new ContributorTeam
                                        {
                                            Id = gr.Key.Id,
                                            Name = gr.Key.Name,
                                            CourseDirection = gr.Key.CourseDirection,
                                            Contributors = gr.Select(c => new Contributor
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
        /// <param name="updatedEntity">Updated contributor team.</param>
        public void Update(ContributorTeam updatedEntity)
        {
            throw new NotImplementedException();
        }
    }
}
