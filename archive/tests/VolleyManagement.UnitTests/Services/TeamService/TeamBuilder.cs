using System.Linq;

namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TeamsAggregate;

    /// <summary>
    /// Team domain object builder
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TeamBuilder
    {
        /// <summary>
        /// Holds test player instance
        /// </summary>
        private Team _team;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamBuilder"/> class
        /// </summary>
        public TeamBuilder()
        {
            _team = new Team(1,
                "TeamNameA",
                "TeamCoachA",
                "TeamAchievementsA",
                new PlayerId(1),
                new List<PlayerId>());
        }

        /// <summary>
        /// Sets team test Id
        /// </summary>
        /// <param name="id">Test team Id</param>
        /// <returns>Team builder object</returns>
        public TeamBuilder WithId(int id)
        {
            _team = new Team(id,
                _team.Name,
                _team.Coach,
                _team.Achievements,
                _team.Captain,
                _team.Roster);
            return this;
        }

        /// <summary>
        /// Sets team test first name
        /// </summary>
        /// <param name="name">Test team name</param>
        /// <returns>Team builder object</returns>
        public TeamBuilder WithName(string name)
        {
            _team.Name = name;
            return this;
        }

        /// <summary>
        /// Sets team test coach
        /// </summary>
        /// <param name="coach">Test team coach</param>
        /// <returns>Team builder object</returns>
        public TeamBuilder WithCoach(string coach)
        {
            _team.Coach = coach;
            return this;
        }

        /// <summary>
        /// Sets team test achievements
        /// </summary>
        /// <param name="achievements">Test team achievements</param>
        /// <returns>Team builder object</returns>
        public TeamBuilder WithAchievements(string achievements)
        {
            _team.Achievements = achievements;
            return this;
        }

        /// <summary>
        /// Sets team test captain
        /// </summary>
        /// <param name="captainId">Test team captain</param>
        /// <returns>Team builder object</returns>
        public TeamBuilder WithCaptain(PlayerId captainId)
        {
            _team.SetCaptain(captainId);
            return this;
        }

        /// <summary>
        /// Sets test team roster
        /// </summary>
        /// <param name="roster">Test team roster</param>
        /// <returns>Team builder object</returns>
        public TeamBuilder WithRoster(IEnumerable<PlayerId> roster)
        {
            _team = new Team(_team.Id,
                _team.Name,
                _team.Coach,
                _team.Achievements,
                _team.Captain,
                roster);
            return this;
        }

        /// <summary>
        /// Add player in the team
        /// </summary>
        /// <param name="playerId">Test team player</param>
        /// <returns>Team builder object</returns>
        public TeamBuilder WithPlayer(PlayerId playerId)
        {
            _team.Roster.ToList().Add(playerId);
            return this;
        }

        /// <summary>
        /// Builds test team
        /// </summary>
        /// <returns>Test team</returns>
        public Team Build()
        {
            return _team;
        }
    }
}
