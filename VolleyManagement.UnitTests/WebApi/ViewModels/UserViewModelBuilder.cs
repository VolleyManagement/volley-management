namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.WebApi.ViewModels.Users;

    /// <summary>
    /// Builder for test User view model
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class UserViewModelBuilder
    {
         /// <summary>
        /// Holds test user view model instance
        /// </summary>
        private UserViewModel _userViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModelBuilder"/> class
        /// </summary>
        public UserViewModelBuilder()
        {
            _userViewModel = new UserViewModel()
            {
                Id = 1,
                UserName = "testLogin",
                FullName = "Test Name",
                Email = "test@gmail.com",
                Password = "abc123",
                CellPhone = "+38(050)0000000"
            };
        }

        /// <summary>
        /// Sets test user id for view model
        /// </summary>
        /// <param name="id">test id for user view model</param>
        /// <returns>User view model builder object</returns>
        public UserViewModelBuilder WithId(int id)
        {
            _userViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets test UserName for view model
        /// </summary>
        /// <param name="userName">UserName for user view model</param>
        /// <returns>User view model builder object</returns>
        public UserViewModelBuilder WithUserName(string userName)
        {
            _userViewModel.UserName = userName;
            return this;
        }

        /// <summary>
        /// Sets test FullName for view model
        /// </summary>
        /// <param name="fullName">FullName for user view model</param>
        /// <returns>User view model builder object</returns>
        public UserViewModelBuilder WithFullName(string fullName)
        {
            _userViewModel.FullName = fullName;
            return this;
        }

        /// <summary>
        /// Sets test Email for view model
        /// </summary>
        /// <param name="email">Email for user view model</param>
        /// <returns>User view model builder object</returns>
        public UserViewModelBuilder WithEmail(string email)
        {
            _userViewModel.Email = email;
            return this;
        }

        /// <summary>
        /// Sets test Password for view model
        /// </summary>
        /// <param name="password">Password for user view model</param>
        /// <returns>User view model builder object</returns>
        public UserViewModelBuilder WithPassword(string password)
        {
            _userViewModel.Password = password;
            return this;
        }

        /// <summary>
        /// Sets test CellPhone for view model
        /// </summary>
        /// <param name="cellPhone">CellPhone for user view model</param>
        /// <returns>User view model builder object</returns>
        public UserViewModelBuilder WithCellPhone(string cellPhone)
        {
            _userViewModel.CellPhone = cellPhone;
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
