using Destructurama.Attributed;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess.RolesAggregate
{
    [LogAsScalar]
    public class RoleId : ImmutableBase<string>
    {
        public RoleId(string value) : base(value) { }
    }
}