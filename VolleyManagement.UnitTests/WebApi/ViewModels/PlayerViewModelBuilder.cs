namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;

    /// <summary>
    /// Builder for test player view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    class PlayerViewModelBuilder
    {
        /// <summary>
        /// Holds test player view model instance
        /// </summary>
        private PlayerViewModel _playerViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerViewModelBuilder"/> class
        /// </summary>
        public PlayerViewModelBuilder()
        {
            _playerViewModel = new PlayerViewModel()
            {
                Id = 1,
                FirstName = "Jack",
                LastName = "Nicholson",
                BirthYear = 1965,
                Height = 180,
                Weight = 100,
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
        /// Builds test tournament view model
        /// </summary>
        /// <returns>test tournament view model</returns>
        public PlayerViewModel Build()
        {
            return _playerViewModel;
        }
    }
}
