namespace VolleyManagement.UnitTests.Services.UserService
{
    using Domain.UsersAggregate;

    internal class BlockUserBuilder
    {
        private readonly User _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockUserBuilder"/> class.
        /// </summary>
        public BlockUserBuilder()
        {
            this._user = new User
            {
                Id = 1,
                UserName = "TestName",
                PersonName = "TestFullName",
                Email = "test@test.test",
                IsUserBlocked = false
            };
        }

        /// <summary>
        /// Sets the identifier of the user.
        /// </summary>
        /// <param name="id">Identifier of the user.</param>
        /// <returns>User object.</returns>
        public BlockUserBuilder WithId(int id)
        {
            this._user.Id = id;
            return this;
        }

        /// <summary>
        /// Sets the user email.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <returns>User object.</returns>
        public BlockUserBuilder WithEmail(string email)
        {
            this._user.Email = email;
            return this;
        }

        /// <summary>
        /// Sets the user name.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>User object.</returns>
        public BlockUserBuilder WithName(string userName)
        {
            this._user.UserName = userName;
            return this;
        }

        /// <summary>
        /// Sets the person name.
        /// </summary>
        /// <param name="personName">Person name</param>
        /// <returns>User object.</returns>
        public BlockUserBuilder WithPersonName(string personName)
        {
            this._user.PersonName = personName;
            return this;
        }

        /// <summary>
        /// Sets the user block status.
        /// </summary>
        /// <param name="blockStatus">User block status</param>
        /// <returns>User object.</returns>
        public BlockUserBuilder WithBlockStatus(bool blockStatus)
        {
            this._user.IsUserBlocked = blockStatus;
            return this;
        }

        /// <summary>
        /// Builds test user.
        /// </summary>
        /// <returns>Test user.</returns>
        public User Build()
        {
            return this._user;
        }
    }
}
