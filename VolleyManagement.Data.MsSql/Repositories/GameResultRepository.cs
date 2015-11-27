namespace VolleyManagement.Data.MsSql.Repositories
{
    using System.Data.Entity;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.MsSql.Mappers;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Defines implementation of the IGameResultRepository contract.
    /// </summary>
    internal class GameResultRepository : IGameResultRepository
    {
        private readonly DbSet<GameResultEntity> _dalGameResults;

        private readonly VolleyUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public GameResultRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalGameResults = _unitOfWork.Context.GameResults;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Adds new game result.
        /// </summary>
        /// <param name="newModel">Game result to add.</param>
        public void Add(GameResult newModel)
        {
            var newEntity = new GameResultEntity();
            DomainToDal.Map(newEntity, newModel);
            _dalGameResults.Add(newEntity);
            _unitOfWork.Commit();
            newModel.Id = newEntity.Id;
        }

        /// <summary>
        /// Updates specified game result.
        /// </summary>
        /// <param name="updatedModel">Updated game result.</param>
        public void Update(GameResult updatedModel)
        {
            var oldEntity = _dalGameResults.SingleOrDefault(gr => gr.Id == updatedModel.Id);

            if (oldEntity == null)
            {
                throw new ConcurrencyException();
            }

            DomainToDal.Map(oldEntity, updatedModel);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Removes game result by specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the game result.</param>
        public void Remove(int id)
        {
            var entity = new GameResultEntity { Id = id };

            if (entity == null)
            {
                throw new ConcurrencyException();
            }

            _dalGameResults.Attach(entity);
            _dalGameResults.Remove(entity);
            _unitOfWork.Commit();
        }
    }
}
