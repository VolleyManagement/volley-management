namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Mvc.ViewModels.Users;

    /// <summary>
    /// Builder for test MVC user edit view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class UserEditMvcViewModelBuilder
    {
        /// <summary>
        /// Holds test user edit view model instance
        /// </summary>
        private UserEditViewModel _userEditViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEditMvcViewModelBuilder"/> class
        /// </summary>
        public UserEditMvcViewModelBuilder()
        {
            _userEditViewModel = new UserEditViewModel()
            {
                Id = 1,
                FullName = "Name",
                CellPhone = "068-11-22-777",
                Email = "exampler@i.ua",
            };
        }

        /// <summary>
        /// Sets id of test user edit view model
        /// </summary>
        /// <param name="id">Id for test user edit view model</param>
        /// <returns>User edit view model builder object</returns>
        public UserEditMvcViewModelBuilder WithId(int id)
        {
            _userEditViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test user edit view model
        /// </summary>
        /// <param name="name">Name for test user edit view model</param>
        /// <returns>User edit view model builder object</returns>
        public UserEditMvcViewModelBuilder WithName(string name)
        {
            _userEditViewModel.FullName = name;
            return this;
        }

        /// <summary>
        /// Sets Cellphone of user edit view model
        /// </summary>
        /// <param name="cellphone">Cellphone for test user edit view model</param>
        /// <returns>User edit view model builder object</returns>
        public UserEditMvcViewModelBuilder WithCellPhone(string cellphone)
        {
            _userEditViewModel.CellPhone = cellphone;
            return this;
        }

        /// <summary>
        /// Sets email of user edit view model
        /// </summary>
        /// <param name="email">Email for test user edit view model</param>
        /// <returns>User edit view model builder object</returns>
        public UserEditMvcViewModelBuilder WithEmail(string email)
        {
            _userEditViewModel.Email = email;
            return this;
        }

        /// <summary>
        /// Builds test user edit view model
        /// </summary>
        /// <returns>test user edit view model</returns>
        public UserEditViewModel Build()
        {
            return _userEditViewModel;
        }
    }
}
