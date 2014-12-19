namespace VolleyManagement.Mvc.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Maps view models to domain models
    /// </summary>
    public static class ViewModelToDomain
    {
        /// <summary>
        /// Maps Tournament.
        /// </summary>
        /// <param name="tournamentViewModel">Tournament view model</param>
        /// <returns>Tournament Domain model</returns>
        public static Tournament Map(TournamentViewModel tournamentViewModel)
        {
            Tournament tournament = new Tournament();
            tournament.Id = tournamentViewModel.Id;
            tournament.Name = tournamentViewModel.Name;
            tournament.Description = tournamentViewModel.Description;
            tournament.Season = tournamentViewModel.Season;
            tournament.Scheme = tournamentViewModel.Scheme;
            tournament.RegulationsLink = tournamentViewModel.RegulationsLink;
            return tournament;
        }
    }
}