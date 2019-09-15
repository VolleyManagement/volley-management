using System.Net.Http.Headers;

namespace VolleyM.Domain.IdentityAndAccess
{
    public class UserId
    {
        private readonly string _id;

        public UserId(string id) => _id = id;

        public override string ToString() => _id;
    }
}