namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System.Diagnostics.CodeAnalysis;
    using Domain.PlayersAggregate;

    /// <summary>
    /// Player domain object builder
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PlayerBuilder
    {
        /// <summary>
        /// Holds test player instance
        /// </summary>
        private Player _player;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBuilder"/> class
        /// </summary>
        public PlayerBuilder()
        {
            _player = new Player(1, "FirstName", "LastName", 1983, 186, 96, 1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBuilder"/> class
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        public PlayerBuilder(int id, string firstName, string lastName)
        {
            _player = new Player(id, firstName, lastName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBuilder"/> class
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="birthYear">BirthYear</param>
        /// <param name="height">Height</param>
        /// <param name="weight">Weight</param>
        public PlayerBuilder(int id, string firstName, string lastName, short birthYear, short height, short weight)
        {
            _player = new Player(id, firstName, lastName, birthYear, height, weight);
        }

        /// <summary>
        /// Sets player test Id
        /// </summary>
        /// <param name="id">Test player Id</param>
        /// <returns>Player builder object</returns>
        public PlayerBuilder WithId(int id)
        {
            _player.Id = id;
            return this;
        }

        /// <summary>
        /// Sets player test teamId
        /// </summary>
        /// <param name="teamId">Test player weight</param>
        /// <returns>Player builder object</returns>
        public PlayerBuilder WithTeamId(int? teamId)
        {
            _player.TeamId = teamId;
            return this;
        }

        /// <summary>
        /// Sets player test teamId = null
        /// </summary>
        /// <returns>Player builder object</returns>
        public PlayerBuilder WithNoTeam()
        {
            _player.TeamId = null;
            return this;
        }

        /// <summary>
        /// Builds test player
        /// </summary>
        /// <returns>Test player</returns>
        public Player Build()
        {
            return _player;
        }
    }
}
