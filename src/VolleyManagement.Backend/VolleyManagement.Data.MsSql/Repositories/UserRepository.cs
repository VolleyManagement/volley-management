namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Contracts;
    using Domain.UsersAggregate;
    using Entities;
    using Mappers;

    /// <summary>
    /// Defines implementation of the IUserRepository contract.
    /// </summary>
    internal class UserRepository : IUserRepository
    {
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
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Adds new user.
        /// </summary>
        /// <param name="newEntity">The user for adding.</param>
        public void Add(User newEntity)
        {
            var newUser = new UserEntity();
            DomainToDal.Map(newUser, newEntity);
            _unitOfWork.Context.Users.Add(newUser);
            _unitOfWork.Commit();
            newEntity.Id = newUser.Id;
        }

        /// <summary>
        /// Updates specified user.
        /// </summary>
        /// <param name="updatedEntity">Updated user.</param>
        public void Update(User updatedEntity)
        {
            var userToUpdate = _unitOfWork.Context.Users.Single(t => t.Id == updatedEntity.Id);
            DomainToDal.Map(userToUpdate, updatedEntity);
            UpdateUserProviders((List<LoginInfoEntity>)userToUpdate.LoginProviders);
        }

        /// <summary>
        /// Removes user by id.
        /// </summary>
        /// <param name="id">The id of user to remove.</param>
        public void Remove(int id)
        {
            throw new NotSupportedException();
        }

        private void UpdateUserProviders(List<LoginInfoEntity> providers)
        {
            for (int i = 0; i < providers.Count; i++)
            {
                string loginProviderName = providers[i].LoginProvider;
                string providerKey = providers[i].ProviderKey;
                var existProvider = _unitOfWork.Context.LoginProviders.Where(
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
