namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
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
        /// <param name="newEntity">Game result to add.</param>
        public void Add(GameResult newEntity)
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
        public void Update(GameResult updatedEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes game result by specified identifier.
        /// </summary>
        /// <param name="id">Identifier of the game result.</param>
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
