namespace VolleyManagement.Data.MsSql.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Contracts;
    using Domain.GamesAggregate;
    using Entities;
    using Exceptions;
    using Mappers;

    /// <summary>
    /// Defines implementation of the <see cref="IGameRepository"/> contract.
    /// </summary>
    internal class GameRepository : IGameRepository
    {
        private readonly VolleyUnitOfWork _unitOfWork;
        private readonly DbSet<GameResultEntity> _dalGame;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">Instance of class which implements <see cref="IUnitOfWork"/>.</param>
        public GameRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalGame = _unitOfWork.Context.GameResults;
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
        /// Adds new game.
        /// </summary>
        /// <param name="newEntity">Game to add.</param>
        public void Add(Game newEntity)
        {
            var newGame = new GameResultEntity();

            DomainToDal.Map(newGame, newEntity);
            _dalGame.Add(newGame);
            _unitOfWork.Commit();
            newEntity.Id = newGame.Id;
        }

        /// <summary>
        /// Updates specified game.
        /// </summary>
        /// <param name="updatedEntity">Updated game</param>
        public void Update(Game updatedEntity)
        {
            var gameToUpdate = _dalGame.SingleOrDefault(t => t.Id == updatedEntity.Id);

            if (gameToUpdate == null)
            {
                throw new ConcurrencyException();
            }

            DomainToDal.Map(gameToUpdate, updatedEntity);
        }

        /// <summary>
        /// Removes game by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the game.</param>
        public void Remove(int id)
        {
            GameResultEntity dalToRemove;
            if (_dalGame.Local.Any(gr => gr.Id == id))
            {
                dalToRemove = _dalGame.Find(id);
            }
            else
            {
                dalToRemove = new GameResultEntity { Id = id };
                _dalGame.Attach(dalToRemove);
            }
            _dalGame.Remove(dalToRemove);
        }
    }
}
