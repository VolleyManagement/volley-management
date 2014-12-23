namespace VolleyManagement.WebApi.Mappers
{
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.WebApi.ViewModels.Tournaments;
    using VolleyManagement.WebApi.ViewModels.Users;

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
                Scheme = tournament.Scheme.ToDescription()
            };

            return tournamentViewModel;
        }

        /// <summary>
        /// Maps User.
        /// </summary>
        /// <param name="user">User Domain model</param>
        /// <returns>User ViewModel</returns>
        public static UserViewModel Map(User user)
        {
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                CellPhone = user.CellPhone,
                Password = string.Empty
            };

            return userViewModel;
        }
    }
}