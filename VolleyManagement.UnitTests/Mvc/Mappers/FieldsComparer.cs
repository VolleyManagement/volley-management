namespace VolleyManagement.UnitTests.Mvc.Mappers
{
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Class to compare fields for domain and view models
    /// </summary>
    public static class FieldsComparer
    {
        /// <summary>
        /// Method to check the mapping.
        /// </summary>
        /// <param name="tournament">domain model</param>
        /// <param name="tournamentViewModel">view model</param>
        /// <returns>true if fields are equal</returns>
        public static bool AreFieldsEqual(Tournament tournament, TournamentViewModel tournamentViewModel)
        {
            if (tournament.Id == tournamentViewModel.Id &&
                tournament.Name == tournamentViewModel.Name &&
                tournament.Description == tournamentViewModel.Description &&
                tournament.RegulationsLink == tournamentViewModel.RegulationsLink &&
                tournament.Scheme == tournamentViewModel.Scheme &&
                tournament.Season == tournamentViewModel.Season)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
