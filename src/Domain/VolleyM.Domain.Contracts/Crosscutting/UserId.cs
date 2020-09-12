using Destructurama.Attributed;

namespace VolleyM.Domain.Contracts
{
    [LogAsScalar()]
    public class UserId : ImmutableBase<string>
    {
        public UserId(string id) : base(id) { }
    }
}