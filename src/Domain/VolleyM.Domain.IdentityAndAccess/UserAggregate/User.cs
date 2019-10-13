using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.IdentityAndAccess
{
    public class User
    {
        public User(UserId id, TenantId tenant)
        {
            Id = id;
            Tenant = tenant;
        }

        public UserId Id { get; }

        public TenantId Tenant { get; }

        public RoleId Role { get; private set; }

        public void AssignRole(RoleId role)
        {
            Role = role;
        }
    }
}