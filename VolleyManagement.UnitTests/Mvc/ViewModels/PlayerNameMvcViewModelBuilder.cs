namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;

    /// <summary>
    /// Player name view model builder
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PlayerNameMvcViewModelBuilder
    {
        /// <summary>
        /// Holds test player name view model instance
        /// </summary>
        private PlayerNameViewModel _playerNameViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerNameMvcViewModelBuilder"/> class
        /// </summary>
        public PlayerNameMvcViewModelBuilder()
        {
            _playerNameViewModel = new PlayerNameViewModel()
            {
                Id = 1,
            };
        }

        /// <summary>
        /// Sets the player name view model Id
        /// </summary>
        /// <param name="id">Player name view model Id</param>
        /// <returns>Player name view model builder object</returns>
        public PlayerNameMvcViewModelBuilder WithId(int id)
        {
            _playerNameViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Builds a test player name view model
        /// </summary>
        /// <returns>test player name view model</returns>
        public PlayerNameViewModel Build()
        {
            return _playerNameViewModel;
        }
    }
}
