namespace VolleyManagement.UnitTests.Services.TeamService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TeamsAggregate;
    using PlayerService;
    using VolleyManagement.Contracts;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TournamentTeamsListViewModelTest
    {
        /// <summary>
        /// Holds collection of teams and groupteams
        /// </summary>
        private List<TournamentTeamsListViewModel> _groupsteams = new List<TournamentTeamsListViewModel>();

        /// <summary>
        /// Return test collection of teams and groupteams
        /// </summary>
        /// <returns>Builder object with collection of teams and groupteams</returns>
        public TournamentTeamsListViewModelTest TestGroupsTeamsWithTeamInSecondDivision()
        {
            _groupsteams.Add(new TournamentTeamsListViewModel()
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
            });
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Team and GroupTeam collection</returns>
        public List<TournamentTeamsListViewModel> Build()
        {
            return _groupsteams;
        }
    }
}
