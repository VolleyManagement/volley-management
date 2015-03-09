namespace VolleyManagement.UI.Areas.Mvc.Mappers
{
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Users;

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
            return new Tournament
            {
                Id = tournamentViewModel.Id,
                Name = tournamentViewModel.Name,
                Description = tournamentViewModel.Description,
                Season = tournamentViewModel.Season,
                Scheme = tournamentViewModel.Scheme,
                RegulationsLink = tournamentViewModel.RegulationsLink
            };
        }

        /// <summary>
        /// Maps User.
        /// </summary>
        /// <param name="userViewModel">User view model</param>
        /// <returns>User Domain model</returns>
        public static User Map(UserViewModel userViewModel)
        {
            return new User
            {
                Id = userViewModel.Id,
                UserName = userViewModel.UserName,
                Password = userViewModel.Password,
                FullName = userViewModel.FullName,
                CellPhone = userViewModel.CellPhone,
                Email = userViewModel.Email
            };
        }
    }
}