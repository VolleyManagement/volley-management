namespace VolleyManagement.Services
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.Domain.Teams;
    using DAL = VolleyManagement.Dal.Contracts;
    using IsolationLevel = System.Data.IsolationLevel;

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
        /// <param name="teamRepository">The team repository</param>
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
            using (IDbTransaction transaction = _playerRepository.UnitOfWork.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                if (playerToCreate.TeamId != null)
                {
                    if (_teamRepository.FindWhere(t => t.Id == playerToCreate.TeamId).SingleOrDefault() == null)
                    {
                        throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, playerToCreate.TeamId);
                    }
                }

                _playerRepository.Add(playerToCreate);
                _playerRepository.UnitOfWork.Commit();
                transaction.Commit();
            }
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
                player = _playerRepository.FindWhere(p => p.Id == id).Single();
            }
            catch (InvalidOperationException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, ex);
            }

            return player;
        }

        /// <summary>
        /// Edit player.
        /// </summary>
        /// <param name="playerToEdit">Player to edit.</param>
        public void Edit(Player playerToEdit)
        {
            using (IDbTransaction transaction = _playerRepository.UnitOfWork.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                // Check if player is captain of team and teamId is null or changed
                Team ledTeam = GetPlayerLedTeam(playerToEdit.Id);
                if (ledTeam != null &&
                    (playerToEdit.TeamId == null || playerToEdit.TeamId != ledTeam.Id))
                {
                    var ex = new ValidationException(ServiceResources.ExceptionMessages.PlayerIsCaptainOfAnotherTeam);
                    ex.Data[Domain.Constants.ExceptionManagement.ENTITY_ID_KEY] = ledTeam.Id;
                    throw ex;
                }
                else if (playerToEdit.TeamId != null
                         && _teamRepository.FindWhere(t => t.Id == playerToEdit.TeamId).SingleOrDefault() == null)
                {
                    throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, playerToEdit.TeamId);
                }

                try
                {
                    _playerRepository.Update(playerToEdit);
                }
                catch (InvalidKeyValueException ex)
                {
                    throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, ex);
                }

                _playerRepository.UnitOfWork.Commit();
                transaction.Commit();
            }
        }

        /// <summary>
        /// Delete player by id.
        /// </summary>
        /// <param name="id">The id of player to delete.</param>
        public void Delete(int id)
        {
            Team playerTeam = GetPlayerLedTeam(id);
            if (playerTeam != null)
            {
                var ex = new ValidationException(ServiceResources.ExceptionMessages.PlayerIsCaptainOfAnotherTeam);
                ex.Data[Domain.Constants.ExceptionManagement.ENTITY_ID_KEY] = playerTeam.Id;
                throw ex;
            }

            try
            {
                _playerRepository.Remove(id);
                _playerRepository.UnitOfWork.Commit();
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, id, ex);
            }
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
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, player.TeamId, ex);
            }
        }

        private Team GetPlayerLedTeam(int playerId)
        {
            return _teamRepository.FindWhere(t => t.CaptainId == playerId).SingleOrDefault();
        }
    }
}
