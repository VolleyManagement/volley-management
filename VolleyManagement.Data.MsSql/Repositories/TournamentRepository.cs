namespace VolleyManagement.Data.MsSql.Repositories
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Contracts;
    using Crosscutting.Contracts.Specifications;
    using Domain.TournamentsAggregate;
    using Entities;
    using Exceptions;
    using Mappers;
    using Specifications;

    /// <summary>
    /// Defines implementation of the ITournamentRepository contract.
    /// </summary>
    internal class TournamentRepository : ITournamentRepository
    {
        private readonly DbSet<TournamentEntity> _dalTournaments;
        private readonly DbSet<TeamEntity> _dalTeams;
        private readonly DbSet<DivisionEntity> _dalDivisions;
        private readonly DbSet<GroupEntity> _dalGroups;
        private readonly VolleyUnitOfWork _unitOfWork;
        private readonly ISpecification<TournamentEntity> _dbStorageSpecification = new TournamentsStorageSpecification();

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public TournamentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalTournaments = _unitOfWork.Context.Tournaments;
            _dalTeams = _unitOfWork.Context.Teams;
            _dalDivisions = _unitOfWork.Context.Divisions;
            _dalGroups = _unitOfWork.Context.Groups;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Adds new tournament.
        /// </summary>
        /// <param name="newEntity">The tournament for adding.</param>
        public void Add(Tournament newEntity)
        {
            var tournament = new TournamentEntity();
            DomainToDal.Map(tournament, newEntity);

            if (!_dbStorageSpecification.IsSatisfiedBy(tournament))
            {
                throw new InvalidEntityException();
            }

            _dalTournaments.Add(tournament);
            _unitOfWork.Commit();
            MapIdentifiers(newEntity, tournament);
        }

        /// <summary>
        /// Updates specified tournament.
        /// </summary>
        /// <param name="updatedEntity">Updated tournament.</param>
        public void Update(Tournament updatedEntity)
        {
            var tournamentToUpdate = _dalTournaments.Single(t => t.Id == updatedEntity.Id);
            UpdateDivisions(tournamentToUpdate.Divisions, updatedEntity.Divisions);
            DomainToDal.Map(tournamentToUpdate, updatedEntity);
        }

        /// <summary>
        /// Removes tournament by id.
        /// </summary>
        /// <param name="id">The id of tournament to remove.</param>
        public void Remove(int id)
        {
            var dalToRemove = _dalTournaments.Single(t => t.Id == id);
            _dalTournaments.Remove(dalToRemove);
        }

        /// <summary>
        /// Add team to the tournament
        /// </summary>
        /// <param name="teamId">Team id to add</param>
        /// <param name="groupId">Group id to add</param>
        public void AddTeamToTournament(int teamId, int groupId)
        {
            var group = from t in _dalTournaments
                        join d in _dalDivisions on t.Id equals d.TournamentId
                        join g in _dalGroups on d.Id equals g.DivisionId
                        where g.Id == groupId
                        select g;
            group.First().Teams.Add(_dalTeams.Find(teamId));
        }

        /// <summary>
        /// Removes team from the tournament
        /// </summary>
        /// <param name="teamId">Team to be removed id</param>
        /// <param name="tournamentId">Tournament id from which remove team</param>
        public void RemoveTeamFromTournament(int teamId, int tournamentId)
        {
            var tournamentEntity = _unitOfWork.Context.Tournaments.Find(tournamentId);
            var teamEntity = tournamentEntity?.Divisions
                                             .SelectMany(d => d.Groups)
                                             .SelectMany(g => g.Teams)
                                             .SingleOrDefault(t => t.Id == teamId);

            if (teamEntity == null)
            {
                throw new ConcurrencyException();
            }

            foreach (var group in teamEntity.Groups)
            {
                if (group.Division.TournamentId == tournamentId)
                {
                    group.Teams.Remove(teamEntity);
                    break;
                }
            }
        }

        /// <summary>
        /// Removes group
        /// </summary>
        /// <param name="groupId">Group to be removed id</param>
        public void RemoveGroup(int groupId)
        {
            var groupEntity = _unitOfWork.Context.Groups.Find(groupId);
            if (groupEntity == null)
            {
                throw new ConcurrencyException();
            }

            _unitOfWork.Context.Groups.Remove(groupEntity);
        }

        /// <summary>
        /// Remove division
        /// </summary>
        /// <param name="divisionId">Division to be removed id</param>
        public void RemoveDivision(int divisionId)
        {
            var divisionEntity = _unitOfWork.Context.Divisions.Find(divisionId);
            if (divisionEntity == null)
            {
                throw new ConcurrencyException();
            }

            foreach (var group in divisionEntity.Groups.ToList())
            {
                RemoveGroup(group.Id);
            }

            _unitOfWork.Context.Divisions.Remove(divisionEntity);
        }

        private void UpdateDivisions(List<DivisionEntity> old, List<Division> changed)
        {
            foreach (var item in old.ToList())
            {
                var updatedDivision = from element in changed
                                      where item.Id == element.Id
                                      select element;
                if (updatedDivision.Count() != 0)
                {
                    UpdateGroups(item.Groups, updatedDivision.First().Groups);
                }
                else
                {
                    RemoveDivision(item.Id);
                }
            }
        }

        private void UpdateGroups(List<GroupEntity> old, List<Group> changed)
        {
            foreach (var item in old.ToList())
            {
                if (!Enumerable.Any(from element in changed
                                   where item.Id == element.Id
                                   select element))
                {
                    RemoveGroup(item.Id);
                }
            }
        }

        private void MapIdentifiers(Tournament to, TournamentEntity from)
        {
            to.Id = from.Id;

            foreach (DivisionEntity divisionEntity in from.Divisions)
            {
                Division divisionDomain = to.Divisions.Where(d => d.Name == divisionEntity.Name).First();
                divisionDomain.Id = divisionEntity.Id;
                divisionDomain.TournamentId = divisionEntity.TournamentId;

                foreach (GroupEntity groupEntity in divisionEntity.Groups)
                {
                    Group groupDomain = divisionDomain.Groups.Where(g => g.Name == groupEntity.Name).First();
                    groupDomain.Id = groupEntity.Id;
                    groupDomain.DivisionId = divisionEntity.Id;
                }
            }
        }
    }
}
