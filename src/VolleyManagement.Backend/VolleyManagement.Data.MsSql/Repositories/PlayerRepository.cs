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
        /// Adds new player.
        /// </summary>
        /// <param name="playerDto">The player for adding.</param>
        public Player Add(CreatePlayerDto playerDto)
        {
            var newEntity = new PlayerEntity {
                FirstName = playerDto.FirstName,
                LastName = playerDto.LastName,
                BirthYear = playerDto.BirthYear,
                Height = playerDto.Height,
                Weight = playerDto.Weight
            };

            if (!_dbStorageSpecification.IsSatisfiedBy(newEntity))
            {
                throw new InvalidEntityException();
            }

            _dalPlayers.Add(newEntity);
            _unitOfWork.Commit();

            return new Player(newEntity.Id, newEntity.FirstName, newEntity.LastName) {
                BirthYear = newEntity.BirthYear,
                Height = newEntity.Height,
                Weight = newEntity.Weight
            };
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
            _unitOfWork.Commit();
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
            _unitOfWork.Commit();
        }
    }
}
