namespace VolleyManagement.Dal.MsSql.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Text;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Dal.MsSql.Mappers;
    using Dal = VolleyManagement.Dal.MsSql;
    using Domain = VolleyManagement.Domain.Players;

    /// <summary>
    /// Defines implementation of the IPlayerRepository contract.
    /// </summary>
    internal class PlayerRepository : IPlayerRepository
    {
        private const int START_DATABASE_ID_VALUE = 0;

        /// <summary>
        /// Holds object set of DAL users.
        /// </summary>
        private readonly ObjectSet<Dal.Player> _dalPlayers;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        
        /// <summary>
        /// Holds TeamRepository instance
        /// Initialized on first using
        /// </summary>
        private TeamRepository _teamRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PlayerRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dalPlayers = unitOfWork.Context.CreateObjectSet<Dal.Player>();
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Gets all players.
        /// </summary>
        /// <returns>Collection of domain players.</returns>
        public IQueryable<Domain.Player> Find()
        {
            return _dalPlayers.Select(p => new Domain.Player
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                BirthYear = p.BirthYear,
                Height = p.Height,
                Weight = p.Weight
            });
        }

        /// <summary>
        /// Gets specified collection of players.
        /// </summary>
        /// <param name="predicate">Condition to find players.</param>
        /// <returns>Collection of domain players.</returns>
        public IQueryable<Domain.Player> FindWhere(System.Linq.Expressions.Expression<Func<Domain.Player, bool>> predicate)
        {
            return Find().Where(predicate);
        }

        /// <summary>
        /// Adds new player.
        /// </summary>
        /// <param name="newEntity">The player for adding.</param>
        public void Add(Domain.Player newEntity)
        {
            Dal.Player newPlayer = DomainToDal.Map(newEntity);
            _dalPlayers.AddObject(newPlayer);
            _unitOfWork.Commit();
            newEntity.Id = newPlayer.Id;
        }

        /// <summary>
        /// Updates specified player.
        /// </summary>
        /// <param name="oldEntity">The player to update.</param>
        public void Update(Domain.Player oldEntity)
        {
            if (oldEntity.Id < START_DATABASE_ID_VALUE)
            {
                var exc = new InvalidKeyValueException("Id is invalid for this Entity");
                exc.Data[Constants.ENTITY_ID_KEY] = oldEntity.Id;
                throw exc;
            }

            Player playerToUpdate;
            try
            {
                playerToUpdate = _dalPlayers.Where(t => t.Id == oldEntity.Id).Single();
            }
            catch (InvalidOperationException e)
            {
                var exc = new InvalidKeyValueException("Entity with request Id does not exist", e);
                exc.Data[Constants.ENTITY_ID_KEY] = oldEntity.Id;
                throw exc;
            }

            if (oldEntity.Team != null && !TeamExist(oldEntity.Team.Id))
            {
                throw new InvalidKeyValueException("Team with requested Id does not exist", oldEntity.Team.Id);
            }

            playerToUpdate.Id = oldEntity.Id;
            playerToUpdate.FirstName = oldEntity.FirstName;
            playerToUpdate.LastName = oldEntity.LastName;
            playerToUpdate.BirthYear = oldEntity.BirthYear;
            playerToUpdate.Height = oldEntity.Height;
            playerToUpdate.Weight = oldEntity.Weight;

            UpdateTeamId(playerToUpdate, oldEntity.Team);
        }

        /// <summary>
        /// Removes player by id.
        /// </summary>
        /// <param name="id">The id of player to remove.</param>
        public void Remove(int id)
        {
            var dalToRemove = new Dal.Player { Id = id };
            _dalPlayers.Attach(dalToRemove);
            _dalPlayers.DeleteObject(dalToRemove);
        }

        private TeamRepository GetTeamRepository()
        {
            if (_teamRepository == null)
            {
                _teamRepository = new TeamRepository(new VolleyUnitOfWork());
            }

            return _teamRepository;
        }

        private bool TeamExist(int id)
        {
            var teamRepository = GetTeamRepository();
            return teamRepository.Find().Count(t => t.Id == id) > 0;
        }

        private void UpdateTeamId(Player dalPlayer, VolleyManagement.Domain.Teams.Team team)
        {
            if (team == null)
            {
                dalPlayer.TeamId = null;
            }
            else
            {
                dalPlayer.TeamId = team.Id;
            }
        }
    }
}
