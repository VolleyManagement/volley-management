namespace VolleyManagement.Contracts.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VolleyManagement.Domain.RolesAggregate;

    public class AllowedOperations
    {
        private List<AppAreaOperation> _allowedOperations;

        public AllowedOperations(List<AppAreaOperation> allowedOperations)
        {
            if (allowedOperations == null)
            {
                throw new ArgumentNullException("Allowed operations list shouldn't be null!");
            }

            this._allowedOperations = allowedOperations;
        }

        public bool IsAllowed(AppAreaOperation operation)
        {
            return _allowedOperations.Select(i => i.Id).Contains(operation.Id);
        }
    }
}
