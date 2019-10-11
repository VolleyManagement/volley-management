using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;

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
    }
}