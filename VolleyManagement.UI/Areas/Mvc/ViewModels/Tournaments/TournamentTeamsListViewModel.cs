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
        /// <param name="source">All needed teams</param>
        /// <param name="tournamentId">Tournament id</param>
        public TournamentTeamsListViewModel(List<Team> source, int tournamentId)
        {
            this.TournamentId = tournamentId;
            this.List = new List<TeamNameViewModel>();
            foreach (var team in source)
            {
                this.List.Add(TeamNameViewModel.Map(team));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentTeamsListViewModel"/> class
        /// </summary>
        public TournamentTeamsListViewModel()
        {
        }

        /// <summary>
        /// Tournament Id
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// List Of Teams
        /// </summary>
        public List<TeamNameViewModel> List { get; set; }

        /// <summary>
        /// Maps presentation list to domain list
        /// </summary>
        /// <returns>Domain list of teams</returns>
        public List<Team> ToDomainList()
        {
            var result = new List<Team>();
            foreach (var teamNameViewModel in this.List)
            {
                result.Add(teamNameViewModel.ToDomain());
            }

            return result;
        }
    }
}