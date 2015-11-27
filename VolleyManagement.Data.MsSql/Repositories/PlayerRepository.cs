namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.MsSql.Mappers;
    using VolleyManagement.Data.MsSql.Repositories.Specifications;
    using VolleyManagement.Domain.PlayersAggregate;

    /// <summary>
    /// Defines implementation of the IPlayerRepository contract.
    /// </summary>
    internal class PlayerRepository : IPlayerRepository
    {
        private const int START_DATABASE_ID_VALUE = 0;

        private readonly PlayerStorageSpecification _dbStorageSpecification
            = new PlayerStorageSpecification();

        private readonly DbSet<PlayerEntity> _dalPlayers;

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
        /// Adds new player.
        /// </summary>
        /// <param name="newModel">The player for adding.</param>
        public void Add(Player newModel)
        {
            var newPlayer = new PlayerEntity();
            DomainToDal.Map(newPlayer, newModel);

            if (!_dbStorageSpecification.IsSatisfiedBy(newPlayer))
            {
                throw new InvalidEntityException();
            }

            this._dalPlayers.Add(newPlayer);
            this._unitOfWork.Commit();
            newModel.Id = newPlayer.Id;
        }

        /// <summary>
        /// Updates specified player.
        /// </summary>
        /// <param name="updatedModel">Updated player.</param>
        public void Update(Player updatedModel)
        {
            if (updatedModel.Id < START_DATABASE_ID_VALUE)
            {
                var exc = new InvalidKeyValueException("Id is invalid for this Entity");
                exc.Data[Constants.ENTITY_ID_KEY] = updatedModel.Id;
                throw exc;
            }

            var playerToUpdate = this._dalPlayers.SingleOrDefault(t => t.Id == updatedModel.Id);

            if (playerToUpdate == null)
            {
                throw new ConcurrencyException();
            }

            DomainToDal.Map(playerToUpdate, updatedModel);
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
