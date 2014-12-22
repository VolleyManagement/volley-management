namespace VolleyManagement.UnitTests.Services.UserService
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.Users;

    /// <summary>
    /// Builder for test users
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class UserBuilder
    {
        /// <summary>
        /// Holds test user instance
        /// </summary>
        private User _user;

        /// <summary>
        ///  Initializes a new instance of the <see cref="UserBuilder"/> class
        /// </summary>
        public UserBuilder()
        {
            _user = new User
            {
                Id = 1,
                UserName = "UserName",
                Email = "test@gmail.com",
                Password = "abc123",
                CellPhone = "1234567890",
                FullName = "Full Name"
            };
        }

        /// <summary>
        /// Sets test user id
        /// </summary>
        /// <param name="id">test user id</param>
        /// <returns>User builder object</returns>
        public UserBuilder WithId(int id)
        {
            this._user.Id = id;
            return this;
        }

        /// <summary>
        /// Sets test user name
        /// </summary>
        /// <param name="userName">test user name</param>
        /// <returns>User builder object</returns>
        public UserBuilder WithUserName(string userName)
        {
            this._user.UserName = userName;
            return this;
        }

        /// <summary>
        /// Sets test user FullName
        /// </summary>
        /// <param name="fullName">test user FullName</param>
        /// <returns>User builder object</returns>
        public UserBuilder WithFullName(string fullName)
        {
            this._user.FullName = fullName;
            return this;
        }

        /// <summary>
        /// Sets test user email
        /// </summary>
        /// <param name="email">test user email</param>
        /// <returns>User builder object</returns>
        public UserBuilder WithEmail(string email)
        {
            this._user.Email = email;
            return this;
        }

        /// <summary>
        /// Sets test user password
        /// </summary>
        /// <param name="password">test user name</param>
        /// <returns>User builder object</returns>
        public UserBuilder WithPassword(string password)
        {
            this._user.Password = password;
            return this;
        }

        /// <summary>
        /// Sets test user cell phone
        /// </summary>
        /// <param name="cellPhone">test user cell phone</param>
        /// <returns>User builder object</returns>
        public UserBuilder WithCellPhone(string cellPhone)
        {
            this._user.CellPhone = cellPhone;
            return this;
        }

        /// <summary>
        /// Builds test user
        /// </summary>
        /// <returns>Test user</returns>
        public User Build()
        {
            return this._user;
        }
    }
}