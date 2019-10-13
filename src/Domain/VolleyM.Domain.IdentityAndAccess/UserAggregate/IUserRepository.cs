using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess
{
    public interface IUserRepository
    {
        Task<Result<User>> Add(User user);

        Task<Result<User>> Get(TenantId tenant, UserId id);

        Task<Result<Unit>> Delete(TenantId tenant, UserId id);
    }
}