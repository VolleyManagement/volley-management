namespace VolleyManagement.Dal.MsSql.Services
{
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    using VolleyManagement.Dal.Contracts;
    using Dal = VolleyManagement.Dal.MsSql;
    using Domain = VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Defines implementation of the ITournamentRepository contract.
    /// </summary>
    internal class TournamentRepository : ITournamentRepository
    {
        /// <summary>
        /// Holds object set of DAL tournaments.
        /// </summary>
        private readonly ObjectSet<Dal.Tournament> dalTournaments;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public TournamentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            dalTournaments = unitOfWork.Context.CreateObjectSet<Dal.Tournament>();
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Gets all tournaments.
        /// </summary>
        /// <returns>Collection of domain tournaments.</returns>
        public IQueryable<Domain.Tournament> FindAll()
        {
            return dalTournaments.Select(t => new Domain.Tournament
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                RegulationsLink = t.RegulationsLink,
                Scheme = t.Scheme,
                Season = t.Season
            });
        }

        /// <summary>
        /// Gets specified collection of tournaments.
        /// </summary>
        /// <param name="predicate">Condition to find tournaments.</param>
        /// <returns>Collection of domain tournaments.</returns>
        public IQueryable<Domain.Tournament> FindWhere(Expression<Func<Domain.Tournament, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds new tournament.
        /// </summary>
        /// <param name="newEntity">The tournament to add.</param>
        public void Add(Domain.Tournament newEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates specified tournament.
        /// </summary>
        /// <param name="oldEntity">The tournament to update.</param>
        public void Update(Domain.Tournament oldEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes specified tournament.
        /// </summary>
        /// <param name="entity">The tournament to remove.</param>
        public void Remove(Domain.Tournament entity)
        {
            throw new NotImplementedException();
        }
    }
}
