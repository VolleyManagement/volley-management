namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
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
        private readonly ObjectSet<UserEntity> _dalUsers = null;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly VolleyUnitOfWork _unitOfWork = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public UserRepository(IUnitOfWork unitOfWork)
        {
            // TODO: Change after Identity API integration
            // this._unitOfWork = unitOfWork;
            // this._dalUsers = unitOfWork.Context.CreateObjectSet<UserEntity>();
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return this._unitOfWork; }
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>Collection of domain users.</returns>
        public IQueryable<Domain.Users.User> Find()
        {
            return this._dalUsers.Select(u => new Domain.Users.User
            {
                Id = u.Id,
                FullName = u.FullName,
                UserName = u.UserName,
                Email = u.Email,
                CellPhone = u.CellPhone,
                Password = u.Password
            });
        }

        /// <summary>
        /// Gets specified collection of users.
        /// </summary>
        /// <param name="predicate">Condition to find users.</param>
        /// <returns>Collection of domain users.</returns>
        public IQueryable<Domain.Users.User> FindWhere(System.Linq.Expressions.Expression<Func<Domain.Users.User, bool>> predicate)
        {
            return this.Find().Where(predicate);
        }

        /// <summary>
        /// Adds new user.
        /// </summary>
        /// <param name="newEntity">The user for adding.</param>
        public void Add(Domain.Users.User newEntity)
        {
            UserEntity newUser = DomainToDal.Map(newEntity);
            this._dalUsers.AddObject(newUser);
            this._unitOfWork.Commit();
            newEntity.Id = newUser.Id;
        }

        /// <summary>
        /// Updates specified user.
        /// </summary>
        /// <param name="oldEntity">The user to update.</param>
        public void Update(Domain.Users.User oldEntity)
        {
            var userToUpdate = this._dalUsers.Where(t => t.Id == oldEntity.Id).Single();
            userToUpdate.UserName = oldEntity.UserName;
            userToUpdate.Password = oldEntity.Password;
            userToUpdate.FullName = oldEntity.FullName;
            userToUpdate.Email = oldEntity.Email;
            userToUpdate.CellPhone = oldEntity.CellPhone;
            this._dalUsers.Context.ObjectStateManager.ChangeObjectState(userToUpdate, EntityState.Modified);
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
