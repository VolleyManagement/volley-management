using System.Threading.Tasks;
using Serilog;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.Authorization
{
    internal class AuthorizationService : IAuthorizationService
    {
        private readonly IRolesStore _rolesStore;
        private readonly ICurrentUserManager _currentUserManager;

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

            var role = await _rolesStore.Get(user.Role);

            if (!role.IsSuccessful)
            {
                Log.Warning("Cannot authorize user because RolesStore returned an {Error}", role.Error);
                return false;
            }

            return role.Value.HasPermission(permission);
        }
    }
}