using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Framework.Authorization
{
    public class CurrentUserContext
    {
        public UserId User { get; set; }
        public TenantId Tenant { get; set; }
    }
}