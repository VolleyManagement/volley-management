namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.TeamsAggregate;

    /// <summary>
    /// Team domain object builder
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CreateTeamDtoBuilder
    {
        /// <summary>
        /// Holds test team instance
        /// </summary>
        private CreateTeamDto _team;
        
        public CreateTeamDtoBuilder()
        {
            _team = new CreateTeamDto {
                Name = "Name",
                Coach = "Coach",
                Achievements = "Achievements",
                Captain = new PlayerId(1),
                Roster = new List<PlayerId>()
            };
        }
        
        public CreateTeamDtoBuilder WithName(string name)
        {
            _team.Name = name;
            return this;
        }
        
        public CreateTeamDtoBuilder WithCoach(string coach)
        {
            _team.Coach = coach;
            return this;
        }
        
        public CreateTeamDtoBuilder WithAchievements(string achievements)
        {
            _team.Achievements = achievements;
            return this;
        }
        
        public CreateTeamDtoBuilder WithCaptain(PlayerId captain)
        {
            _team.Captain = captain;
            return this;
        }

        public CreateTeamDtoBuilder WithRoster(IEnumerable<PlayerId> roster)
        {
            _team.Roster = roster;
            return this;
        }

        /// <summary>
        /// Builds test team
        /// </summary>
        /// <returns>Test team</returns>
        public CreateTeamDto Build()
        {
            return _team;
        }
    }
}
