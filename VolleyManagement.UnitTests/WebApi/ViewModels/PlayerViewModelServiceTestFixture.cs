namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using VolleyManagement.UI.Areas.WebApi.ViewModels.Players;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PlayerViewModelServiceTestFixture
    {
        /// <summary>
        /// Holds collection of players
        /// </summary>
        private IList<PlayerViewModel> _players = new List<PlayerViewModel>();

        /// <summary>
        /// Adds players to collection
        /// </summary>
        /// <returns>Builder object with collection of players</returns>
        public PlayerViewModelServiceTestFixture TestPlayers()
        {
            _players.Add(new PlayerViewModel()
            {
                Id = 1,
                FirstName = "FirstNameA",
                LastName = "LastNameA",
                BirthYear = 1989,
                Height = 211,
                Weight = 120
            });
            _players.Add(new PlayerViewModel()
            {
                Id = 2,
                FirstName = "FirstNameB",
                LastName = "LastNameB",
                BirthYear = 1984,
                Height = 205,
                Weight = 99
            });
            _players.Add(new PlayerViewModel()
            {
                Id = 3,
                FirstName = "FirstNameC",
                LastName = "LastNameC",
                BirthYear = 1969,
                Height = 169,
                Weight = 79
            });
            return this;
        }

        /// <summary>
        /// Add player to collection.
        /// </summary>
        /// <param name="newPlayer">Player to add.</param>
        /// <returns>Builder object with collection of players.</returns>
        public PlayerViewModelServiceTestFixture AddPlayer(PlayerViewModel newPlayer)
        {
            _players.Add(newPlayer);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Player collection</returns>
        public IList<PlayerViewModel> Build()
        {
            return _players;
        }
    }
}
