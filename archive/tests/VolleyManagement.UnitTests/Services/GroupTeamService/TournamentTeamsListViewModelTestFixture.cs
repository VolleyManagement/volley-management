﻿namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Class for generating Teams with specific Id, Name
    /// And for GroupTeams with TeamId and GroupId
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TournamentTeamsListViewModelTestFixture
    {
        /// <summary>
        /// Holds collection of teams and groupteams
        /// </summary>
        private TournamentTeamsListViewModel _groupsteams = new TournamentTeamsListViewModel();

        /// <summary>
        /// Return test collection of teams and groupteams
        /// </summary>
        /// <returns>Builder object with collection of teams and groupteams</returns>
        public TournamentTeamsListViewModelTestFixture TestTournamentTeams()
        {
            _groupsteams = new TournamentTeamsListViewModel()
            {
                TeamsList = new List<TeamNameViewModel>
                {
                    new TeamNameViewModel()
                    {
                        Id = 1,
                        Name = "Team 1",
                    },
                    new TeamNameViewModel()
                    {
                        Id = 2,
                        Name = "Team 2",
                    },
                },
                GroupTeamList = new List<GroupTeamViewModel>()
                {
                    new GroupTeamViewModel()
                    {
                        TeamId = 1,
                        GroupId = 1,
                    },
                    new GroupTeamViewModel()
                    {
                        TeamId = 2,
                        GroupId = 1,
                    },
                },
                TournamentId = 1,
            };
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Team and GroupTeam collection</returns>
        public TournamentTeamsListViewModel Build()
        {
            return _groupsteams;
        }
    }
}
