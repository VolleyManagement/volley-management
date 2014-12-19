namespace VolleyManagement.Mvc.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Maps Domain models to view models
    /// </summary>
    public static class DomainToViewModel
    {
        /// <summary>
        /// Maps Tournament.
        /// </summary>
        /// <param name="tournament">Tournament Domain model</param>
        /// <returns>Tournament view model</returns>
        public static TournamentViewModel Map(Tournament tournament)
        {
            TournamentViewModel tournamentViewModel = new TournamentViewModel();
            tournamentViewModel.Id = tournament.Id;
            tournamentViewModel.Name = tournament.Name;
            tournamentViewModel.Description = tournament.Description;
            tournamentViewModel.Season = tournament.Season;
            tournamentViewModel.Scheme = tournament.Scheme;
            tournamentViewModel.RegulationsLink = tournament.RegulationsLink;
            return tournamentViewModel;
        }
    }
}