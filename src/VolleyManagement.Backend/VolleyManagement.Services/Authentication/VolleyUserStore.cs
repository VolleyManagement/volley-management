namespace VolleyManagement.Services.Authentication
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts.Authentication;
    using Contracts.Authentication.Models;
    using Data.Contracts;
    using Data.Queries.Common;
    using Data.Queries.User;
    using Domain.UsersAggregate;
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Stores Volley Users data
    /// </summary>
    public sealed class VolleyUserStore : IVolleyUserStore
    {
        #region Fields

        private readonly IUserRepository _userRepository;

        private readonly IQueryAsync<User, FindByIdCriteria> _getByIdQuery;

        private readonly IQueryAsync<User, FindByNameCriteria> _getByNameQuery;

        private readonly IQueryAsync<User, FindByEmailCriteria> _getByEmailQuery;

        private readonly IQueryAsync<User, FindByLoginInfoCriteria> _getByLoginInfoQuery;

        #endregion

        #region ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyUserStore"/> class.
        /// </summary>
        /// <param name="userRepository"> The user repository. </param>
        /// <param name="getByIdQuery"> Get By Id object query</param>
        /// <param name="getByNameQuery"> Get By Name object query</param>
        /// <param name="getByEmailQuery"> Get By Email object query</param>
        /// <param name="getByLoginInfoQuery"> Get By Login information object query</param>
        public VolleyUserStore(
            IUserRepository userRepository,
            IQueryAsync<User, FindByIdCriteria> getByIdQuery,
            IQueryAsync<User, FindByNameCriteria> getByNameQuery,
            IQueryAsync<User, FindByEmailCriteria> getByEmailQuery,
            IQueryAsync<User, FindByLoginInfoCriteria> getByLoginInfoQuery)
        {
            _userRepository = userRepository;
            _getByIdQuery = getByIdQuery;
            _getByNameQuery = getByNameQuery;
            _getByEmailQuery = getByEmailQuery;
            _getByLoginInfoQuery = getByLoginInfoQuery;
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Disposes current instance
        /// </summary>
        public void Dispose()
        {
            // Do Nothing - there are no resources to dispose explicitly
        }

        #endregion

        #region IUserStore

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="user">User to create</param>
        /// <returns>async Task</returns>
        public async Task CreateAsync(UserModel user)
        {
            var domainUser = CreateDomainUser(user);

            _userRepository.Add(domainUser);
            user.Id = domainUser.Id;

            await _userRepository.UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// Updates existing user
        /// </summary>
        /// <param name="user">User to update</param>
        /// <returns>Task to await</returns>
        public async Task UpdateAsync(UserModel user)
        {
            var domainUser = CreateDomainUser(user);

            _userRepository.Update(domainUser);

            await _userRepository.UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// Deletes existing user
        /// </summary>
        /// <param name="user">User to delete</param>
        /// <returns>Task to await</returns>
        public async Task DeleteAsync(UserModel user)
        {
            _userRepository.Remove(user.Id);

            await _userRepository.UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// Retrieves user by given Id
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns> Found User </returns>
        public async Task<UserModel> FindByIdAsync(int userId)
        {
            var criteria = new FindByIdCriteria { Id = userId };
            var user = await _getByIdQuery.ExecuteAsync(criteria);

            return CreateUserModel(user);
        }

        /// <summary>
        /// Retrieves user by given Name
        /// </summary>
        /// <param name="userName">Name of the user</param>
        /// <returns> Found User </returns>
        public async Task<UserModel> FindByNameAsync(string userName)
        {
            var criteria = new FindByNameCriteria { Name = userName };
            var user = await _getByNameQuery.ExecuteAsync(criteria);

            return CreateUserModel(user);
        }

        #endregion

        #region IUserEmailStore

        /// <summary>
        /// Sets user email
        /// </summary>
        /// <param name="user"> The user. </param>
        /// <param name="email"> The email. </param>
        /// <returns> Task to await </returns>
        public Task SetEmailAsync(UserModel user, string email)
        {
            user.Email = email;
            return Task.FromResult(true);
        }

        /// <summary>
        /// Gets email of the user
        /// </summary>
        /// <param name="user">User to retrieve email for</param>
        /// <returns>Email string</returns>
        public Task<string> GetEmailAsync(UserModel user)
        {
            return Task.FromResult(user.Email);
        }

        /// <summary>
        /// Returns true if the user email is confirmed
        /// </summary>
        /// <param name="user">User to look at</param>
        /// <returns> whether or not email is confirmed </returns>
        public Task<bool> GetEmailConfirmedAsync(UserModel user)
        {
            // We don't have own Identity storage so we don't need to confirm anything
            return Task.FromResult(false);
        }

        /// <summary>
        /// Sets confirmed value for the email
        /// </summary>
        /// <param name="user">User to look at</param>
        /// <param name="confirmed">Is confirmed</param>
        /// <returns> whether or not email is confirmed </returns>
        public Task SetEmailConfirmedAsync(UserModel user, bool confirmed)
        {
            // do nothing - we do not support this feature
            return Task.FromResult(false);
        }

        /// <summary>
        /// Retrieves user by given Email
        /// </summary>
        /// <param name="email">Email of the user</param>
        /// <returns> Found User </returns>
        public async Task<UserModel> FindByEmailAsync(string email)
        {
            var criteria = new FindByEmailCriteria { Email = email };
            var user = await _getByEmailQuery.ExecuteAsync(criteria);

            return CreateUserModel(user);
        }

        #endregion

        #region IUserLoginStore

        /// <summary>
        /// Adds User Login information for user
        /// </summary>
        /// <param name="user">User to add login information</param>
        /// <param name="login">Login information</param>
        /// <returns>Task to await</returns>
        public Task AddLoginAsync(UserModel user, UserLoginInfo login)
        {
            var loginInfo = new LoginProviderModel
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };
            user.Logins.Add(loginInfo);
            return Task.FromResult(true);
        }

        /// <summary>
        /// Removes User Login information for user
        /// </summary>
        /// <param name="user">User to remove login information</param>
        /// <param name="login">Login information</param>
        /// <returns>Task to await</returns>
        public Task RemoveLoginAsync(UserModel user, UserLoginInfo login)
        {
            var loginToDelete = user.Logins.Find(l =>
                                    l.LoginProvider == login.LoginProvider
                                    && l.ProviderKey == login.ProviderKey);
            if (loginToDelete != null)
            {
                user.Logins.Remove(loginToDelete);
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Gets all Login information for user
        /// </summary>
        /// <param name="user">User to get login information</param>
        /// <returns>Task to await</returns>
        public Task<IList<UserLoginInfo>> GetLoginsAsync(UserModel user)
        {
            return Task.FromResult((IList<UserLoginInfo>)((List<LoginProviderModel>)(user.Logins))
                       .ConvertAll(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey)));
        }

        /// <summary>
        /// Retrieves user by given Login Information
        /// </summary>
        /// <param name="login">User login information</param>
        /// <returns> Found User </returns>
        public async Task<UserModel> FindAsync(UserLoginInfo login)
        {
            var criteria = new FindByLoginInfoCriteria
            {
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider
            };
            var user = await _getByLoginInfoQuery.ExecuteAsync(criteria);

            return CreateUserModel(user);
        }

        #endregion

        #region Helpers

        private User CreateDomainUser(UserModel user)
        {
            var domainUser = new User();
            Map(to: domainUser, from: user);
            return domainUser;
        }

        private UserModel CreateUserModel(User user)
        {
            UserModel result = null;

            if (user != null)
            {
                result = new UserModel();
                Map(to: result, from: user);
            }

            return result;
        }

        #endregion

        #region Mapper

        private void Map(User to, UserModel from)
        {
            to.Id = from.Id;
            to.UserName = from.UserName;
            to.Email = from.Email;
            to.PersonName = from.PersonName;
            to.PhoneNumber = from.PhoneNumber;
            to.LoginProviders = ((List<LoginProviderModel>)from.Logins).ConvertAll(
                                l => new LoginProviderInfo
                                {
                                    ProviderKey = l.ProviderKey,
                                    LoginProvider = l.LoginProvider
                                });
        }

        private void Map(UserModel to, User from)
        {
            to.Id = from.Id;
            to.UserName = from.UserName;
            to.Email = from.Email;
            to.PersonName = from.PersonName;
            to.PhoneNumber = from.PhoneNumber;
            to.Logins = from.LoginProviders.Select(
                                l => new LoginProviderModel
                                {
                                    LoginProvider = l.LoginProvider,
                                    ProviderKey = l.ProviderKey
                                })
                                         .ToList();
        }

        #endregion
    }
}