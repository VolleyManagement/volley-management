namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Mvc.ViewModels.Players;

    /// <summary>
    /// Player view model builder
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PlayerMvcViewModelBuilder
    {
        /// <summary>
        /// Holds test player view model instance
        /// </summary>
        private PlayerViewModel _playerViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerMvcViewModelBuilder"/> class
        /// </summary>
        public PlayerMvcViewModelBuilder()
        {
            _playerViewModel = new PlayerViewModel()
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                BirthYear = 1983,
                Height = 186,
                Weight = 96,
                TeamId = 1
            };
        }

        /// <summary>
        /// Sets the player view model Id
        /// </summary>
        /// <param name="id">Player view model Id</param>
        /// <returns>Player view model builder object</returns>
        public PlayerMvcViewModelBuilder WithId(int id)
        {
            _playerViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the player view model first name
        /// </summary>
        /// <param name="firstName">Player view model first name</param>
        /// <returns>Player view model builder object</returns>
        public PlayerMvcViewModelBuilder WithFirstName(string firstName)
        {
            _playerViewModel.FirstName = firstName;
            return this;
        }

        /// <summary>
        /// Sets the player view model last name
        /// </summary>
        /// <param name="lastName">Player view model last name</param>
        /// <returns>Player view model builder object</returns>
        public PlayerMvcViewModelBuilder WithLastName(string lastName)
        {
            _playerViewModel.LastName = lastName;
            return this;
        }

        /// <summary>
        /// Sets the player view model birth year
        /// </summary>
        /// <param name="birthYear">Player view model birth year</param>
        /// <returns>Player view model builder object</returns>
        public PlayerMvcViewModelBuilder WithBirthYear(short birthYear)
        {
            _playerViewModel.BirthYear = birthYear;
            return this;
        }

        /// <summary>
        /// Sets the player view model height
        /// </summary>
        /// <param name="height">Player view model height</param>
        /// <returns>Player view model builder object</returns>
        public PlayerMvcViewModelBuilder WithHeight(short height)
        {
            _playerViewModel.Height = height;
            return this;
        }

        /// <summary>
        /// Sets the player view model weight
        /// </summary>
        /// <param name="weight">Player view model weight</param>
        /// <returns>Player view model builder object</returns>
        public PlayerMvcViewModelBuilder WithWeight(short weight)
        {
            _playerViewModel.Weight = weight;
            return this;
        }

        /// <summary>
        /// Sets the player TeamId
        /// </summary>
        /// <param name="teamId">Player view model teamId</param>
        /// <returns>Player view model builder object</returns>
        public PlayerMvcViewModelBuilder WithTeamId(int? teamId)
        {
            _playerViewModel.TeamId = teamId;
            return this;
        }

        /// <summary>
        /// Builds a test player view model
        /// </summary>
        /// <returns>test player view view model</returns>
        public PlayerViewModel Build()
        {
            return _playerViewModel;
        }
    }
}
