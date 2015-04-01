namespace VolleyManagement.Services
{
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Players;

    /// <summary>
    /// Defines PlayerService
    /// </summary>
    public class PlayerService : IPlayerService
    {
        /// <summary>
        /// Holds PlayerRepository instance.
        /// </summary>
        private readonly IPlayerRepository _playerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerService"/> class.
        /// </summary>
        /// <param name="playerRepository">The user repository</param>
        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        /// <summary>
        /// Method to get all players.
        /// </summary>
        /// <returns>All players.</returns>
        public IQueryable<Player> Get()
        {
            return _playerRepository.Find();
        }

        /// <summary>
        /// Create a new player.
        /// </summary>
        /// <param name="playerToCreate">A Player to create.</param>
        public void Create(Player playerToCreate)
        {
            _playerRepository.Add(playerToCreate);
            _playerRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Finds a Player by id.
        /// </summary>
        /// <param name="id">id for search.</param>
        /// <returns>A found Player.</returns>
        public Player Get(int id)
        {
            var player = _playerRepository.FindWhere(t => t.Id == id).Single();
            return player;
        }

        /// <summary>
        /// Edit player.
        /// </summary>
        /// <param name="playerToEdit">Player to edit.</param>
        public void Edit(Player playerToEdit)
        {
            _playerRepository.Update(playerToEdit);
            _playerRepository.UnitOfWork.Commit();
        }
    }
}
