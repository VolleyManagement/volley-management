namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Collections.Generic;
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
        /// Holds object set of DAL login providers.
        /// </summary>
        private readonly DbSet<LoginInfoEntity> _dalLoginProviders;

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
            _dalLoginProviders = _unitOfWork.Context.LoginProviders;
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
        /// <param name="newEntity">The user for adding.</param>
        public void Add(User newEntity)
        {
            var newUser = new UserEntity();
            DomainToDal.Map(newUser, newEntity);
            this._dalUsers.Add(newUser);
            this._unitOfWork.Commit();
            newEntity.Id = newUser.Id;
        }

        /// <summary>
        /// Updates specified user.
        /// </summary>
        /// <param name="oldEntity">The user to update.</param>
        public void Update(User oldEntity)
        {
            var userToUpdate = this._dalUsers.Single(t => t.Id == oldEntity.Id);
            DomainToDal.Map(userToUpdate, oldEntity);
            UpdateUserProviders((List<LoginInfoEntity>)userToUpdate.LoginProviders);
        }

        /// <summary>
        /// Removes user by id.
        /// </summary>
        /// <param name="id">The id of user to remove.</param>
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        private void UpdateUserProviders(List<LoginInfoEntity> providers)
        {
            for (int i = 0; i < providers.Count; i++)
            {
                string loginProviderName = providers[i].LoginProvider;
                string providerKey = providers[i].ProviderKey;
                var existProvider = _dalLoginProviders.Where(
                                                            dlp =>
                                                            dlp.LoginProvider == loginProviderName
                                                            && dlp.ProviderKey == providerKey)
                                                           .FirstOrDefault();
                if (existProvider != null)
                {
                    providers[i] = existProvider;
                }
            }
        }
    }
}
