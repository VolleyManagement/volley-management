using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.Authorization
{
    public interface IRolesStore
    {
        Task<Either<Error, Role>> Get(RoleId roleId);
    }
}