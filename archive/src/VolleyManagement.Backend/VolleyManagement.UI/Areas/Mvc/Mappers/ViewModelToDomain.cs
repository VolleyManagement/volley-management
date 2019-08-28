namespace VolleyManagement.UI.Areas.Mvc.Mappers
{
    using Domain.TournamentsAggregate;
    using Domain.UsersAggregate;
    using ViewModels.Tournaments;
    using ViewModels.Users;

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
            return new Tournament {
                Id = tournamentViewModel.Id,
                Name = tournamentViewModel.Name,
                Description = tournamentViewModel.Description,
                Location = tournamentViewModel.Location,
                Season = tournamentViewModel.Season,
                Scheme = tournamentViewModel.Scheme,
                RegulationsLink = tournamentViewModel.RegulationsLink,
                GamesStart = tournamentViewModel.GamesStart,
                GamesEnd = tournamentViewModel.GamesEnd
            };
        }

        /// <summary>
        /// Maps User.
        /// </summary>
        /// <param name="userViewModel">User view model</param>
        /// <returns>User Domain model</returns>
        public static User Map(UserViewModel userViewModel)
        {
            return new User {
                Id = userViewModel.Id,
                UserName = userViewModel.UserName,
                PhoneNumber = userViewModel.CellPhone,
                Email = userViewModel.Email
            };
        }
    }
}