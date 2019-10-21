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
            _permissions.Add(GetPermissionKey(permission));
        }

        public bool HasPermission(Permission permission)
        {
            return _permissions.TryGetValue(GetPermissionKey(permission), out var actual);
        }

        private static string GetPermissionKey(Permission permission) 
            => permission.ToString().ToLowerInvariant();
    }
}