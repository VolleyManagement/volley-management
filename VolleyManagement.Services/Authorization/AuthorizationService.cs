namespace VolleyManagement.Services.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.RolesAggregate;

    public class AuthorizationService: IAuthorizationService
    {
        private ICurrentUserService _userService;
        private IQuery<List<AppAreaOperation>, FindByIdCriteria> _getOperationsQuery;

        public AuthorizationService(
            ICurrentUserService userService,
            IQuery<List<AppAreaOperation>, FindByIdCriteria> getOperationsQuery)
        {
            if (userService == null)
            {
                throw new ArgumentException("userService");
            }

            if (getOperationsQuery == null)
            {
                throw new ArgumentException("getFeaturesQuery");
            }

            this._userService = userService;
            this._getOperationsQuery = getOperationsQuery;
        }

        public bool CheckAccess(AppAreaOperation operation)
        {
            var allowedOperations = this.GetAllowedOperations();
            return allowedOperations.Select(i => i.Id).Contains(operation.Id);
        }

        public AllowedOperations GetAuthOperationsVerifier(List<AppAreaOperation> requestedOperations)
        {
            var data = from ao in this.GetAllowedOperations()
                       join ro in requestedOperations
                            on ao.Id equals ro.Id
                       select ao;

            return new AllowedOperations(data.ToList());
        }

        public AllowedOperations GetAuthOperationsVerifier(AppAreaOperation requestedOperation)
        {
            return this.GetAuthOperationsVerifier(new List<AppAreaOperation> { requestedOperation });
        }

        #region Private

        private List<AppAreaOperation> GetAllowedOperations()
        {
            var userId = this._userService.GetCurrentUserId();
            return this._getOperationsQuery.Execute(new FindByIdCriteria() { Id = userId });
        }

        #endregion        
    }
}
