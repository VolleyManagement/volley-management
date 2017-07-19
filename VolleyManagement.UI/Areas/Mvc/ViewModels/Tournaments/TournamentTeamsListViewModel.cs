namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Domain.TeamsAggregate;
    using Domain.TournamentsAggregate;
    using ViewModels.Division;

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
        /// <param name="group">All needed groups</param>
        /// <param name="divisionId">Division id</param>
        public TournamentTeamsListViewModel(List<Team> source, int tournamentId, List<Group> group, int divisionId)
        {
            TournamentId = tournamentId;
            List = source.Select(TeamNameViewModel.Map).ToList();
            DivisionId = divisionId;
            GroupsList = group.Select(GroupViewModel.Map).ToList();
        }

        /// <summary>
        /// Gets or sets tournament Id
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets division Id
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// Gets or sets list Of Teams
        /// </summary>
        public List<TeamNameViewModel> List { get; set; }

        /// <summary>
        /// Gets or sets list of Groups
        /// </summary>
        public List<GroupViewModel> GroupsList { get; set; }

        /// <summary>
        /// Maps presentation list to domain list
        /// </summary>
        /// <returns>Domain list of teams</returns>
        public List<Team> ToDomain()
        {
            return List.Select(t => t.ToDomain()).ToList();
        }

        /// <summary>
        /// Maps presentation list to domain list
        /// </summary>
        /// <returns>Domain list of groups</returns>
        public List<Group> GroupToDomain()
        {
            return GroupsList.Select(t => t.ToDomain()).ToList();
        }
    }
}