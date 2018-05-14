namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.PlayersAggregate;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PlayerServiceTestFixture
    {
        /// <summary>
        /// Holds collection of players
        /// </summary>
        private readonly List<Player> _players = new List<Player>();

        /// <summary>
        /// Adds players to collection
        /// </summary>
        /// <returns>Builder object with collection of players</returns>
        public PlayerServiceTestFixture TestPlayers()
        {
            _players.Add(new Player(1, "FirstNameA", "LastNameA", 1989, 211, 120));
            _players.Add(new Player(2, "FirstNameB", "LastNameB", 1984, 205, 99));
            _players.Add(new Player(3, "FirstNameC", "LastNameC", 1969, 169, 79));

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
        public List<Player> Build()
        {
            return _players;
        }
    }
}