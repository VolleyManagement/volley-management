namespace VolleyManagement.WebApi.Mappers
{
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.WebApi.ViewModels.Tournaments;

    /// <summary>
    /// Maps domain model to view model.
    /// </summary>
    public static class DomainToViewModel
    {
        /// <summary>
        /// Maps Tournament.
        /// </summary>
        /// <param name="tournament">Tournament Domain model</param>
        /// <returns>Tournament ViewModel</returns>
        public static TournamentViewModel Map(Tournament tournament)
        {
            var tournamentViewModel = new TournamentViewModel
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                Season = tournament.Season,
                Scheme = tournament.Scheme.ToString(),
                RegulationsLink = tournament.RegulationsLink,
            };
            return tournamentViewModel;
        }
    }
}