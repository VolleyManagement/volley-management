namespace VolleyManagement.Data.MsSql.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.MsSql.Mappers;
    using VolleyManagement.Domain.GamesAggregate;

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
            var dalToRemove = new GameResultEntity { Id = id };

            _dalGame.Attach(dalToRemove);
            _dalGame.Remove(dalToRemove);
        }

        /// <summary>
        /// Removes all games in tournament by tournament's id
        /// </summary>
        /// <param name="tournamentId">Identifier of the tournament</param>
        public void RemoveAllGamesInTournament(int tournamentId)
        {
            var gamesToRemove = _dalGame.Where(gr => gr.TournamentId == tournamentId);
            _dalGame.RemoveRange(gamesToRemove);
        }

        /// <summary>
        /// Adds collection of new games.
        /// </summary>
        /// <param name="games">Collection of games to add</param>
        public void AddGamesInTournament(List<Game> games)
        {
            foreach (var game in games)
            {
                var newGame = new GameResultEntity();
                DomainToDal.Map(newGame, game);
                _dalGame.Add(newGame);
            }
        }
    }
}
