﻿namespace VolleyManagement.Services.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Queries.Common;
    using Domain.RolesAggregate;

    /// <summary>
    /// Provides way to check permissions for particular operation
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ICurrentUserService _userService;
        private readonly IQuery<ICollection<AuthOperation>, FindByUserIdCriteria> _getOperationsQuery;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationService"/> class
        /// </summary>
        /// <param name="userService">Service to get information about user</param>
        /// <param name="getOperationsQuery">Implementation of authorization queries object</param>
        public AuthorizationService(
            ICurrentUserService userService,
            IQuery<ICollection<AuthOperation>, FindByUserIdCriteria> getOperationsQuery)
        {
            if (userService == null)
            {
                throw new ArgumentException("userService");
            }

            if (getOperationsQuery == null)
            {
                throw new ArgumentException("getFeaturesQuery");
            }

            _userService = userService;
            _getOperationsQuery = getOperationsQuery;
        }

        #endregion

        /// <summary>
        /// Checks if specified operation is allowed for user
        /// </summary>
        /// <param name="operation">Operation to check</param>
        public void CheckAccess(AuthOperation operation)
        {
            var allowedOperations = GetAllUserOperations();
            if (!allowedOperations.Contains(operation))
            {
                throw new AuthorizationException();
            }
        }

        /// <summary>
        /// Returns an instance of <see cref="AllowedOperations"/> class, which allows to check permissions
        /// to specified list of operations
        /// </summary>
        /// <param name="requestedOperations">Operations to check</param>
        /// <returns>An instance of <see cref="AllowedOperations"/> class</returns>
        public AllowedOperations GetAllowedOperations(ICollection<AuthOperation> requestedOperations)
        {
            if (requestedOperations == null)
            {
                throw new ArgumentNullException(nameof(requestedOperations), "Requested operation shouldn't be null!");
            }

            var data = GetAllUserOperations()
                       .Where(op => requestedOperations.Contains(op))
                       .ToList();

            return new AllowedOperations(data);
        }

        /// <summary>
        /// Returns an instance of <see cref="AllowedOperations"/> class, which allows to check permissions
        /// to specified operation
        /// </summary>
        /// <param name="requestedOperation">Operation to check</param>
        /// <returns>An instance of <see cref="AllowedOperations"/> class</returns>
        public AllowedOperations GetAllowedOperations(AuthOperation requestedOperation)
        {
            if (requestedOperation == null)
            {
                throw new ArgumentNullException(nameof(requestedOperation), "Requested operation shouldn't be null!");
            }

            return GetAllowedOperations(new List<AuthOperation> { requestedOperation });
        }

        #region Private

        private ICollection<AuthOperation> GetAllUserOperations()
        {
            var userId = _userService.GetCurrentUserId();
            return _getOperationsQuery.Execute(new FindByUserIdCriteria { UserId = userId });
        }

        #endregion
    }
}
