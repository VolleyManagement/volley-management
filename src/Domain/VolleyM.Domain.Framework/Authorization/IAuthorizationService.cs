using System.Threading.Tasks;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.Authorization
{
    public interface IAuthorizationService
    {
        /// <summary>
        /// Validates if current user has required permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task<bool> CheckAccess(Permission permission);
    }
}