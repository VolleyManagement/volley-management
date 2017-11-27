namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TeamsAggregate;
    using PlayerService;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TeamServiceTestFixture
    {
        /// <summary>
        /// Holds collection of teams
        /// </summary>
        private List<Team> _teams = new List<Team>();

        /// <summary>
        /// Holds collection of teams collections
        /// </summary>
        private List<List<Team>> _teamsByDivisions = new List<List<Team>>();

        /// <summary>
        /// Holds collection of teams
        /// </summary>
        private PlayerBuilder _playerBuilder;

        /// <summary>
        /// Return test collection of teams
        /// </summary>
        /// <returns>Builder object with collection of teams</returns>
        public TeamServiceTestFixture TestTeams()
        {
            _playerBuilder = new PlayerBuilder();

            _teams.Add(new Team()
            {
                Id = 1,
                Name = "TeamNameA",
                CaptainId = 1,
                Coach = "TeamCoachA",
                Achievements = "TeamAchievementsA"
            });
            _teams.Add(new Team()
            {
                Id = 2,
                Name = "TeamNameB",
                CaptainId = 2,
                Coach = "TeamCoachB",
                Achievements = "TeamAchievementsB"
            });
            _teams.Add(
            new Team()
            {
                Id = 3,
                Name = "TeamNameC",
                CaptainId = 3,
                Coach = "TeamCoachC",
                Achievements = "TeamAchievementsC"
            });
            return this;
        }

        /// <summary>
        /// Return test collection of teams grouped by divisions
        /// </summary>
        /// <returns>Builder object with collection of teams collection</returns>
        public TeamServiceTestFixture TestTeamsByDivisions()
        {
            _playerBuilder = new PlayerBuilder();

            _teamsByDivisions.Add(new List<Team>()
            {
                new Team()
                {
                    Id = 1,
                    Name = "TeamNameA",
                    CaptainId = 1,
                    Coach = "TeamCoachA",
                    Achievements = "TeamAchievementsA"
                },
                new Team()
                {
                    Id = 2,
                    Name = "TeamNameB",
                    CaptainId = 2,
                    Coach = "TeamCoachB",
                    Achievements = "TeamAchievementsB"
                },
                new Team()
                {
                    Id = 3,
                    Name = "TeamNameC",
                    CaptainId = 3,
                    Coach = "TeamCoachC",
                    Achievements = "TeamAchievementsC"
                },
            });
            _teamsByDivisions.Add(new List<Team>()
            {
                new Team()
                {
                    Id = 4,
                    Name = "TeamNameD",
                    CaptainId = 4,
                    Coach = "TeamCoachD",
                    Achievements = "TeamAchievementsD"
                },
                new Team()
                {
                    Id = 5,
                    Name = "TeamNameE",
                    CaptainId = 5,
                    Coach = "TeamCoachE",
                    Achievements = "TeamAchievementsE"
                },
                new Team()
                {
                    Id = 6,
                    Name = "TeamNameF",
                    CaptainId = 6,
                    Coach = "TeamCoachF",
                    Achievements = "TeamAchievementsF"
                }
            });
            return this;
        }

        /// <summary>
        /// Add player to collection.
        /// </summary>
        /// <param name="newTeam">Team to add.</param>
        /// <returns>Builder object with collection of teams.</returns>
        public TeamServiceTestFixture AddTeam(Team newTeam)
        {
            _teams.Add(newTeam);
            return this;
        }

        /// <summary>
        /// Add collection of teams to collection.
        /// </summary>
        /// <param name="newTeams">Teams to add.</param>
        /// <returns>Builder object with collection of teams colections.</returns>
        public TeamServiceTestFixture AddTeams(List<Team> newTeams)
        {
            _teamsByDivisions.Add(newTeams);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Team collection</returns>
        public List<Team> Build()
        {
            return _teams;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Collection of teams collections</returns>
        public List<List<Team>> BuildWithDivisions()
        {
            return _teamsByDivisions;
        }
    }
}
