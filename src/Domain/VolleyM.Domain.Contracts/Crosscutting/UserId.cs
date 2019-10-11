using Destructurama.Attributed;

namespace VolleyM.Domain.Contracts
{
    [LogAsScalar()]
    public class UserId : IdBase<string>
    {
        public UserId(string id) : base(id) { }
    }
}