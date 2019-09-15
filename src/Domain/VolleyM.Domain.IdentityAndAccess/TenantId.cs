namespace VolleyM.Domain.IdentityAndAccess
{
    public class TenantId
    {
        private readonly string _id;

        public TenantId(string id) => _id = id;

        public override string ToString() => _id;
    }
}