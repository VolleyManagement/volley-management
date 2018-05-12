namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Data.Queries.Team;
    using Domain.PlayersAggregate;
    using Domain.RolesAggregate;
    using Domain.TeamsAggregate;

#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    /// <summary>
    /// Defines PlayerService
    /// </summary>
    public class PlayerService : IPlayerService
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IQuery<Player, FindByIdCriteria> _getPlayerByIdQuery;
        private readonly IQuery<int, FindByPlayerCriteria> _getPlayerTeamQuery;
        private readonly IQuery<ICollection<FreePlayerDto>, EmptyCriteria> _getFreePlayers;
        private readonly IQuery<Team, FindByIdCriteria> _getTeamByIdQuery;
        private readonly IQuery<IQueryable<Player>, GetAllCriteria> _getAllPlayersQuery;
        private readonly IQuery<Team, FindByCaptainIdCriteria> _getTeamByCaptainQuery;
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerService"/> class.
        /// </summary>
        /// <param name="playerRepository"> The player repository </param>
        /// <param name="getTeamByIdQuery"> Get By ID query for Teams</param>
        /// <param name="getPlayerByIdQuery"> Get By ID query for Players</param>
        /// <param name="getPlayerTeamQuery"></param>
        /// <param name="getFreePlayers"></param>
        /// <param name="getAllPlayersQuery"> Get All players query</param>
        /// <param name="getTeamByCaptainQuery">Get Player by Captain query</param>
        /// <param name="authService">Authorization service</param>
        public PlayerService(
            IPlayerRepository playerRepository,
            IQuery<Team, FindByIdCriteria> getTeamByIdQuery,
            IQuery<Player, FindByIdCriteria> getPlayerByIdQuery,
            IQuery<int, FindByPlayerCriteria> getPlayerTeamQuery,
            IQuery<ICollection<FreePlayerDto>, EmptyCriteria> getFreePlayers,
            IQuery<IQueryable<Player>, GetAllCriteria> getAllPlayersQuery,
            IQuery<Team, FindByCaptainIdCriteria> getTeamByCaptainQuery,
            IAuthorizationService authService)
        {
            _playerRepository = playerRepository;
            _getTeamByIdQuery = getTeamByIdQuery;
            _getFreePlayers = getFreePlayers;
            _getPlayerByIdQuery = getPlayerByIdQuery;
            _getAllPlayersQuery = getAllPlayersQuery;
            _getPlayerTeamQuery = getPlayerTeamQuery;
            _getTeamByCaptainQuery = getTeamByCaptainQuery;
            _authService = authService;
        }

        /// <summary>
        /// Method to get all players.
        /// </summary>
        /// <returns>All players.</returns>
        public IQueryable<Player> Get()
        {
            return _getAllPlayersQuery.Execute(new GetAllCriteria());
        }

        public ICollection<FreePlayerDto> GetFreePlayerDto()
        {
            return _getFreePlayers.Execute(new EmptyCriteria());
        }

        /// <summary>
        /// Create a new player.
        /// </summary>
        /// <param name="playerToCreate">A Player to create.</param>
        public Player Create(CreatePlayerDto playerToCreate)
        {
            _authService.CheckAccess(AuthOperations.Players.Create);
            if (playerToCreate == null)
            {
                throw new ArgumentNullException("playerToCreate");
            }

            return _playerRepository.Add(playerToCreate);
        }

        /// <summary>
        /// Create new players.
        /// </summary>
        /// <param name="fullNames">FullNmaes of players.</param>
        public ICollection<Player> CreateBulk(IEnumerable<string> fullNames)
        {
            _authService.CheckAccess(AuthOperations.Players.Create);

            var players = fullNames.Select(p => _playerRepository.Add(
                new CreatePlayerDto {
                    FirstName = p.Substring(0, p.LastIndexOf(' ')),
                    LastName = p.Substring(p.LastIndexOf(' ') + 1)
                })).ToList();

            return players;
        }

        /// <summary>
        /// Create new players.
        /// </summary>
        /// <param name="playersToCreate">New players.</param>
        public ICollection<Player> CreateBulk(ICollection<CreatePlayerDto> playersToCreate)
        {
            _authService.CheckAccess(AuthOperations.Players.Create);

            var players = playersToCreate.Select(p => _playerRepository.Add(p)).ToList();

            return players;
        }

        /// <summary>
        /// Finds a Player by id.
        /// </summary>
        /// <param name="id">id for search.</param>
        /// <returns>A found Player.</returns>
        public Player Get(int id)
        {
            return _getPlayerByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        /// <summary>
        /// Edit player.
        /// </summary>
        /// <param name="playerToEdit">Player to edit.</param>
        public void Edit(Player playerToEdit)
        {
            _authService.CheckAccess(AuthOperations.Players.Edit);
            try
            {
                _playerRepository.Update(playerToEdit);
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.PlayerNotFound, ex);
            }
        }

        /// <summary>
        /// Delete player by id.
        /// </summary>
        /// <param name="id">The id of player to delete.</param>
        public void Delete(int id)
        {
            _authService.CheckAccess(AuthOperations.Players.Delete);
            var playerTeam = GetPlayerLedTeam(id);

            if (playerTeam != null)
            {
                var ex = new ValidationException(ServiceResources.ExceptionMessages.PlayerIsCaptainOfAnotherTeam);
                ex.Data[Domain.Constants.ExceptionManagement.ENTITY_ID_KEY] = playerTeam.Id;
                throw ex;
            }

            try
            {
                _playerRepository.Remove(id);
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
            var teamId = _getPlayerTeamQuery.Execute(new FindByPlayerCriteria { Id = player.Id });

            if (teamId == default(int))
            {
                return null;
            }

            return GetTeamById(teamId);
        }

        private Team GetPlayerLedTeam(int playerId)
        {
            return _getTeamByCaptainQuery.Execute(new FindByCaptainIdCriteria { CaptainId = playerId });
        }

        private Team GetTeamById(int id)
        {
            return _getTeamByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

    }
}
