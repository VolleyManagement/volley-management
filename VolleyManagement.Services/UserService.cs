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
        /// <summary>
        /// Holds UserRepository instance
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// True if user name is unique
        /// </summary>
        private bool _isUserNameUnique;

        /// <summary>
        /// True if user email is unique
        /// </summary>
        private bool _isUserEmailUnique;

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
        public IQueryable<User> GetAll()
        {
            return _userRepository.FindAll();
        }

        /// <summary>
        /// Checks whether user data is unique or not.
        /// </summary>
        /// <param name="newUser">user to validate</param>
        public void IsUserUnique(User newUser)
        {
            _isUserNameUnique = true;
            _isUserEmailUnique = true;
            CheckUserDataUniqueness(newUser);
            ThrowExceptionForUserData();
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userToCreate">A User to create</param>
        public void Create(User userToCreate)
        {
            IsUserUnique(userToCreate);
            userToCreate.Password = Crypto.HashPassword(userToCreate.Password);
            _userRepository.Add(userToCreate);
            _userRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Finds a User by id
        /// </summary>
        /// <param name="id">id for search</param>
        /// <returns>A found User</returns>
        public User FindById(int id)
        {
            var user = _userRepository.FindWhere(t => t.Id == id).Single();
            return user;
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
        /// Checks whether user data is unique or not
        /// </summary>
        /// <param name="newUser">New user</param>
        private void CheckUserDataUniqueness(User newUser)
        {
            var userDuplicatesList = _userRepository.FindWhere(u => u.Id != newUser.Id &&
                (u.Email == newUser.Email || u.UserName == newUser.UserName)).ToList();

            if (userDuplicatesList != null)
            {
                foreach (var userDuplicate in userDuplicatesList)
                {
                    if (userDuplicate.UserName.Equals(newUser.UserName))
                    {
                        _isUserNameUnique = false;
                    }

                    if (userDuplicate.Email.Equals(newUser.Email))
                    {
                        _isUserEmailUnique = false;
                    }

                    if (!_isUserNameUnique && !_isUserEmailUnique)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Method throws specific exception for invalid user data
        /// </summary>
        private void ThrowExceptionForUserData()
        {
            if (_isUserNameUnique && _isUserEmailUnique)
            {
                return;
            }
            else if (!_isUserNameUnique && !_isUserEmailUnique)
            {
                throw new ArgumentException(Resources.UserNameAndEmailMustBeUnique);
            }
            else if (!_isUserNameUnique)
            {
                throw new ArgumentException(Resources.UserNameMustBeUnique);
            }
            else if (!_isUserEmailUnique)
            {
                throw new ArgumentException(Resources.UserEmailMustBeUnique);
            }
        }
    }
}