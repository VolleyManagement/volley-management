using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Infrastructure.Hardcoded
{
    public class HardcodedRolesStore : IRolesStore
    {
        private static readonly RoleId _visitor = new RoleId("visitor");
        private readonly Dictionary<RoleId, Role> _roles = new Dictionary<RoleId, Role>
        {
            [_visitor] = new Role(_visitor)
        };

        public HardcodedRolesStore()
        {
            var visitor = new Role(_visitor);
            visitor.AddPermission(new Permission("contributors", "getall"));
        }

        public Task<Result<Role>> Get(RoleId roleId)
        {
            if (_roles.TryGetValue(roleId, out var role))
            {
                return Task.FromResult<Result<Role>>(role);
            }
            return Task.FromResult<Result<Role>>(Error.NotFound());
        }
    }
}