namespace VolleyManagement.UnitTests.Services.PlayerService
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.PlayersAggregate;

    /// <summary>
    /// Player domain object builder
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CreatePlayerDtoBuilder
    {
        /// <summary>
        /// Holds test player instance
        /// </summary>
        private CreatePlayerDto _player;

        /// <summary>
        /// Initializes a new instance 
        /// </summary>
        public CreatePlayerDtoBuilder()
        {
            _player = new CreatePlayerDto {
                FirstName = "FirstName",
                LastName = "LastName",
                BirthYear = 1983,
                Height = 186,
                Weight = 95
            };
        }

        /// <summary>
        /// Sets player test first name
        /// </summary>
        /// <param name="firstName">Test player first name</param>
        /// <returns>Player builder object</returns>
        public CreatePlayerDtoBuilder WithFirstName(string firstName)
        {
            _player.FirstName = firstName;
            return this;
        }

        /// <summary>
        /// Sets player test second name
        /// </summary>
        /// <param name="firstName">Test player last name</param>
        /// <returns>Player builder object</returns>
        public CreatePlayerDtoBuilder WithLastName(string lastName)
        {
            _player.LastName = lastName;
            return this;
        }

        /// <summary>
        /// Sets player test birth year
        /// </summary>
        /// <param name="birthYear">Test player birth year</param>
        /// <returns>Player builder object</returns>
        public CreatePlayerDtoBuilder WithBirthYear(short? birthYear)
        {
            _player.BirthYear = birthYear;
            return this;
        }

        /// <summary>
        /// Sets player test height
        /// </summary>
        /// <param name="height">Test player height</param>
        /// <returns>Player builder object</returns>
        public CreatePlayerDtoBuilder WithHeight(short? height)
        {
            _player.Height = height;
            return this;
        }

        /// <summary>
        /// Sets player test weight
        /// </summary>
        /// <param name="weight">Test player weight</param>
        /// <returns>Player builder object</returns>
        public CreatePlayerDtoBuilder WithWeight(short? weight)
        {
            _player.Weight = weight;
            return this;
        }
        /// <summary>
        /// Builds test player
        /// </summary>
        /// <returns>Test player</returns>
        public CreatePlayerDto Build()
        {
            return _player;
        }
    }
}
