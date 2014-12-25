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
            Tournament tournament = new Tournament();
            tournament.Id = tournamentViewModel.Id;
            tournament.Name = tournamentViewModel.Name;
            tournament.Description = tournamentViewModel.Description;
            tournament.Season = tournamentViewModel.Season;
            tournament.Scheme = tournamentViewModel.Scheme;
            tournament.RegulationsLink = tournamentViewModel.RegulationsLink;
            return tournament;
        }

        /// <summary>
        /// Maps User.
        /// </summary>
        /// <param name="userViewModel">User view model</param>
        /// <returns>User Domain model</returns>
        public static User Map(UserViewModel userViewModel)
        {
            User user = new User();
            user.Id = userViewModel.Id;
            user.UserName = userViewModel.UserName;
            user.Password = userViewModel.Password;
            user.FullName = userViewModel.FullName;
            user.CellPhone = userViewModel.CellPhone;
            user.Email = userViewModel.Email;
            return user;
        }
    }
}