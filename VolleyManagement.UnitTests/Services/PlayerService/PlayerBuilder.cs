namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.Players;

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
            this._player = new Player
            {
                Id = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                BirthYear = 1983,
                Height = 186,
                Weight = 93
            };
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
        public PlayerBuilder WithBirthYear(int birthYear)
        {
            _player.BirthYear = birthYear;
            return this;
        }

        /// <summary>
        /// Sets player test height
        /// </summary>
        /// <param name="height">Test player height</param>
        /// <returns>Player builder object</returns>
        public PlayerBuilder WithHeight(int height)
        {
            _player.Height = height;
            return this;
        }

        /// <summary>
        /// Sets player test weight
        /// </summary>
        /// <param name="weight">Test player weight</param>
        /// <returns>Player builder object</returns>
        public PlayerBuilder WithWeight(int weight)
        {
            _player.Weight = weight;
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
