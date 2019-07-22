﻿namespace VolleyManagement.UI.Areas.Admin.Models
{
    using System.Collections.Generic;
    using Domain.Dto;
    using Domain.RolesAggregate;
    using Domain.UsersAggregate;
    using Mvc.ViewModels.Players;

    /// <summary>
    /// The user view model.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the person's name.
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user is Blocked.
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Gets or sets Player info
        /// </summary>
        public PlayerViewModel Player { get; set; }

        /// <summary>
        /// Gets or sets the login providers.
        /// </summary>
        public IEnumerable<LoginProviderInfo> LoginProviders { get; set; }

        /// <summary>
        /// Gets or sets the login providers.
        /// </summary>
        public IEnumerable<Role> Roles { get; set; }

        /// <summary>
        /// Creates <see cref="UserViewModel"/> instance based on <see cref="UserInRoleDto"/>
        /// </summary>
        /// <param name="user"> The source instance. </param>
        /// <returns> The <see cref="UserViewModel"/>. </returns>
        public static UserViewModel From(UserInRoleDto user)
        {
            return new UserViewModel { Id = user.UserId, Name = user.UserName };
        }

        /// <summary>
        /// Creates <see cref="UserViewModel"/> instance
        /// </summary>
        /// <param name="user"> The source instance. </param>
        /// <returns> The <see cref="UserViewModel"/>. </returns>
        public static UserViewModel Initialize(User user)
        {
            return new UserViewModel {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                PersonName = user.PersonName,
                Phone = user.PhoneNumber,
                IsBlocked = user.IsBlocked
            };
        }

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="user"><see cref="User"/> domain entity.</param>
        /// <returns> View model object </returns>
        public static UserViewModel Map(User user)
        {
            var userViewModel = new UserViewModel {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                PersonName = user.PersonName,
                Phone = user.PhoneNumber,
                IsBlocked = user.IsBlocked,
                LoginProviders = user.LoginProviders,
                Roles = user.Roles
            };

            if (user.Player != null)
            {
                userViewModel.Player = PlayerViewModel.Map(user.Player);
            }

            return userViewModel;
        }
    }
}