namespace VolleyManagement.Services
{
    using System;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Dal.Contracts;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks whether user data is unique or not.
        /// </summary>
        /// <param name="newUser">user to edit or create</param>
        /// <returns>true, if name is unique</returns>
        public bool IsUserUnique(User newUser)
        {
            var userName = _userRepository.FindWhere(t => t.UserName == newUser.UserName
                && t.Id != newUser.Id).FirstOrDefault();

            var userEmail = _userRepository.FindWhere(t => t.Email == newUser.Email
                && t.Id != newUser.Id).FirstOrDefault();

            return userName == null && userEmail == null;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="userToCreate">A User to create</param>
        public void Create(User userToCreate)
        {
            if (IsUserUnique(userToCreate))
            {
                _userRepository.Add(userToCreate);
                _userRepository.UnitOfWork.Commit();
            }
            else
            {
                throw new ArgumentException(VolleyManagement.Domain.Properties.Resources.UserNameMustBeUnique);
            }
        }

        /// <summary>
        /// Finds a User by id
        /// </summary>
        /// <param name="id">id for search</param>
        /// <returns>A found User</returns>
        public User FindById(int id)
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
    }
}