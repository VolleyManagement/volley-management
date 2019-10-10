using Destructurama.Attributed;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess
{
    [LogAsScalar()]
    public class UserId : IdBase<string>
    {
        public UserId(string id) : base(id)
        {

        }
    }
}