namespace VolleyManagement.Services
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.Team;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;

    /// <summary>
    /// Defines PlayerService
    /// </summary>
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        private readonly IQuery<Player, FindByIdCriteria> _getPlayerByIdQuery;

        private readonly IQuery<Team, FindByIdCriteria> _getTeamByIdQuery;

        private readonly IQuery<IQueryable<Player>, GetAllCriteria> _getAllPlayersQuery;

        private readonly IQuery<Team, FindByCaptainIdCriteria> _getTeamByCaptainQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerService"/> class.
        /// </summary>
        /// <param name="playerRepository"> The player repository </param>
        /// <param name="getTeamByIdQuery"> Get By ID query for Teams</param>
        /// <param name="getPlayerByIdQuery"> Get By ID query for Players</param>
        /// <param name="getAllPlayersQuery"> Get All players query</param>
        /// <param name="getTeamByCaptainQuery">Get Player by Captain query</param>
        public PlayerService(
            IPlayerRepository playerRepository,
            IQuery<Team, FindByIdCriteria> getTeamByIdQuery,
            IQuery<Player, FindByIdCriteria> getPlayerByIdQuery,
            IQuery<IQueryable<Player>, GetAllCriteria> getAllPlayersQuery,
            IQuery<Team, FindByCaptainIdCriteria> getTeamByCaptainQuery)
        {
            _playerRepository = playerRepository;
            _getTeamByIdQuery = getTeamByIdQuery;
            _getPlayerByIdQuery = getPlayerByIdQuery;
            _getAllPlayersQuery = getAllPlayersQuery;
            _getTeamByCaptainQuery = getTeamByCaptainQuery;
        }

        /// <summary>
        /// Method to get all players.
        /// </summary>
        /// <returns>All players.</returns>
        public IQueryable<Player> Get()
        {
            return _getAllPlayersQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Create a new player.
        /// </summary>
        /// <param name="playerToCreate">A Player to create.</param>
        public void Create(Player playerToCreate)
        {
            if (playerToCreate == null)
            {
                throw new ArgumentNullException("playerToCreate");
            }

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
            var criteria = new FindByIdCriteria { Id = id };
            return _getPlayerByIdQuery.Execute(criteria);
        }

        /// <summary>
        /// Edit player.
        /// </summary>
        /// <param name="playerToEdit">Player to edit.</param>
        public void Edit(Player playerToEdit)
        {
            // Check if player is captain of team and teamId is null or changed
            ////Team ledTeam = GetPlayerLedTeam(playerToEdit.Id);
            ////if (ledTeam != null &&
            ////    (playerToEdit.TeamId == null || playerToEdit.TeamId != ledTeam.Id))
            ////{
            ////    var ex = new ValidationException(ServiceResources.ExceptionMessages.PlayerIsCaptainOfAnotherTeam);
            ////    ex.Data[Domain.Constants.ExceptionManagement.ENTITY_ID_KEY] = ledTeam.Id;
            ////    throw ex;
            ////}

            ////if (playerToEdit.TeamId != null
            ////    && this._teamRepository.FindWhere(t => t.Id == playerToEdit.TeamId).SingleOrDefault() == null)
            ////{
            ////    throw new MissingEntityException(ServiceResources.ExceptionMessages.TeamNotFound, playerToEdit.TeamId);
            ////}

            try
            {
                _playerRepository.Update(playerToEdit);
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, ex);
            }

            _playerRepository.UnitOfWork.Commit();
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

            return this.GetTeamById(player.TeamId.GetValueOrDefault());
        }

        private Team GetPlayerLedTeam(int playerId)
        {
            var criteria = new FindByCaptainIdCriteria { CaptainId = playerId };
            return _getTeamByCaptainQuery.Execute(criteria);
        }

        private Team GetTeamById(int id)
        {
            var criteria = new FindByIdCriteria { Id = id };
            return _getTeamByIdQuery.Execute(criteria);
        }
    }
}
