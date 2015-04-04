namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;

    /// <summary>
    /// Builder for test player view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PlayerViewModelBuilder
    {
        /// <summary>
        /// Holds test player view model instance
        /// </summary>
        private PlayerViewModel _playerViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerViewModelBuilder" /> class
        /// </summary>
        public PlayerViewModelBuilder()
        {
            _playerViewModel = new PlayerViewModel()
            {
                Id = 2,
                FirstName = "First name 1",
                LastName = "Last name 1",
                BirthYear = 1955,
                Weight = 100,
                Height = 180
            };
        }

        /// <summary>
        /// Sets id of test player view model
        /// </summary>
        /// <param name="id">Id for test player view model</param>
        /// <returns>Player view model builder object</returns>
        public PlayerViewModelBuilder WithId(int id)
        {
            _playerViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the first name of test player view model
        /// </summary>
        /// <param name="firstName">First name for test player view model</param>
        /// <returns>Player view model builder object</returns>
        public PlayerViewModelBuilder WithFirstName(string firstName)
        {
            _playerViewModel.FirstName = firstName;
            return this;
        }

        /// <summary>
        /// Sets the last name of test player view model
        /// </summary>
        /// <param name="lastName">Last name fir test player view model</param>
        /// <returns>Player view model builder object</returns>
        public PlayerViewModelBuilder WithLastName(string lastName)
        {
            _playerViewModel.LastName = lastName;
            return this;
        }

        /// <summary>
        /// Sets the birth year view model
        /// </summary>
        /// <param name="birthYear">Birth year for test player view model</param>
        /// <returns>Player view model builder object</returns>
        public PlayerViewModelBuilder WithBirthYear(int birthYear)
        {
            _playerViewModel.BirthYear = birthYear;
            return this;
        }

        /// <summary>
        /// Sets the weight view model
        /// </summary>
        /// <param name="weight">Weight for test player view model</param>
        /// <returns>Player view model builder object</returns>
        public PlayerViewModelBuilder WithWeight(int weight)
        {
            _playerViewModel.Weight = weight;
            return this;
        }

        /// <summary>
        /// Sets the height view model
        /// </summary>
        /// <param name="height">Height for test player view model</param>
        /// <returns>Player view model builder object</returns>
        public PlayerViewModelBuilder WithHeight(int height)
        {
            _playerViewModel.Height = height;
            return this;
        }

        /// <summary>
        /// Builds test player view model
        /// </summary>
        /// <returns>test player view model</returns>
        public PlayerViewModel Build()
        {
            return _playerViewModel;
        }
    }
}
