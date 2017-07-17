namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Domain.TournamentsAggregate;
    using ViewModels.Division;

    /// <summary>
    /// Tournament groups list view model.
    /// </summary>
    public class TournamentGroupsListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentGroupsListViewModel"/> class
        /// </summary>
        public TournamentGroupsListViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentGroupsListViewModel"/> class
        /// </summary>
        /// <param name="source">All needed groups</param>
        /// <param name="divisionId">Division id</param>
        public TournamentGroupsListViewModel(List<Group> source, int divisionId)
        {
            DivisionId = divisionId;
            List = source.Select(GroupViewModel.Map).ToList();
        }

        /// <summary>
        /// Gets or sets division Id
        /// </summary>
        public int DivisionId { get; set; }

        /// <summary>
        /// Gets or sets list of Groups
        /// </summary>
        public List<GroupViewModel> List { get; set; }

        /// <summary>
        /// Maps presentation list to domain list
        /// </summary>
        /// <returns>Domain list of groups</returns>
        public List<Group> ToDomain()
        {
            return List.Select(t => t.ToDomain()).ToList();
        }
    }
}