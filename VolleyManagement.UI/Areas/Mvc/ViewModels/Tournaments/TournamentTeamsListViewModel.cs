namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Domain.TeamsAggregate;

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
            List = source.Select(t => TeamNameViewModel.Map(t)).ToList();
        }

        /// <summary>
        /// Gets or sets tournament Id
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets list Of Teams
        /// </summary>
        public List<TeamNameViewModel> List { get; set; }

        /// <summary>
        /// Maps presentation list to domain list
        /// </summary>
        /// <returns>Domain list of teams</returns>
        public List<Team> ToDomain()
        {
            return List.Select(t => t.ToDomain()).ToList();
        }
    }
}