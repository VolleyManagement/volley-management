namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.MsSql.Mappers;
    using VolleyManagement.Domain.UsersAggregate;

    /// <summary>
    /// Defines implementation of the IUserRepository contract.
    /// </summary>
    internal class UserRepository : IUserRepository
    {
        /// <summary>
        /// Holds object set of DAL users.
        /// </summary>
        private readonly DbSet<UserEntity> _dalUsers;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly VolleyUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalUsers = _unitOfWork.Context.Users;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return this._unitOfWork; }
        }

        /// <summary>
        /// Adds new user.
        /// </summary>
        /// <param name="newModel">The user for adding.</param>
        public void Add(User newModel)
        {
            var newUser = new UserEntity();
            DomainToDal.Map(newUser, newModel);
            this._dalUsers.Add(newUser);
            this._unitOfWork.Commit();
            newModel.Id = newUser.Id;
        }

        /// <summary>
        /// Updates specified user.
        /// </summary>
        /// <param name="updatedModel">Updated user.</param>
        public void Update(User updatedModel)
        {
            var userToUpdate = this._dalUsers.Single(t => t.Id == updatedModel.Id);
            DomainToDal.Map(userToUpdate, updatedModel);
        }

        /// <summary>
        /// Removes user by id.
        /// </summary>
        /// <param name="id">The id of user to remove.</param>
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
