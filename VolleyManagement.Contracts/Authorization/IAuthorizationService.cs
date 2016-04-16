namespace VolleyManagement.Contracts.Authorization
{
    using VolleyManagement.Domain.RolesAggregate;

    public interface IAuthorizationService
    {
        void SetUser(int userId);

        bool CheckAccess(AppOperations operation);

        IAuthOperationsVerifier GetAuthOperationsVerifier();
    }
}
