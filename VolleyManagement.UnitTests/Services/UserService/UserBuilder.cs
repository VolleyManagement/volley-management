﻿namespace VolleyManagement.UnitTests.Services.UserComparer
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
                UserName = "testLogin",
                Email = "test@gmail.com",
                Password = "abc123"
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
        /// Builds test user
        /// </summary>
        /// <returns>Test user</returns>
        public User Build()
        {
            return this._user;
        }
    }
}
