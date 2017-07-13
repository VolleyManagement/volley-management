namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Domain.TournamentsAggregate;
    using ViewModels.Division;

    /// <summary>
    /// Tournament divisions list view model.
    /// </summary>
    public class TournamentDivisionsListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentDivisionsListViewModel"/> class
        /// </summary>
        public TournamentDivisionsListViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentDivisionsListViewModel"/> class
        /// </summary>
        /// <param name="source">All needed divisions</param>
        /// <param name="tournamentId">Tournament id</param>
        public TournamentDivisionsListViewModel(List<Division> source, int tournamentId)
        {
            TournamentId = tournamentId;
            List = source.Select(t => DivisionViewModel.Map(t)).ToList();
        }

        /// <summary>
        /// Gets or sets division Id
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets list of Division
        /// </summary>
        public List<DivisionViewModel> List { get; set; }

        /// <summary>
        /// Maps presentation list to domain list
        /// </summary>
        /// <returns>Domain list of divisions</returns>
        public List<Division> ToDomain()
        {
            return List.Select(t => t.ToDomain()).ToList();
        }
    }
}