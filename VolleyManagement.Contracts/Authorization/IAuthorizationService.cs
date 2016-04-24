namespace VolleyManagement.Contracts.Authorization
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.RolesAggregate;

    public interface IAuthorizationService
    {
        bool CheckAccess(AppAreaOperation operation);

        AllowedOperations GetAuthOperationsVerifier(List<AppAreaOperation> requestedOperations);
        AllowedOperations GetAuthOperationsVerifier(AppAreaOperation requestedOperation);
    }
}
