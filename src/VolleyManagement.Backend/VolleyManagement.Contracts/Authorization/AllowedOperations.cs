namespace VolleyManagement.Contracts.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.RolesAggregate;

    /// <summary>
    /// Provides the way to check if specified operation is allowed for user
    /// </summary>
    public class AllowedOperations
    {
        private readonly ICollection<AuthOperation> _allowedOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllowedOperations"/> class
        /// </summary>
        /// <param name="allowedOperations">List of operations which should be checked for accessibility</param>
        public AllowedOperations(ICollection<AuthOperation> allowedOperations)
        {
            _allowedOperations = allowedOperations ?? throw new ArgumentNullException("Allowed operations list shouldn't be null!");
        }

        /// <summary>
        /// Returns the flag - is specified operation is allowed for user
        /// </summary>
        /// <param name="operation">Operation to check</param>
        /// <returns>Sign if operation is allowed</returns>
        public bool IsAllowed(AuthOperation operation)
        {
            return _allowedOperations.Contains(operation);
        }
    }
}
