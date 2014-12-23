namespace VolleyManagement.Dal.MsSql.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Text;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.MsSql.Mappers;
    using Dal = VolleyManagement.Dal.MsSql;
    using Domain = VolleyManagement.Domain.Users;

    /// <summary>
    /// Defines implementation of the IUserRepository contract.
    /// </summary>
    internal class UserRepository : IUserRepository
    {
        /// <summary>
        /// Holds object set of DAL users.
        /// </summary>
        private readonly ObjectSet<Dal.User> _dalUsers;

        /// <summary>
        /// Holds UnitOfWork instance.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public UserRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dalUsers = unitOfWork.Context.CreateObjectSet<Dal.User>();
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>Collection of domain users.</returns>
        public IQueryable<Domain.User> FindAll()
        {
            return _dalUsers.Select(u => new Domain.User
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
        public IQueryable<Domain.User> FindWhere(System.Linq.Expressions.Expression<Func<Domain.User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds new user.
        /// </summary>
        /// <param name="newEntity">The user for adding.</param>
        public void Add(Domain.User newEntity)
        {
            Dal.User newUser = DomainToDal.Map(newEntity);
            _dalUsers.AddObject(newUser);
            _unitOfWork.Commit();
            newEntity.Id = newUser.Id;
        }

        /// <summary>
        /// Updates specified user.
        /// </summary>
        /// <param name="oldEntity">The user to update.</param>
        public void Update(Domain.User oldEntity)
        {
            throw new NotImplementedException();
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
