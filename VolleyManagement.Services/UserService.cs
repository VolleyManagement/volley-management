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
            IsUserUnique(userToEdit);
            userToEdit.Password = Crypto.HashPassword(userToEdit.Password);
            _userRepository.Update(userToEdit);
            _userRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Checks whether user data is unique or not.
        /// </summary>
        /// <param name="newUser">user to validate</param>
        private void IsUserUnique(User newUser)
        {
            bool isUserNameUnique = true;
            bool isUserEmailUnique = true;
            CheckUserDataUniqueness(newUser, ref isUserNameUnique, ref isUserEmailUnique);
            ThrowExceptionForUserData(isUserNameUnique, isUserEmailUnique);
        }

        /// <summary>
        /// Checks whether user data is unique or not.
        /// </summary>
        /// <param name="newUser">New user</param>
        /// <param name="isUserNameUnique">True if user name is unique</param>
        /// <param name="isUserEmailUnique">True if user email is unique</param>
        private void CheckUserDataUniqueness(User newUser, ref bool isUserNameUnique, ref bool isUserEmailUnique)
        {
            var userDuplicatesList = _userRepository.FindWhere(u => u.Id != newUser.Id &&
                (u.Email == newUser.Email || u.UserName == newUser.UserName))
                .Select(u => new { u.UserName, u.Email }).ToList();

            if (userDuplicatesList != null)
            {
                foreach (var userDuplicate in userDuplicatesList)
                {
                    if (userDuplicate.UserName.Equals(newUser.UserName))
                    {
                        isUserNameUnique = false;
                    }

                    if (userDuplicate.Email.Equals(newUser.Email))
                    {
                        isUserEmailUnique = false;
                    }

                    if (!isUserNameUnique && !isUserEmailUnique)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Method throws specific exception for invalid user data
        /// </summary>
        /// <param name="isUserNameUnique">True if user name is unique</param>
        /// <param name="isUserEmailUnique">True if user email is unique</param>
        private void ThrowExceptionForUserData(bool isUserNameUnique, bool isUserEmailUnique)
        {
            if (!isUserNameUnique && !isUserEmailUnique)
            {
                throw new ArgumentException(Resources.UserNameAndEmailMustBeUnique);
            }
            else if (!isUserNameUnique)
            {
                throw new ArgumentException(Resources.UserNameMustBeUnique);
            }
            else if (!isUserEmailUnique)
            {
                throw new ArgumentException(Resources.UserEmailMustBeUnique);
            }
        }
    }
}