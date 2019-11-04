using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

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

            var roleRes = GetSystemRole(user.Role)
                .ToEither(Error.NotFound())
                .MapLeft(_ => _rolesStore.Get(user.Role).ToAsync())
                .Match(EitherAsync<Error, Role>.Right, l => l);

            return (await roleRes.ToEither())
                .Match(
                    role => role.HasPermission(permission),
                    e =>
                    {
                        Log.Warning("Cannot authorize user because RolesStore returned an {Error}", e);
                        return false;
                    });
        }

        private static Option<Role> GetSystemRole(RoleId roleId)
        {
            if (_systemRoleStore.TryGetValue(roleId, out var role))
            {
                return role;
            }

            return Option<Role>.None;
        }

        private static void PopulateSystemRoles()
        {
            var authZRole = new Role(_authZRoleId);
            authZRole.AddPermission(new Permission(IdentityAndAccessConstants.Context, nameof(GetUser)));
            authZRole.AddPermission(new Permission(IdentityAndAccessConstants.Context, nameof(CreateUser)));
            _systemRoleStore[_authZRoleId] = authZRole;
        }
    }
}