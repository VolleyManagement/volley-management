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
                RegulationsLink = tournament.RegulationsLink,
            };
            switch (tournament.Scheme)
            {
                case TournamentSchemeEnum.One: tournamentViewModel.Scheme = "1";
                    break;
                case TournamentSchemeEnum.Two: tournamentViewModel.Scheme = "2";
                    break;
                case TournamentSchemeEnum.TwoAndHalf: tournamentViewModel.Scheme = "2.5";
                    break;
            }

            return tournamentViewModel;
        }
    }
}