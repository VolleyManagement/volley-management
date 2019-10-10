using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess
{
    public class UserId : IdBase<string>
    {
        public UserId(string id) : base(id)
        {

        }
    }
}