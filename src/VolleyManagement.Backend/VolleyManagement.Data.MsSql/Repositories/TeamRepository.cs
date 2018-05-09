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
            var dalToRemove = new TeamEntity { Id = teamId.Id };
            _dalTeams.Attach(dalToRemove);
            _dalTeams.Remove(dalToRemove);
            _unitOfWork.Commit();
        }

        #region private

        private void AddAndRemovePlayersInTeam(Team teamToUpdated, TeamEntity entityToUpdate)
        {
            var playersInTeamToUpdate = teamToUpdated.Roster.Select(p => p.Id);
            var playersInEntityToUpdate = entityToUpdate.Players.Select(p => p.Id);

            var playersToAdd = playersInTeamToUpdate.Except(playersInEntityToUpdate);
            var playersToRemove = playersInEntityToUpdate.Except(playersInTeamToUpdate);

            var playersIds = playersToAdd.Union(playersToRemove).ToList();

            var playerEntities = _unitOfWork.Context.Players
                .Where(p => playersIds.Contains(p.Id));

            foreach (var player in playerEntities)    
            {
                if (playersToAdd.Contains(player.Id))
                {
                    player.TeamId = entityToUpdate.Id;
                }

                if (playersToRemove.Contains(player.Id))
                {
                    player.TeamId = null;
                }
            }
        }

        #endregion
    }
}
