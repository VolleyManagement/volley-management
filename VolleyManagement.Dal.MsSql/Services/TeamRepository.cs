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
    using Domain = VolleyManagement.Domain.Teams;
    using DomainPlayer = VolleyManagement.Domain.Players.Player;

    /// <summary>
    /// Defines implementation of the ITeamRepository contract.
    /// </summary>
    internal class TeamRepository : ITeamRepository
    {
        private const int START_DATABASE_ID_VALUE = 0;

        /// <summary>
        /// Holds object set of DAL users.
        /// </summary>
        private readonly ObjectSet<Dal.Team> _dalTeams;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Holds PlayerRepository instance
        /// Initialized on first using
        /// </summary>
        private PlayerRepository _playerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public TeamRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dalTeams = unitOfWork.Context.CreateObjectSet<Dal.Team>();
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Gets all teams.
        /// </summary>
        /// <returns>Collection of domain teams.</returns>
        public IQueryable<Domain.Team> Find()
        {
            return _dalTeams.Select(t => new Domain.Team
            {
                Id = t.Id,
                Name = t.Name

                // TODO: Create DalToDomain mapper class
            });
        }

        /// <summary>
        /// Gets specified collection of teams.
        /// </summary>
        /// <param name="predicate">Condition to find teams.</param>
        /// <returns>Collection of domain teams.</returns>
        public IQueryable<Domain.Team> FindWhere(System.Linq.Expressions.Expression<Func<Domain.Team, bool>> predicate)
        {
            return Find().Where(predicate);
        }

        /// <summary>
        /// Adds new team.
        /// </summary>
        /// <param name="newEntity">The team for adding.</param>
        public void Add(Domain.Team newEntity)
        {
            if (!PlayerExist(newEntity.Captain.Id))
            {
                throw new InvalidKeyValueException("Captain with requested Id does not exist", newEntity.Captain.Id);
            }

            foreach (var domainPlayer in newEntity.Roster)
            {
                if (!PlayerExist(domainPlayer.Id))
                {
                    throw new InvalidKeyValueException("Roster player with requested Id does not exist", domainPlayer.Id);
                }
            }

            Dal.Team newTeam = DomainToDal.Map(newEntity);
            _dalTeams.AddObject(newTeam);
            _unitOfWork.Commit();

            newEntity.Id = newTeam.Id;

            UpdateTeamForRosterPlayers(newEntity.Roster, newEntity);
        }

        /// <summary>
        /// Removes team by id.
        /// </summary>
        /// <param name="id">The id of team to remove.</param>
        public void Remove(int id)
        {
            Domain.Team domainTeam;
            try
            {
                domainTeam = FindWhere(t => t.Id == id).Single();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidKeyValueException("Team with requested Id does not exist", id, ex);
            }

            UpdateTeamForRosterPlayers(domainTeam.Roster, null);

            var dalToRemove = new Dal.Team { Id = id };
            _dalTeams.Attach(dalToRemove);
            _dalTeams.DeleteObject(dalToRemove);
        }

        /// <summary>
        /// Updates specified team.
        /// </summary>
        /// <param name="oldEntity">The team to update</param>
        public void Update(Domain.Team oldEntity)
        {
            throw new NotImplementedException();
        }

        private bool PlayerExist(int id)
        {
            var playerRepository = GetPlayerRepository();
            return playerRepository.Find().Count(p => p.Id == id) > 0;
        }

        private PlayerRepository GetPlayerRepository()
        {
            if (_playerRepository == null)
            {
                _playerRepository = new PlayerRepository(new VolleyUnitOfWork());
            }

            return _playerRepository;
        }

        private void UpdateTeamForRosterPlayers(IEnumerable<DomainPlayer> players, Domain.Team domainTeam)
        {
            var playerRepository = GetPlayerRepository();
            foreach (var domainPlayer in players)
            {
                _playerRepository.UpdateTeamId(domainPlayer.Id, domainTeam);
            }
        }
    }
}
