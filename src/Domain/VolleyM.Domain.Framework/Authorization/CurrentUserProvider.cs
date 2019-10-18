using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Framework.Authorization
{
    public class CurrentUserProvider : ICurrentUserProvider, ICurrentUserManager
    {
        public UserId UserId => Context?.User?.Id;

        public TenantId Tenant => Context?.User?.Tenant;

        public CurrentUserContext Context { get; set; }

        public CurrentUserScope BeginScope(CurrentUserContext userScope)
        {
            return new CurrentUserScope(this, userScope);
        }
    }
}