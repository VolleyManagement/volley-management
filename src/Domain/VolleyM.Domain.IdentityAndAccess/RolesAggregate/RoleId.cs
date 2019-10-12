using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess.RolesAggregate
{
    public class RoleId : IdBase<string>
    {
        public RoleId(string value) : base(value) { }
    }
}