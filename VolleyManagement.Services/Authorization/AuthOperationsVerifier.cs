namespace VolleyManagement.Services.Authorization
{
    using System.Collections.Generic;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.RolesAggregate;

    public class AuthOperationsVerifier : IAuthOperationsVerifier
    {
        private List<AppOperations> _allowedOperations;

        public AuthOperationsVerifier(List<AppOperations> allowedOperations)
        {
            this._allowedOperations = allowedOperations;
        }

        public bool IsAllowed(AppOperations operation)
        {
            return _allowedOperations.Contains(operation);
        }
    }
}
