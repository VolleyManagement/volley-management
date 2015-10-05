namespace VolleyManagement.Data.MsSql.Services
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Linq.Expressions;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Mappers;
    using VolleyManagement.Domain.TournamentsAggregate;

    using DAL = VolleyManagement.Data.MsSql;
    using Domain = VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Defines implementation of the ITournamentRepository contract.
    /// </summary>
    internal class TournamentRepository : ITournamentRepository
    {
        private readonly ObjectSet<DAL.Tournament> _dalTournaments;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public TournamentRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._dalTournaments = unitOfWork.Context.CreateObjectSet<DAL.Tournament>();
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return this._unitOfWork; }
        }

        /// <summary>
        /// Gets all tournaments.
        /// </summary>
        /// <returns>Collection of domain tournaments.</returns>
        public IQueryable<Tournament> Find()
        {
            return this._dalTournaments.Select(t => new Tournament
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    RegulationsLink = t.RegulationsLink,
                    Scheme = (TournamentSchemeEnum)t.Scheme,
                    Season = t.Season
                });
        }

        /// <summary>
        /// Gets specified collection of tournaments.
        /// </summary>
        /// <param name="predicate">Condition to find tournaments.</param>
        /// <returns>Collection of domain tournaments.</returns>
        public IQueryable<Tournament> FindWhere(
            Expression<Func<Tournament, bool>> predicate)
        {
            return this.Find().Where(predicate);
        }

        /// <summary>
        /// Adds new tournament.
        /// </summary>
        /// <param name="newEntity">The tournament for adding.</param>
        public void Add(Tournament newEntity)
        {
            DAL.Tournament newTournament = DomainToDal.Map(newEntity);
            this._dalTournaments.AddObject(newTournament);
            this._unitOfWork.Commit();
            newEntity.Id = newTournament.Id;
        }

        /// <summary>
        /// Updates specified tournament.
        /// </summary>
        /// <param name="oldEntity">The tournament to update.</param>
        public void Update(Tournament oldEntity)
        {
            var tournamentToUpdate = this._dalTournaments.Single(t => t.Id == oldEntity.Id);
            tournamentToUpdate.Name = oldEntity.Name;
            tournamentToUpdate.Description = oldEntity.Description;
            tournamentToUpdate.RegulationsLink = oldEntity.RegulationsLink;
            tournamentToUpdate.Scheme = (byte)oldEntity.Scheme;
            tournamentToUpdate.Season = oldEntity.Season;
            this._dalTournaments.Context.ObjectStateManager.ChangeObjectState(tournamentToUpdate, EntityState.Modified);
        }

        /// <summary>
        /// Removes tournament by id.
        /// </summary>
        /// <param name="id">The id of tournament to remove.</param>
        public void Remove(int id)
        {
            var dalToRemove = new DAL.Tournament { Id = id };
            this._dalTournaments.Attach(dalToRemove);
            this._dalTournaments.DeleteObject(dalToRemove);
        }
    }
}
