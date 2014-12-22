namespace VolleyManagement.UnitTests.Services.UserService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.Users;

    /// <summary>
    /// Generates a test data
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class UserServiceTestFixture
    {
        /// <summary>
        /// Holds collection of users
        /// </summary>
        private IList<User> _users = new List<User>();

        /// <summary>
        /// Adds users to collection
        /// </summary>
        /// <returns>Builder object with collection of users</returns>
        public UserServiceTestFixture TestUsers()
        {
            _users.Add(new User()
            {
                Id = 1,
                UserName = "testLogin1",
                FullName = "Test Name 1",
                Email = "test1@gmail.com",
                Password = "abc111",
                CellPhone = "+38(050)0000001"
            });
            _users.Add(new User()
            {
                Id = 2,
                UserName = "testLogin2",
                FullName = "Test Name 2",
                Email = "test2@gmail.com",
                Password = "abc222",
                CellPhone = "+38(050)0000002"
            });
            _users.Add(new User()
            {
                Id = 3,
                UserName = "testLogin3",
                FullName = "Test Name 3",
                Email = "test3@gmail.com",
                Password = "abc333",
                CellPhone = "+38(050)0000003"
            });
            return this;
        }

        /// <summary>
        /// Add user to collection.
        /// </summary>
        /// <param name="newUser">User to add.</param>
        /// <returns>Builder object with collection of users.</returns>
        public UserServiceTestFixture AddUser(User newUser)
        {
            _users.Add(newUser);
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>User collection</returns>
        public IList<User> Build()
        {
            return _users;
        }
    }
}
