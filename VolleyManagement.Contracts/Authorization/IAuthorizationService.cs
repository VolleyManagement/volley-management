namespace VolleyManagement.Contracts.Authorization
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.RolesAggregate;

    /// <summary>
    /// Provides way to check permissions for particular operation
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Checks if specified operation is allowed for user
        /// </summary>
        /// <param name="operation">Operation to check</param>
        void CheckAccess(AuthOperation operation);

        /// <summary>
        /// Returns an instance of <see cref="AllowedOperations"/> class, which allows to check permissions
        /// to specified list of operations
        /// </summary>
        /// <param name="requestedOperations">Operations to check</param>
        /// <returns>An instance of <see cref="AllowedOperations"/> class</returns>
        AllowedOperations GetAllowedOperations(List<AuthOperation> requestedOperations);

        /// <summary>
        /// Returns an instance of <see cref="AllowedOperations"/> class, which allows to check permissions
        /// to specified operation
        /// </summary>
        /// <param name="requestedOperation">Operation to check</param>
        /// <returns>An instance of <see cref="AllowedOperations"/> class</returns>
        AllowedOperations GetAllowedOperations(AuthOperation requestedOperation);
    }
}
