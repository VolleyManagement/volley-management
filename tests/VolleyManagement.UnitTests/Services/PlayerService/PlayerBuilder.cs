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
        /// Initializes a new instance with all required parameters.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public PlayerBuilder(int id, string firstName, string lastName)
        {
            _player = new Player(id, firstName, lastName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBuilder"/> class
        /// </summary>
        public PlayerBuilder() : this(1)
        {
        }

        public PlayerBuilder(int id) : this(id, "FirstName", "LastName")
        {
        }

        /// <summary>
        /// Sets player test birth year
        /// </summary>
        /// <param name="birthYear">Test player birth year</param>
        /// <returns>Player builder object</returns>
        public PlayerBuilder WithBirthYear(short? birthYear)
        {
            _player.BirthYear = birthYear;
            return this;
        }

        /// <summary>
        /// Sets player test height
        /// </summary>
        /// <param name="height">Test player height</param>
        /// <returns>Player builder object</returns>
        public PlayerBuilder WithHeight(short? height)
        {
            _player.Height = height;
            return this;
        }

        /// <summary>
        /// Sets player test weight
        /// </summary>
        /// <param name="weight">Test player weight</param>
        /// <returns>Player builder object</returns>
        public PlayerBuilder WithWeight(short? weight)
        {
            _player.Weight = weight;
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
