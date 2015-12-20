namespace VolleyManagement.Data.MsSql.Repositories
{
    using System.Data.Entity;
    using System.Linq;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.MsSql.Mappers;
    using VolleyManagement.Domain.GameResultsAggregate;

    /// <summary>
    /// Defines implementation of the <see cref="IGameResultRepository"/> contract.
    /// </summary>
    internal class GameResultRepository : IGameResultRepository
    {
        private readonly VolleyUnitOfWork _unitOfWork;
        private readonly DbSet<GameResultEntity> _dalGameResults;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameResultRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">Instance of class which implements <see cref="IUnitOfWork"/>.</param>
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
            get
            {
                return _unitOfWork;
            }
        }

        /// <summary>
        /// Adds new game result.
        /// </summary>
        /// <param name="newEntity">Game result to add.</param>
        public void Add(GameResultStorable newEntity)
        {
            var newGameResult = new GameResultEntity();

            DomainToDal.Map(newGameResult, newEntity);
            _dalGameResults.Add(newGameResult);
            _unitOfWork.Commit();
            newEntity.Id = newGameResult.Id;
        }

        /// <summary>
        /// Updates specified game result.
        /// </summary>
        /// <param name="updatedEntity">Updated game result.</param>
        public void Update(GameResultStorable updatedEntity)
        {
            var gameResultToUpdate = _dalGameResults.SingleOrDefault(t => t.Id == updatedEntity.Id);

            if (gameResultToUpdate == null)
            {
                throw new ConcurrencyException();
            }

            DomainToDal.Map(gameResultToUpdate, updatedEntity);
        }

        /// <summary>
        /// Removes game result by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the game result.</param>
        public void Remove(int id)
        {
            var dalToRemove = new GameResultEntity { Id = id };

            _dalGameResults.Attach(dalToRemove);
            _dalGameResults.Remove(dalToRemove);
        }
    }
}
