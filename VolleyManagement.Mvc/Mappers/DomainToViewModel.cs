namespace VolleyManagement.Mvc.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.Mvc.ViewModels.Tournaments;
    using VolleyManagement.Mvc.ViewModels.Users;

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
            TournamentViewModel tournamentViewModel = new TournamentViewModel();
            tournamentViewModel.Id = tournament.Id;
            tournamentViewModel.Name = tournament.Name;
            tournamentViewModel.Description = tournament.Description;
            tournamentViewModel.Season = tournament.Season;
            tournamentViewModel.Scheme = tournament.Scheme;
            tournamentViewModel.RegulationsLink = tournament.RegulationsLink;
            return tournamentViewModel;
        }

        /// <summary>
        /// Maps User.
        /// </summary>
        /// <param name="user">User Domain model</param>
        /// <returns>User view model</returns>
        public static UserViewModel Map(User user)
        {
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Id = user.Id;
            userViewModel.UserName = user.UserName;
            userViewModel.Password = user.Password;
            userViewModel.FullName = user.FullName;
            userViewModel.CellPhone = user.CellPhone;
            userViewModel.Email = user.Email;
            return userViewModel;
        }
    }
}