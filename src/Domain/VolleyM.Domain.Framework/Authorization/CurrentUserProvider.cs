using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Framework.Authorization
{
    public class CurrentUserProvider : ICurrentUserProvider, ICurrentUserManager
    {
        private CurrentUserContext _context;

        public UserId User => _context?.User;
        public TenantId Tenant => _context?.Tenant;

        public void SetCurrentContext(CurrentUserContext context)
        {
            _context = context;
        }
    }
}