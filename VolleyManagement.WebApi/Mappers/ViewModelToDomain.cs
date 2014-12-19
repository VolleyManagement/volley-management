namespace VolleyManagement.WebApi.Mappers
{
    using System;
    using System.Linq;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.WebApi.ViewModels.Tournaments;

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
            var tournament = new Tournament();
            tournament.Id = tournamentViewModel.Id;
            tournament.Name = tournamentViewModel.Name;
            tournament.Description = tournamentViewModel.Description;
            tournament.Season = tournamentViewModel.Season;
            tournament.RegulationsLink = tournamentViewModel.RegulationsLink;
            tournament.Scheme = Enum.GetValues(typeof(TournamentSchemeEnum))
                .Cast<TournamentSchemeEnum>()
                .FirstOrDefault(v => v.ToDescription() == tournamentViewModel.Scheme);
            return tournament;
        }
    }
}