using System.Collections;
using System.Collections.Generic;

namespace VolleyManagement.Data.MsSql.Repositories
{
    using System.Data.Entity;
    using System.Linq;
    using Contracts;
    using Domain.TeamsAggregate;
    using Entities;
    using Exceptions;
    using Mappers;
    using Specifications;

    /// <summary>
    /// Defines implementation of the ITeamRepository contract.
    /// </summary>
    internal class TeamRepository : ITeamRepository
    {
        private static readonly TeamStorageSpecification _dbStorageSpecification
            = new TeamStorageSpecification();

        private readonly DbSet<TeamEntity> _dalTeams;

        private readonly VolleyUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public TeamRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalTeams = _unitOfWork.Context.Teams;
        }

        public Team Add(CreateTeamDto teamToCreate)
        {
            var newTeam = new TeamEntity {
                Name = teamToCreate.Name,
                Coach = teamToCreate.Coach,
                CaptainId = teamToCreate.Captain.Id,
                Achievements = teamToCreate.Achievements
            };

            if (!_dbStorageSpecification.IsSatisfiedBy(newTeam))
            {
                throw new InvalidEntityException();
            }

            _dalTeams.Add(newTeam);
            _unitOfWork.Commit();

            return new Team(
                newTeam.Id,
                newTeam.Name,
                newTeam.Coach,
                newTeam.Achievements,
                new PlayerId(newTeam.CaptainId),
                teamToCreate.Roster.ToList()
            );
        }

        /// <summary>
        /// Updates specified team.
        /// </summary>
        /// <param name="updatedEntity">Updated team.</param>
        public void Update(Team updatedEntity)
        {
            var teamToUpdate = _dalTeams.SingleOrDefault(t => t.Id == updatedEntity.Id);

            if (teamToUpdate == null)
            {
                throw new ConcurrencyException();
            }

            DomainToDal.Map(teamToUpdate, updatedEntity);

            var playersToAdd = updatedEntity.Roster.Select(p => p.Id)
                .Except(teamToUpdate.Players.Select(p => p.Id));

            var playersToRemove = teamToUpdate.Players.Select(p => p.Id)
                .Except(updatedEntity.Roster.Select(p => p.Id));

            AddPlayers(teamToUpdate, playersToAdd);
            RemovePlayers(teamToUpdate, playersToRemove);


            _unitOfWork.Commit();
        }

        /// <summary>
        /// Removes team by id.
        /// </summary>
        /// <param name="teamId">The id of team to remove.</param>
        public void Remove(TeamId teamId)
        {
            var dalToRemove = new TeamEntity { Id = teamId.Id };
            _dalTeams.Attach(dalToRemove);
            _dalTeams.Remove(dalToRemove);
            _unitOfWork.Commit();
        }

        private static void RemovePlayers(TeamEntity team, IEnumerable<int> playersToRemove)
        {
            foreach (var playerId in playersToRemove.ToList())
            {
                team.Players.Remove(team.Players.First(p => p.Id == playerId));
            }
        }

        private void AddPlayers(TeamEntity team, IEnumerable<int> playersToAdd)
        {
            var entitiesToAdd = _unitOfWork.Context.Players.Where(p => playersToAdd.Contains(p.Id));
            foreach (var player in entitiesToAdd)
            {
                team.Players.Add(player);
            }
        }
    }
}
