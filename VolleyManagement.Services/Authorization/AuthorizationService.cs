namespace VolleyManagement.Services.Authorization
{
    using System;
    using System.Collections.Generic;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.RolesAggregate;

    public class AuthorizationService: IAuthorizationService
    {
        private AuthOperationsVerifier _verifier;
        private int _userId;
        private IQuery<List<AppOperations>, FindByIdCriteria> _getOperationsQuery;

        public AuthorizationService(IQuery<List<AppOperations>, FindByIdCriteria> getOperationsQuery)
        {
            if (getOperationsQuery == null)
            {
                throw new ArgumentException("getFeaturesQuery");
            }
            this._getOperationsQuery = getOperationsQuery;
        }

        public bool CheckAccess(AppOperations operation)
        {
            var verifier = GetAuthOperationsVerifier();
            return verifier.IsAllowed(operation);
        }
                
        public void SetUser(int userId)
        {
            this._userId = userId;
        }

        public IAuthOperationsVerifier GetAuthOperationsVerifier()
        {
            CheckServiceState();
            if (this._verifier == null)
            {
                UpdateVerifier();
            }

            return this._verifier;
        }

        private void UpdateVerifier()
        {
            var operations = this._getOperationsQuery.Execute(new FindByIdCriteria() { Id = this._userId });
            this._verifier = new AuthOperationsVerifier(operations);
        }

        private void CheckServiceState()
        {
            if (this._userId == 0)
            {
                throw new InvalidOperationException("User is not set to authorization service!");
            }
        }
    }
}
