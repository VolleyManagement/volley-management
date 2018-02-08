namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Contracts;
    using Data.Queries.Common;
    using Data.Queries.User;
    using Domain.Dto;
    using Domain.RolesAggregate;
    using Domain.UsersAggregate;
    using Entities;

    /// <summary>
    /// Provides Object Query implementation for Users
    /// </summary>
    public class UserQueries : IQueryAsync<User, FindByIdCriteria>,
                             IQueryAsync<User, FindByNameCriteria>,
                             IQueryAsync<User, FindByEmailCriteria>,
                             IQueryAsync<User, FindByLoginInfoCriteria>,
                             IQuery<List<UserInRoleDto>, FindByRoleCriteria>,
                             IQuery<List<UserInRoleDto>, GetAllCriteria>,
                             IQuery<User, FindByIdCriteria>,
                             IQuery<List<User>, GetAllCriteria>,
                             IQuery<List<User>, UniqueUserCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork"> The unit of work. </param>
        public UserQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds User by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public Task<User> ExecuteAsync(FindByIdCriteria criteria)
        {
            var query = _unitOfWork.Context.Users.Where(u => u.Id == criteria.Id);

            // ToDo: Use Automapper to substitute Select clause
            return query.Select(GetUserMapping()).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds User by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public Task<User> ExecuteAsync(FindByNameCriteria criteria)
        {
            var query = _unitOfWork.Context.Users.Where(u => u.UserName == criteria.Name);

            // ToDo: Use Automapper to substitute Select clause
            return query.Select(GetUserMapping()).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds User by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public Task<User> ExecuteAsync(FindByEmailCriteria criteria)
        {
            var query = _unitOfWork.Context.Users.Where(u => u.Email == criteria.Email);

            // ToDo: Use Automapper to substitute Select clause
            return query.Select(GetUserMapping()).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds User by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public Task<User> ExecuteAsync(FindByLoginInfoCriteria criteria)
        {
            var query = _unitOfWork.Context.LoginProviders
                                           .Where(l => l.ProviderKey == criteria.ProviderKey
                                               && l.LoginProvider == criteria.LoginProvider)
                                           .Select(l => l.User);

            // ToDo: Use Automapper to substitute Select clause
            return query.Select(GetUserMapping()).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds User by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public List<UserInRoleDto> Execute(FindByRoleCriteria criteria)
        {
            var users = _unitOfWork.Context.Roles
                                           .Where(r => r.Id == criteria.RoleId)
                                           .SelectMany(r => r.Users)
                                           .Select(GetUserInRoleMapper())
                                           .ToList();
            return users;
        }

        /// <summary>
        /// Finds User by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public List<UserInRoleDto> Execute(GetAllCriteria criteria)
        {
            var users = _unitOfWork.Context.Users
                                           .Select(GetUserInRoleMapper())
                                           .ToList();

            return users;
        }

        /// <summary>
        /// Finds user by given criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>User entity.</returns>
        public User Execute(FindByIdCriteria criteria)
        {
            return
                _unitOfWork.Context.Users
                .Where(i => i.Id == criteria.Id)
                .Select(GetUserMapping())
                .SingleOrDefault();
        }

        /// <summary>
        /// Finds user by given criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>User entity list.</returns>
        List<User> IQuery<List<User>, GetAllCriteria>.Execute(GetAllCriteria criteria)
        {
            return _unitOfWork.Context.Users.Select(GetUserMapping()).ToList();
        }

        /// <summary>
        /// Finds User by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="User"/>. </returns>
        public List<User> Execute(UniqueUserCriteria criteria)
        {
            var users = _unitOfWork.Context.Roles
                                           .Where(r => r.Id == criteria.RoleId)
                                           .SelectMany(r => r.Users)
                                           .Select(GetUserMapping())
                                           .ToList();
            return users;
        }

        #endregion

        #region Mapping

        private static Expression<Func<UserEntity, User>> GetUserMapping()
        {
            return
                t =>
                new User
                {
                    Id = t.Id,
                    UserName = t.UserName,
                    Email = t.Email,
                    PersonName = t.FullName,
                    PhoneNumber = t.CellPhone,
                    IsBlocked = t.IsBlocked,
                    PlayerId = t.PlayerId,
                    LoginProviders = t.LoginProviders.Select(
                                            l => new LoginProviderInfo
                                            {
                                                ProviderKey = l.ProviderKey,
                                                LoginProvider = l.LoginProvider
                                            }),
                    Roles = t.Roles.Select(r => new Role
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                };
        }

        private static Expression<Func<UserEntity, UserInRoleDto>> GetUserInRoleMapper()
        {
            return u =>
                   new UserInRoleDto
                   {
                       UserId = u.Id,
                       UserName = u.UserName,
                       RoleIds = u.Roles.Select(r => r.Id).ToList()
                   };
        }

        #endregion
    }
}