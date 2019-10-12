using Destructurama.Attributed;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess.RolesAggregate
{
    [LogAsScalar]
    public class RoleId : IdBase<string>
    {
        public RoleId(string value) : base(value) { }
    }
}