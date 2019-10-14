using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.IdentityAndAccess
{
    /// <summary>
    /// Data object to reconstruct User state from persistence
    /// </summary>
    public class UserFactoryDto
    {
        public UserId Id { get; set; }
        public TenantId Tenant { get; set; }
        public RoleId Role { get; set; }
    }
}