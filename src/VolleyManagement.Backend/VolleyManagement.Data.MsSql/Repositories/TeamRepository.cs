using System.Collections;
using System.Collections.Generic;
using System.Data;

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
                Achievements = teamToCreate.Achievements,
                Players = new List<PlayerEntity>()
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
            var teamToUpdate = _dalTeams.Find(updatedEntity.Id);

            DomainToDal.Map(teamToUpdate, updatedEntity);

            AddAndRemovePlayersInTeam(updatedEntity, teamToUpdate);

            _unitOfWork.Commit();
        }

        /// <summary>
        /// Removes team by id.
        /// </summary>
        /// <param name="teamId">The id of team to remove.</param>
        public void Remove(TeamId teamId)
        {
            var teamtodelete = _dalTeams.Find(teamId.Id);
            if (teamtodelete != null)
            {
                _dalTeams.Remove(teamtodelete);
                _unitOfWork.Commit();
            }
            else
            {
                throw new DBConcurrencyException();
            }
        }

        #region private

        private void AddAndRemovePlayersInTeam(Team updatedEntity, TeamEntity teamToUpdate)
        {
            var playersToAdd = updatedEntity.Roster.Select(p => p.Id)
                .Except(teamToUpdate.Players.Select(p => p.Id));
            var playersToRemove = teamToUpdate.Players.Select(p => p.Id)
                .Except(updatedEntity.Roster.Select(p => p.Id));
            var playersIds = playersToAdd.Union(playersToRemove).ToList();

            var playerEntities = _unitOfWork.Context.Players
                .Where(p => playersIds.Contains(p.Id));

            foreach (var playerId in playersToAdd)
            {
                playerEntities.Single(p => p.Id == playerId).TeamId = teamToUpdate.Id;
            }

            foreach (var playerId in playersToRemove)
            {
                playerEntities.Single(p => p.Id == playerId).TeamId = null;
            }
        }

        #endregion
    }
}
