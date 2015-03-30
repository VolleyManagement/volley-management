namespace VolleyManagement.Dal.MsSql.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Text;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.MsSql.Mappers;
    using Dal = VolleyManagement.Dal.MsSql;
    using Domain = VolleyManagement.Domain.Players;

    /// <summary>
    /// Defines implementation of the IPlayerRepository contract.
    /// </summary>
    internal class PlayerRepository : IPlayerRepository
    {
        /// <summary>
        /// Holds object set of DAL users.
        /// </summary>
        private readonly ObjectSet<Dal.Player> _dalPlayers;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PlayerRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dalPlayers = unitOfWork.Context.CreateObjectSet<Dal.Player>();
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Gets all players.
        /// </summary>
        /// <returns>Collection of domain players.</returns>
        public IQueryable<Domain.Player> Find()
        {
            return _dalPlayers.Select(p => new Domain.Player
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                BirthYear = p.BirthYear,
                Height = p.Height,
                Weight = p.Weight
            });
        }

        /// <summary>
        /// Gets specified collection of players.
        /// </summary>
        /// <param name="predicate">Condition to find players.</param>
        /// <returns>Collection of domain players.</returns>
        public IQueryable<Domain.Player> FindWhere(System.Linq.Expressions.Expression<Func<Domain.Player, bool>> predicate)
        {
            return Find().Where(predicate);
        }

        /// <summary>
        /// Adds new player.
        /// </summary>
        /// <param name="newEntity">The player for adding.</param>
        public void Add(Domain.Player newEntity)
        {
            Dal.Player newPlayer = DomainToDal.Map(newEntity);
            _dalPlayers.AddObject(newPlayer);
            _unitOfWork.Commit();
            newEntity.Id = newPlayer.Id;
        }

        /// <summary>
        /// Updates specified player.
        /// </summary>
        /// <param name="oldEntity">The player to update.</param>
        public void Update(Domain.Player oldEntity)
        {
            var playerToUpdate = _dalPlayers.Where(t => t.Id == oldEntity.Id).Single();
            if (playerToUpdate.FirstName != oldEntity.FirstName
             || playerToUpdate.LastName != oldEntity.LastName
             || playerToUpdate.BirthYear != oldEntity.BirthYear
             || playerToUpdate.Height != oldEntity.Height
             || playerToUpdate.Weight != oldEntity.Weight)
            {
                playerToUpdate.FirstName = oldEntity.FirstName;
                playerToUpdate.LastName = oldEntity.LastName;
                playerToUpdate.BirthYear = oldEntity.BirthYear;
                playerToUpdate.Height = oldEntity.Height;
                playerToUpdate.Weight = oldEntity.Weight;
                _dalPlayers.Context.ObjectStateManager.ChangeObjectState(playerToUpdate, EntityState.Modified);
            }
        }

        /// <summary>
        /// Removes player by id.
        /// </summary>
        /// <param name="id">The id of player to remove.</param>
        public void Remove(int id)
        {
            var dalToRemove = new Dal.Player { Id = id };
            _dalPlayers.Attach(dalToRemove);
            _dalPlayers.DeleteObject(dalToRemove);
        }
    }
}
