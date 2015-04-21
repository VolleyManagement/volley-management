namespace VolleyManagement.Services
{
    using System;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.Domain.Teams;

    using DAL = VolleyManagement.Dal.Contracts;

    /// <summary>
    /// Defines PlayerService
    /// </summary>
    public class PlayerService : IPlayerService
    {
        /// <summary>
        /// Holds PlayerRepository instance.
        /// </summary>
        private readonly IPlayerRepository _playerRepository;

        private readonly ITeamRepository _teamRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerService"/> class.
        /// </summary>
        /// <param name="playerRepository">The player repository</param>
        /// <param name="teamService">Service which provide basic operation with team repository</param>
        public PlayerService(IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
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
            Player player;
            try
            {
                player = _playerRepository.FindWhere(t => t.Id == id).Single();
            }
            catch (InvalidOperationException ex)
            {
                throw new MissingEntityException("Player with specified Id can not be found", ex);
            }

            return player;
        }

        /// <summary>
        /// Edit player.
        /// </summary>
        /// <param name="playerToEdit">Player to edit.</param>
        public void Edit(Player playerToEdit)
        {
            try
            {
                _playerRepository.Update(playerToEdit);
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException("Player with specified Id can not be found", ex);
            }

            // TODO: Handle cases:
            // 1. teamId isn't exist
            // 2. after updating some team will lose required field captainId
            _playerRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Delete player by id.
        /// </summary>
        /// <param name="id">The id of player to delete.</param>
        public void Delete(int id)
        {
            try
            {
                _playerRepository.Remove(id);
                _playerRepository.UnitOfWork.Commit();
            }
            catch (InvalidKeyValueException ex)
            {
                var serviceException = new MissingEntityException("Player with specified Id can not be found", ex);
                throw serviceException;
            }

            // TODO: Handle case if after deleting some team will lose required field captainId
        }

        /// <summary>
        /// Find team of specified player
        /// </summary>
        /// <param name="player">Player which team should be found</param>
        /// <returns>Player's team</returns>
        public Team GetPlayerTeam(Player player)
        {
            if (player.TeamId == null)
            {
                return null;
            }
            
            try
            {
                return _teamRepository.FindWhere(t => t.Id == player.TeamId).Single();
            }
            catch (InvalidOperationException ex)
            {
                throw new MissingEntityException("Team with specified Id can not be found", ex);
            }
        }
    }
}
