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
    using Domain = VolleyManagement.Domain.Contributors;

    /// <summary>
    /// Defines implementation of the IPlayerRepository contract.
    /// </summary>
    internal class ContributorRepository : IContributorRepository
    {
        private const int START_DATABASE_ID_VALUE = 0;

        /// <summary>
        /// Holds object set of DAL users.
        /// </summary>
        private readonly ObjectSet<Dal.Contributor> _dalContributors;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContributorRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dalContributors = unitOfWork.Context.CreateObjectSet<Dal.Contributor>();
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Gets all contributors.
        /// </summary>
        /// <returns>Collection of domain contributors.</returns>
        public IQueryable<Domain.Contributor> Find()
        {
            return _dalContributors.Select(p => new Domain.Contributor
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                BirthYear = p.BirthYear,
                ContributorTeamId = p.ContributorTeamId
            });
        }
    }
}
