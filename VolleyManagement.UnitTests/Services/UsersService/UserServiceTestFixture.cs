namespace VolleyManagement.UnitTests.Services.UsersService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Domain.UsersAggregate;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class UserServiceTestFixture
    {
        /// <summary>
        /// Holds collection of users
        /// </summary>
        private List<User> _users = new List<User>();

        /// <summary>
        /// Adds tournaments to collection
        /// </summary>
        /// <returns>Builder object with collection of tournaments</returns>
        public UserServiceTestFixture TestUsers()
        {
            _users.Add(new User()
            {
                Id = 1,
                UserName = "Player1",
                Email = "example@i.ua",
                PersonName = "Nick",
                PhoneNumber = "068-11-22-333",
                LoginProviders = new List<LoginProviderInfo>
                {
                    new LoginProviderInfo
                    {
                        LoginProvider = "Google",
                        ProviderKey = "11111111111111"
                    }
                },
                Roles = new List<Role>
                {
                      new Role
                    {
                        Id = 1,
                        Name = "Administrator"
                    }
                }
            });
            _users.Add(new User()
            {
                Id = 2,
                UserName = "Player2",
                Email = "example1@i.ua",
                PersonName = "Alice",
                PhoneNumber = "045-41-22-453",
                LoginProviders = new List<LoginProviderInfo>
                {
                    new LoginProviderInfo
                    {
                        LoginProvider = "Google",
                        ProviderKey = "222222222222"
                    }
                },
                Roles = new List<Role>
                {
                      new Role
                    {
                        Id = 2,
                        Name = "TournamentAdministrator"
                    }
                }
            });

            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Users collection</returns>
        public List<User> Build()
        {
            return _users;
        }
    }
}
