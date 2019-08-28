﻿namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TeamsAggregate;

    [ExcludeFromCodeCoverage]
    public class TeamInTournamentTestFixture
    {
        private readonly List<TeamTournamentDto> _teams = new List<TeamTournamentDto>();

        public TeamInTournamentTestFixture WithTeamsInSingleDivisionSingleGroup()
        {
            AddTeam(1, "A");
            AddTeam(2, "B");
            AddTeam(3, "C");
            return this;
        }

        public TeamInTournamentTestFixture WithTeamsInTwoDivisionTwoGroups()
        {
            // Division 1
            // Group 1
            AddTeam(teamId: 1, teamName: "A", divisionId: 1, divisionName: "DivisionNameA", groupId: 1);
            AddTeam(teamId: 2, teamName: "B", divisionId: 1, divisionName: "DivisionNameA", groupId: 1);
            AddTeam(teamId: 3, teamName: "C", divisionId: 1, divisionName: "DivisionNameA", groupId: 1);

            // Group 2
            AddTeam(teamId: 4, teamName: "D", divisionId: 1, divisionName: "DivisionNameA", groupId: 2);
            AddTeam(teamId: 5, teamName: "E", divisionId: 1, divisionName: "DivisionNameA", groupId: 2);
            AddTeam(teamId: 6, teamName: "F", divisionId: 1, divisionName: "DivisionNameA", groupId: 2);

            // Division 2
            // Group 1
            AddTeam(teamId: 7, teamName: "G", divisionId: 2, divisionName: "DivisionNameB", groupId: 3);
            AddTeam(teamId: 8, teamName: "H", divisionId: 2, divisionName: "DivisionNameB", groupId: 3);
            AddTeam(teamId: 9, teamName: "I", divisionId: 2, divisionName: "DivisionNameB", groupId: 3);

            // Group 2
            AddTeam(teamId: 10, teamName: "J", divisionId: 2, divisionName: "DivisionNameB", groupId: 4);
            AddTeam(teamId: 11, teamName: "K", divisionId: 2, divisionName: "DivisionNameB", groupId: 4);
            AddTeam(teamId: 12, teamName: "L", divisionId: 2, divisionName: "DivisionNameB", groupId: 4);

            return this;
        }

        public TeamInTournamentTestFixture WithUnorderedTeams()
        {
            AddTeam(teamId: 4, teamName: "D", divisionId: 1, divisionName: "DivisionNameA", groupId: 1, groupName: "GroupNameC");
            AddTeam(teamId: 5, teamName: "E", divisionId: 1, divisionName: "DivisionNameA", groupId: 2, groupName: "GroupNameA");
            AddTeam(teamId: 6, teamName: "F", divisionId: 1, divisionName: "DivisionNameA", groupId: 3, groupName: "GroupNameD");

            AddTeam(teamId: 11, teamName: "J", divisionId: 3, divisionName: "DivisionNameC", groupId: 7, groupName: "GroupNameC");
            AddTeam(teamId: 12, teamName: "K", divisionId: 3, divisionName: "DivisionNameC", groupId: 8, groupName: "GroupNameB");
            AddTeam(teamId: 13, teamName: "L", divisionId: 3, divisionName: "DivisionNameC", groupId: 9, groupName: "GroupNameA");

            AddTeam(teamId: 7, teamName: "G", divisionId: 2, divisionName: "DivisionNameB", groupId: 4, groupName: "GroupNameA");
            AddTeam(teamId: 8, teamName: "H", divisionId: 2, divisionName: "DivisionNameB", groupId: 5, groupName: "GroupNameC");
            AddTeam(teamId: 9, teamName: "I", divisionId: 2, divisionName: "DivisionNameB", groupId: 6, groupName: "GroupNameB");
            AddTeam(teamId: 10, teamName: "Z", divisionId: 2, divisionName: "DivisionNameB", groupId: 5, groupName: "GroupNameC");

            return this;
        }

        public List<TeamTournamentDto> Build()
        {
            return _teams;
        }

        private void AddTeam(int teamId, string teamName, int divisionId = 1, int groupId = 1, string divisionName = "DivisionNameA", string groupName = null)
        {
            _teams.Add(new TeamTournamentDto
            {
                TeamId = teamId,
                TeamName = $"TeamName{teamName}",
                DivisionId = divisionId,
                GroupId = groupId,
                DivisionName = divisionName,
                GroupName = groupName
            });
        }

        public TeamInTournamentTestFixture With8TeamsPlayoff()
        {
            AddTeam(1, "A");
            AddTeam(2, "B");
            AddTeam(3, "C");
            AddTeam(4, "D");
            AddTeam(5, "E");
            AddTeam(6, "F");
            AddTeam(7, "G");
            AddTeam(8, "H");
            return this;
        }
    }
}