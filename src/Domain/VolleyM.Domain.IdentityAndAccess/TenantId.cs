using Destructurama.Attributed;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess
{
    [LogAsScalar()]
    public class TenantId : IdBase<string>
    {
        public TenantId(string id) : base(id) { }

        public static TenantId Default { get; } = new TenantId("V011EYMG-0D29-4E9C-BF36-0074DBFC192B");
    }
}