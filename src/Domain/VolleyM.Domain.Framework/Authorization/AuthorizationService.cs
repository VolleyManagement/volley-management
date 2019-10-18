using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using IAPermissions = VolleyM.Domain.IdentityAndAccess.Permissions;

namespace VolleyM.Domain.Framework.Authorization
{
    internal class AuthorizationService : IAuthorizationService
    {
        internal static RoleId _authZRoleId = new RoleId("authz.handler");

        private readonly IRolesStore _rolesStore;
        private readonly ICurrentUserManager _currentUserManager;

        private static readonly Dictionary<RoleId, Role> _systemRoleStore = new Dictionary<RoleId, Role>();

        static AuthorizationService()
        {
            PopulateSystemRoles();
        }

        public AuthorizationService(IRolesStore rolesStore, ICurrentUserManager currentUserManager)
        {
            _rolesStore = rolesStore;
            _currentUserManager = currentUserManager;
        }

        public async Task<bool> CheckAccess(Permission permission)
        {
            var user = _currentUserManager.Context.User;

            if (!user.HasRole)
            {
                return false;
            }

            if (!IsSystemRole(user.Role, out Role role))
            {

                var roleStoreResult = await _rolesStore.Get(user.Role);

                if (!roleStoreResult.IsSuccessful)
                {
                    Log.Warning("Cannot authorize user because RolesStore returned an {Error}", roleStoreResult.Error);
                    return false;
                }

                role = roleStoreResult.Value;
            }

            return role.HasPermission(permission);
        }

        private static bool IsSystemRole(RoleId roleId, out Role role)
        {
            return _systemRoleStore.TryGetValue(roleId, out role);
        }

        private static void PopulateSystemRoles()
        {
            var authZRole = new Role(_authZRoleId);
            authZRole.AddPermission(new Permission(IAPermissions.Context, IAPermissions.User.GetUser));
            authZRole.AddPermission(new Permission(IAPermissions.Context, IAPermissions.User.CreateUser));
            _systemRoleStore[_authZRoleId] = authZRole;
        }
    }
}