namespace VolleyManagement.UnitTests.Services.UserManager
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Contracts.Authentication.Models;

    /// <summary>
    /// Builder for test users
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class UserModelBuilder
    {
        /// <summary>
        /// Holds test user instance
        /// </summary>
        private UserModel _userModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserModelBuilder"/> class
        /// </summary>
        public UserModelBuilder()
        {
            _userModel = new UserModel
            {
                Id = 1,
                UserName = "Player",
                Email = "example@i.ua",
                PersonName = "Eugene",
                PhoneNumber = "068-11-22-333",
                Logins = new List<LoginProviderModel>
                {
                    new LoginProviderModel
                    {
                        LoginProvider = "Google",
                        ProviderKey = "11111111111111"
                    }
                }
            };
        }

        /// <summary>
        /// Sets id of test user
        /// </summary>
        /// <param name="id">Id for test user</param>
        /// <returns>user builder object</returns>
        public UserModelBuilder WithId(int id)
        {
            _userModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets username of test user
        /// </summary>
        /// <param name="username">Username for test user</param>
        /// <returns>User builder object</returns>
        public UserModelBuilder WithUserName(string username)
        {
            _userModel.UserName = username;
            return this;
        }

        /// <summary>
        /// Sets email of test user
        /// </summary>
        /// <param name="email">email for test user</param>
        /// <returns>User builder object</returns>
        public UserModelBuilder WithEmail(string email)
        {
            _userModel.Email = email;
            return this;
        }

        /// <summary>
        /// Sets phone number of test user
        /// </summary>
        /// <param name="phoneNumber">Phone Number for test user</param>
        /// <returns>User builder object</returns>
        public UserModelBuilder WithPhoneNumber(string phoneNumber)
        {
            _userModel.PhoneNumber = phoneNumber;
            return this;
        }

        /// <summary>
        /// Sets person name of test user
        /// </summary>
        /// <param name="personName">Person name for test user</param>
        /// <returns>User builder object</returns>
        public UserModelBuilder WithFullName(string personName)
        {
            _userModel.PersonName = personName;
            return this;
        }

        /// <summary>
        /// Sets login providers of test user
        /// </summary>
        /// <param name="providers">Login providers for test user</param>
        /// <returns>User builder object</returns>
        public UserModelBuilder WithLoginProviders(List<LoginProviderModel> providers)
        {
            _userModel.Logins = providers;
            return this;
        }

        /// <summary>
        /// Builds test user
        /// </summary>
        /// <returns>Test user</returns>
        public UserModel Build()
        {
            return _userModel;
        }
    }
}
