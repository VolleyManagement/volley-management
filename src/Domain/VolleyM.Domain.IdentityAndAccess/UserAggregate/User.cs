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

        internal User(UserFactoryDto userDto)
            : this(userDto.Id, userDto.Tenant)
        {
            Role = userDto.Role;
        }

        public UserId Id { get; }

        public TenantId Tenant { get; }

        public RoleId Role { get; private set; }

        public bool HasRole => Role != null;

        public void AssignRole(RoleId role)
        {
            Role = role;
        }
    }
}