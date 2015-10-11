namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.MsSql.Mappers;
    using VolleyManagement.Domain.PlayersAggregate;

    /// <summary>
    /// Defines implementation of the IPlayerRepository contract.
    /// </summary>
    internal class PlayerRepository : IPlayerRepository
    {
        private const int START_DATABASE_ID_VALUE = 0;

        /// <summary>
        /// Holds object set of DAL users.
        /// </summary>
        private readonly DbSet<Entities.PlayerEntity> _dalPlayers;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly VolleyUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PlayerRepository(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (VolleyUnitOfWork)unitOfWork;
            this._dalPlayers = _unitOfWork.Context.Players;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return this._unitOfWork; }
        }

        /// <summary>
        /// Gets all players.
        /// </summary>
        /// <returns>Collection of domain players.</returns>
        public IQueryable<Player> Find()
        {
            return this._dalPlayers.Select(p => new Player
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
        public IQueryable<Player> FindWhere(System.Linq.Expressions.Expression<Func<Player, bool>> predicate)
        {
            return this.Find().Where(predicate);
        }

        /// <summary>
        /// Adds new player.
        /// </summary>
        /// <param name="newEntity">The player for adding.</param>
        public void Add(Player newEntity)
        {
            Entities.PlayerEntity newPlayer = DomainToDal.Map(newEntity);
            this._dalPlayers.Add(newPlayer);
            this._unitOfWork.Commit();
            newEntity.Id = newPlayer.Id;
        }

        /// <summary>
        /// Updates specified player.
        /// </summary>
        /// <param name="oldEntity">The player to update.</param>
        public void Update(Player oldEntity)
        {
            if (oldEntity.Id < START_DATABASE_ID_VALUE)
            {
                var exc = new InvalidKeyValueException("Id is invalid for this Entity");
                exc.Data[Constants.ENTITY_ID_KEY] = oldEntity.Id;
                throw exc;
            }

            Entities.PlayerEntity playerToUpdate;
            try
            {
                playerToUpdate = this._dalPlayers.Single(t => t.Id == oldEntity.Id);
            }
            catch (InvalidOperationException e)
            {
                var exc = new InvalidKeyValueException("Entity with request Id does not exist", e);
                exc.Data[Constants.ENTITY_ID_KEY] = oldEntity.Id;
                throw exc;
            }

            playerToUpdate.Id = oldEntity.Id;
            playerToUpdate.FirstName = oldEntity.FirstName;
            playerToUpdate.LastName = oldEntity.LastName;
            playerToUpdate.BirthYear = oldEntity.BirthYear;
            playerToUpdate.Height = oldEntity.Height;
            playerToUpdate.Weight = oldEntity.Weight;
            // playerToUpdate.TeamId = oldEntity.TeamId;
        }

        /// <summary>
        /// Removes player by id.
        /// </summary>
        /// <param name="id">The id of player to remove.</param>
        public void Remove(int id)
        {
            var dalToRemove = new Entities.PlayerEntity { Id = id };
            this._dalPlayers.Attach(dalToRemove);
            this._dalPlayers.Remove(dalToRemove);
        }
    }
}
