namespace VolleyManagement.WebApi.Mappers
{
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
            var tournament = new Tournament
            {
                Name = tournamentViewModel.Name,
                Description = tournamentViewModel.Description,
                Season = tournamentViewModel.Season,
                ////Scheme = tournamentViewModel.Scheme,
                RegulationsLink = tournamentViewModel.RegulationsLink,
            };
            return tournament;
        }
    }
}