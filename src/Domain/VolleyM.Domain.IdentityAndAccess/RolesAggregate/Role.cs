using System.Collections.Generic;

namespace VolleyM.Domain.IdentityAndAccess.RolesAggregate
{
    public class Role
    {
        private readonly HashSet<string> _permissions = new HashSet<string>();

        public Role(RoleId id)
        {
            Id = id;
        }

        public RoleId Id { get; }

        public void AddPermission(Permission permission)
        {
            _permissions.Add(permission.ToString());
        }

        public bool HasPermission(Permission permission)
        {
            return _permissions.TryGetValue(permission.ToString(), out var actual);
        }
    }
}