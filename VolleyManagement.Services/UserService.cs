namespace VolleyManagement.Services
{
    using System;
    using System.Linq;
    using System.Web.Helpers;
    using VolleyManagement.Contracts;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Properties;
    using VolleyManagement.Domain.Users;

    /// <summary>
    /// Defines UserService
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class
        /// </summary>
        /// <param name="userRepository">The user repository</param>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Method to get all users
        /// </summary>
        /// <returns>All users</returns>
        public IQueryable<User> Get()
        {
            return _userRepository.Find();
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userToCreate">A User to create</param>
        public void Create(User userToCreate)
        {
            CheckUserDataUniqueness(userToCreate);
            userToCreate.Password = Crypto.HashPassword(userToCreate.Password);
            _userRepository.Add(userToCreate);
            _userRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Finds a User by id
        /// </summary>
        /// <param name="id">id for search</param>
        /// <returns>A found User</returns>
        public User Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit user
        /// </summary>
        /// <param name="userToEdit">User to edit</param>
        public void Edit(User userToEdit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete user by id.
        /// </summary>
        /// <param name="id">The id of user to delete.</param>
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks whether user data is unique or not.
        /// </summary>
        /// <param name="newUser">New user</param>
        private void CheckUserDataUniqueness(User newUser)
        {
            var userDuplicatesList = _userRepository.FindWhere(u => u.Id != newUser.Id &&
                (u.Email == newUser.Email || u.UserName == newUser.UserName)).Select(u => new { u.UserName, u.Email });

            var userDuplicateName = userDuplicatesList.FirstOrDefault(ud => ud.UserName.Equals(newUser.UserName));
            var userDuplicateEmail = userDuplicatesList.FirstOrDefault(ud => ud.Email.Equals(newUser.Email));

            if (userDuplicateName != null && userDuplicateEmail != null)
            {
                throw new ArgumentException(Resources.UserNameAndEmailMustBeUnique);
            }
            else if (userDuplicateEmail != null)
            {
                throw new ArgumentException(Resources.UserEmailMustBeUnique);
            }
            else if (userDuplicateName != null)
            {
                throw new ArgumentException(Resources.UserNameMustBeUnique);
            }
        }
    }
}