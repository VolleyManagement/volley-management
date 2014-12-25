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
        /// Checks whether user name is unique or not.
        /// </summary>
        /// <param name="newUser">user to edit or create</param>
        /// <returns>true, if name is unique</returns>
        public bool IsUserNameUnique(User newUser)
        {
            var userName = _userRepository.FindWhere(t => t.UserName == newUser.UserName
                && t.Id != newUser.Id).FirstOrDefault();

            return userName == null;
        }

        /// <summary>
        /// Checks whether user email is unique or not.
        /// </summary>
        /// <param name="newUser">user to edit or create</param>
        /// <returns>true, if email is unique</returns>
        public bool IsUserEmailUnique(User newUser)
        {
            var userEmail = _userRepository.FindWhere(t => t.Email == newUser.Email
                && t.Id != newUser.Id).FirstOrDefault();

            return userEmail == null;
        }

        /// <summary>
        /// Checks whether user data is unique or not.
        /// </summary>
        /// <param name="newUser">user to validate</param>
        public void IsUserUnique(User newUser)
        {
            bool isNameUnique = IsUserNameUnique(newUser);
            bool isEmailUnique = IsUserEmailUnique(newUser);

            if (!isNameUnique && !isEmailUnique)
            {
                throw new ArgumentException(Resources.UserNameAndEmailMustBeUnique);
            }
            else if (!isNameUnique)
            {
                throw new ArgumentException(Resources.UserNameMustBeUnique);
            }
            else if (!isEmailUnique)
            {
                throw new ArgumentException(Resources.UserEmailMustBeUnique);
            }
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
            IsUserNameUnique(userToEdit);
            userToEdit.Password = Crypto.HashPassword(userToEdit.Password);
            _userRepository.Update(userToEdit);
            _userRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Delete user by id.
        /// </summary>
        /// <param name="id">The id of user to delete.</param>
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}