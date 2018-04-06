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
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Removes team by id.
        /// </summary>
        /// <param name="id">The id of team to remove.</param>
        public void Remove(int id)
        {
            var dalToRemove = new TeamEntity { Id = id };
            _dalTeams.Attach(dalToRemove);
            _dalTeams.Remove(dalToRemove);
            _unitOfWork.Commit();
        }
    }
}
