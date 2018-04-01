namespace VolleyManagement.Data.MsSql.Repositories
{
    using System.Data.Entity;
    using System.Linq;
    using Contracts;
    using Domain.PlayersAggregate;
    using Entities;
    using Exceptions;
    using Mappers;
    using Specifications;

    /// <summary>
    /// Defines implementation of the IPlayerRepository contract.
    /// </summary>
    internal class PlayerRepository : IPlayerRepository
    {
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
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalPlayers = _unitOfWork.Context.Players;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        /// <summary>
        /// Adds new player.
        /// </summary>
        /// <param name="newEntity">The player for adding.</param>
        public void Add(Player newEntity)
        {
            var newPlayer = new PlayerEntity();
            DomainToDal.Map(newPlayer, newEntity);

            if (!_dbStorageSpecification.IsSatisfiedBy(newPlayer))
            {
                throw new InvalidEntityException();
            }

            _dalPlayers.Add(newPlayer);
            _unitOfWork.Commit();
            newEntity.Id = newPlayer.Id;
        }

        /// <summary>
        /// Updates specified player.
        /// </summary>
        /// <param name="updatedEntity">Updated player.</param>
        public void Update(Player updatedEntity)
        {
            if (updatedEntity.Id < Constants.START_DATABASE_ID_VALUE)
            {
                var exc = new InvalidKeyValueException(Properties.Resources.InvalidEntityId);
                exc.Data[Constants.ENTITY_ID_KEY] = updatedEntity.Id;
                throw exc;
            }

            var playerToUpdate = _dalPlayers.SingleOrDefault(t => t.Id == updatedEntity.Id);

            if (playerToUpdate == null)
            {
                throw new ConcurrencyException();
            }

            DomainToDal.Map(playerToUpdate, updatedEntity);
        }

        /// <summary>
        /// Removes player by id.
        /// </summary>
        /// <param name="id">The id of player to remove.</param>
        public void Remove(int id)
        {
            var dalToRemove = new PlayerEntity { Id = id };
            _dalPlayers.Attach(dalToRemove);
            _dalPlayers.Remove(dalToRemove);
        }

        public void UpdateTeam(Player updatedEntity, int? teamId)
        {
            if (updatedEntity.Id < Constants.START_DATABASE_ID_VALUE)
            {
                var exc = new InvalidKeyValueException(Properties.Resources.InvalidEntityId);
                exc.Data[Constants.ENTITY_ID_KEY] = updatedEntity.Id;
                throw exc;
            }

            var playerToUpdate = _dalPlayers.SingleOrDefault(t => t.Id == updatedEntity.Id);

            if (playerToUpdate == null)
            {
                throw new ConcurrencyException();
            }

            playerToUpdate.Id = teamId.Value;
            _unitOfWork.Commit();
        }
    }
}
