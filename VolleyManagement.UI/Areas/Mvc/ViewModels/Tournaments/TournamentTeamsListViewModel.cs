namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Contracts;
    using Domain.TeamsAggregate;
    using Domain.TournamentsAggregate;
    using ViewModels.Division;
    using ViewModels.Tournaments;

    /// <summary>
    /// Tournament teams list view model.
    /// </summary>
    public class TournamentTeamsListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentTeamsListViewModel"/> class
        /// </summary>
        public TournamentTeamsListViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentTeamsListViewModel"/> class
        /// </summary>
        /// <param name="source">All needed teams</param>
        /// <param name="tournamentId">Tournament id</param>
        public TournamentTeamsListViewModel(List<Team> source, int tournamentId)
        {
            TournamentId = tournamentId;
            TeamsList = source.Select(TeamNameViewModel.Map).ToList();
        }

        /// <summary>
        /// Gets or sets tournament Id
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets list Of Teams
        /// </summary>
        public List<TeamNameViewModel> TeamsList { get; set; }

        /// <summary>
        /// Gets or sets list of GroupTeam
        /// </summary>
        public List<GroupTeamViewModel> GroupTeamList { get; set; }

        /// <summary>
        /// Maps presentation list to domain list
        /// </summary>
        /// <returns>Domain list of teams</returns>
        public List<Team> ToDomain()
        {
            return TeamsList.Select(t => t.ToDomain()).ToList();
        }

        /// <summary>
        /// Maps presentation list to domain list
        /// </summary>
        /// <returns>Domain list of teams and groups</returns>
        public List<TeamToGroupInsert> ToGroupTeamDomain()
        {
            return GroupTeamList.Select(t => t.ToDomain()).ToList();
        }
    }
}