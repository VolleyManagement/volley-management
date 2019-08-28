namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Mvc.ViewModels.Users;

    /// <summary>
    /// Builder for test MVC user view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class UserMvcViewModelBuilder
    {
        /// <summary>
        /// Holds test user view model instance
        /// </summary>
        private UserViewModel _userViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMvcViewModelBuilder"/> class
        /// </summary>
        public UserMvcViewModelBuilder()
        {
            _userViewModel = new UserViewModel()
            {
                Id = 1,
                FullName = "Name",
                CellPhone = "068-11-22-777",
                Email = "exampler@i.ua",
                LoginProviders = new List<string>()
                {
                    "Google",
                    "Twitter",
                    "Facebook"
                }
            };
        }

        /// <summary>
        /// Sets id of test user view model
        /// </summary>
        /// <param name="id">Id for test user view model</param>
        /// <returns>User view model builder object</returns>
        public UserMvcViewModelBuilder WithId(int id)
        {
            _userViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test user view model
        /// </summary>
        /// <param name="name">Name for test user view model</param>
        /// <returns>User view model builder object</returns>
        public UserMvcViewModelBuilder WithName(string name)
        {
            _userViewModel.FullName = name;
            return this;
        }

        /// <summary>
        /// Sets Username of user view model
        /// </summary>
        /// <param name="userName">Username for test user view model</param>
        /// <returns>User view model builder object</returns>
        public UserMvcViewModelBuilder WithUserName(string userName)
        {
            _userViewModel.UserName = userName;
            return this;
        }

        /// <summary>
        /// Sets Cellphone of user view model
        /// </summary>
        /// <param name="cellphone">Cellphone for test user view model</param>
        /// <returns>User view model builder object</returns>
        public UserMvcViewModelBuilder WithCellPhone(string cellphone)
        {
            _userViewModel.CellPhone = cellphone;
            return this;
        }

        /// <summary>
        /// Sets login providers of test user view model
        /// </summary>
        /// <param name="providers">Login providers for test user view model</param>
        /// <returns>User view model builder object</returns>
        public UserMvcViewModelBuilder WithProviders(List<string> providers)
        {
            _userViewModel.LoginProviders = providers;
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
