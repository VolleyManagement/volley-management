using VolleyManagement.Domain.RolesAggregate;
using VolleyManagement.Domain.UsersAggregate;
using VolleyManagement.UI.Areas.Admin.Models;
using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;

namespace VolleyManagement.UnitTests.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Builder for test MVC user view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class UserAdminViewModelBuilder
    {
        /// <summary>
        /// Holds test user view model instance
        /// </summary>
        private UserViewModel _userViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAdminViewModelBuilder"/> class
        /// </summary>
        public UserAdminViewModelBuilder()
        {
            _userViewModel = new UserViewModel()
            {
                Id = 1,
                Name = "Player",
                Email = "example@i.ua",
                PersonName = "Eugene",
                Phone = "068-11-22-333",
                LoginProviders = new List<LoginProviderInfo>
                {
                    new LoginProviderInfo
                    {
                        LoginProvider = "Google",
                        ProviderKey = "11111111111111"
                    }
                },
                Roles = new List<Role>
                {
                      new Role
                    {
                        Id = 1,
                        Name = "Administrator"
                    }
                }
            };
        }

        /// <summary>
        /// Sets id of test user view model
        /// </summary>
        /// <param name="id">Id for test user view model</param>
        /// <returns>User view model builder object</returns>
        public UserAdminViewModelBuilder WithId(int id)
        {
            _userViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test user view model
        /// </summary>
        /// <param name="name">Name for test user view model</param>
        /// <returns>User view model builder object</returns>
        public UserAdminViewModelBuilder WithName(string name)
        {
            _userViewModel.PersonName = name;
            return this;
        }

        /// <summary>
        /// Sets Username of user view model
        /// </summary>
        /// <param name="userName">Username for test user view model</param>
        /// <returns>User view model builder object</returns>
        public UserAdminViewModelBuilder WithUserName(string userName)
        {
            _userViewModel.Name = userName;
            return this;
        }

        /// <summary>
        /// Sets Cellphone of user view model
        /// </summary>
        /// <param name="cellphone">Cellphone for test user view model</param>
        /// <returns>User view model builder object</returns>
        public UserAdminViewModelBuilder WithCellPhone(string cellphone)
        {
            _userViewModel.Phone = cellphone;
            return this;
        }

        /// <summary>
        /// Sets login providers of test user view model
        /// </summary>
        /// <param name="providers">Login providers for test user view model</param>
        /// <returns>User view model builder object</returns>
        public UserAdminViewModelBuilder WithProviders(List<LoginProviderInfo> providers)
        {
            _userViewModel.LoginProviders = providers;
            return this;
        }

        /// <summary>
        /// Sets roles of test user view model
        /// </summary>
        /// <param name="roles">Roles for test user view model</param>
        /// <returns>User view model builder object</returns>
        public UserAdminViewModelBuilder WithRoles(List<Role> roles)
        {
            _userViewModel.Roles = roles;
            return this;
        }

        /// <summary>
        /// Sets player belongs to user
        /// </summary>
        /// <param name="player">Player instance</param>
        /// <returns>User view model builder object</returns>
        public UserAdminViewModelBuilder WithPlayer(PlayerViewModel player)
        {
            _userViewModel.Player = player;
            return this;
        }

        /// <summary>
        /// Builds test user view model
        /// </summary>
        /// <returns>test user view model</returns>
        public UserViewModel Build()
        {
            return _userViewModel;
        }
    }
}

