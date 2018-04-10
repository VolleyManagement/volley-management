namespace VolleyManagement.UI.Areas.Mvc.Mappers
{
    using Domain.TournamentsAggregate;
    using Domain.UsersAggregate;
    using ViewModels.Tournaments;
    using ViewModels.Users;

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
            return new TournamentViewModel {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                Location = tournament.Location,
                Season = tournament.Season,
                Scheme = tournament.Scheme,
                RegulationsLink = tournament.RegulationsLink,
                GamesStart = tournament.GamesStart,
                GamesEnd = tournament.GamesEnd
            };
        }

        /// <summary>
        /// Maps User. Password is set to empty string to avoid exposing it out of server.
        /// </summary>
        /// <param name="user">User Domain model</param>
        /// <returns>User view model</returns>
        public static UserViewModel Map(User user)
        {
            return new UserViewModel {
                Id = user.Id,
                UserName = user.UserName,
                Password = string.Empty,
                FullName = user.PersonName,
                CellPhone = user.PhoneNumber,
                Email = user.Email
            };
        }
    }
}