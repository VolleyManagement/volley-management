namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Users;
    using VolleyManagement.UnitTests.Services.UserService;

    /// <summary>
    /// Builder for test MVC user view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class UserMvcViewModelBuilder
    {
        /// <summary>
        /// Holds test user instance
        /// </summary>
        private UserViewModel _userViewModel;

        /// <summary>
        ///  Initializes a new instance of the <see cref="UserBuilder"/> class
        /// </summary>
        public UserMvcViewModelBuilder()
        {
            _userViewModel = new UserViewModel
            {
                Id = 1,
                UserName = "testLogin",
                Email = "test@gmail.com",
                Password = "abc123",
                CellPhone = "0500000001",
                FullName = "test full name"
            };
        }

        /// <summary>
        /// Sets test user id
        /// </summary>
        /// <param name="id">test user id</param>
        /// <returns>User builder object</returns>
        public UserMvcViewModelBuilder WithId(int id)
        {
            _userViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets test user name
        /// </summary>
        /// <param name="userName">test user name</param>
        /// <returns>User builder object</returns>
        public UserMvcViewModelBuilder WithUserName(string userName)
        {
            _userViewModel.UserName = userName;
            return this;
        }

        /// <summary>
        /// Sets test user FullName
        /// </summary>
        /// <param name="fullName">test user FullName</param>
        /// <returns>User builder object</returns>
        public UserMvcViewModelBuilder WithFullName(string fullName)
        {
            _userViewModel.FullName = fullName;
            return this;
        }

        /// <summary>
        /// Sets test user email
        /// </summary>
        /// <param name="email">test user email</param>
        /// <returns>User builder object</returns>
        public UserMvcViewModelBuilder WithEmail(string email)
        {
            _userViewModel.Email = email;
            return this;
        }

        /// <summary>
        /// Sets test user password
        /// </summary>
        /// <param name="password">test user name</param>
        /// <returns>User builder object</returns>
        public UserMvcViewModelBuilder WithPassword(string password)
        {
            _userViewModel.Password = password;
            return this;
        }

        /// <summary>
        /// Sets test user cell phone
        /// </summary>
        /// <param name="cellPhone">test user cell phone</param>
        /// <returns>User builder object</returns>
        public UserMvcViewModelBuilder WithCellPhone(string cellPhone)
        {
            _userViewModel.CellPhone = cellPhone;
            return this;
        }

        /// <summary>
        /// Builds test user
        /// </summary>
        /// <returns>Test user</returns>
        public UserViewModel Build()
        {
            return _userViewModel;
        }
    }
}