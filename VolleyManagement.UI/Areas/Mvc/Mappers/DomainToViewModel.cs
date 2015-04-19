namespace VolleyManagement.UI.Areas.Mvc.Mappers
{
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Users;

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
            return new TournamentViewModel
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                Season = tournament.Season,
                Scheme = tournament.Scheme,
                RegulationsLink = tournament.RegulationsLink,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate
            };
        }

        /// <summary>
        /// Maps User. Password is set to empty string to avoid exposing it out of server.
        /// </summary>
        /// <param name="user">User Domain model</param>
        /// <returns>User view model</returns>
        public static UserViewModel Map(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = string.Empty,
                FullName = user.FullName,
                CellPhone = user.CellPhone,
                Email = user.Email
            };
        }
    }
}