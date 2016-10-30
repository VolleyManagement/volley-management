using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VolleyManagement.Domain.RolesAggregate;
using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;

namespace VolleyManagement.UI.Areas.Admin.Models
{
    using VolleyManagement.Domain.Dto;
    using VolleyManagement.Domain.UsersAggregate;

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
        /// Gets or sets the login providers.
        /// </summary>
        public List<LoginProviderViewModel> LoginProviders { get; set; }

        /// <summary>
        /// Gets or sets the login providers.
        /// </summary>
        public List<RoleViewModel> Roles { get; set; }

        /// <summary>
        /// Creates <see cref="UserViewModel"/> instance based on <see cref="UserInRoleDto"/>
        /// </summary>
        /// <param name="user"> The source instance. </param>
        /// <returns> The <see cref="UserViewModel"/>. </returns>
        public static UserViewModel From(UserInRoleDto user)
        {
            return new UserViewModel { Id = user.UserId, Name = user.UserName};
        }

        public static UserViewModel Initialize(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                PersonName = user.PersonName,
                Phone = user.PhoneNumber
            };
        }

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="user"><see cref="User"/> domain entity.</param>
        /// <param name="roles">User's roles.</param>
        /// <param name="authProviders">User's auth providers.</param>
        /// <returns> View model object </returns>
        public static UserViewModel Map(User user, IEnumerable<Role> roles, IEnumerable<LoginProviderInfo> authProviders)
        {
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                PersonName = user.PersonName,
                Phone = user.PhoneNumber
            };

            if (roles != null)
            {
                userViewModel.Roles = new List<RoleViewModel>();

                foreach (var role in roles)
                {
                    userViewModel.Roles.Add(new RoleViewModel(role));
                }

            }

            if (authProviders != null)
            {
                userViewModel.LoginProviders = new List<LoginProviderViewModel>();
                foreach (var provider in authProviders)
                {
                    userViewModel.LoginProviders.Add(new LoginProviderViewModel(provider));
                }
            }

            return userViewModel;
        }
    }
}