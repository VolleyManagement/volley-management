namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using VolleyManagement.Domain.PlayersAggregate;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PlayerServiceTestFixture
    {
        /// <summary>
        /// Holds collection of players
        /// </summary>
        private IList<Player> _players = new List<Player>();

        /// <summary>
        /// Adds players to collection
        /// </summary>
        /// <returns>Builder object with collection of players</returns>
        public PlayerServiceTestFixture TestPlayers()
        {
            _players.Add(new Player()
            {
                Id = 1,
                FirstName = "FirstNameA",
                LastName = "LastNameA",
                BirthYear = 1989,
                Height = 211,
                Weight = 120
            });
            _players.Add(new Player()
            {
                Id = 2,
                FirstName = "FirstNameB",
                LastName = "LastNameB",
                BirthYear = 1984,
                Height = 205,
                Weight = 99
            });
            _players.Add(new Player()
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
        /// <returns>Builder object with collection of tournaments.</returns>
        public PlayerServiceTestFixture AddPlayer(Player newPlayer)
        {
            _players.Add(newPlayer);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Player collection</returns>
        public IList<Player> Build()
        {
            return _players;
        }
    }
}