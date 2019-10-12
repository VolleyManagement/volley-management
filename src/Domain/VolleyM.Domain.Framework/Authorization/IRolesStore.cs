using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.Authorization
{
    public interface IRolesStore
    {
        Task<Result<Role>> Get(RoleId roleId);
    }
}