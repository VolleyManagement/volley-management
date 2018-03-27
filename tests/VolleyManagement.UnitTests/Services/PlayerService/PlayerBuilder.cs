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
        /// Sets player test first name
        /// </summary>
        /// <param name="firstName">Test player first name</param>
        /// <returns>Player builder object</returns>
        public PlayerBuilder WithFirstName(string firstName)
        {
            _player.FirstName = firstName;
            return this;
        }

        /// <summary>
        /// Sets player test last name
        /// </summary>
        /// <param name="lastName">Test player last name</param>
        /// <returns>Player builder object</returns>
        public PlayerBuilder WithLastName(string lastName)
        {
            _player.LastName = lastName;
            return this;
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
