using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess
{
    public interface IUserRepository
    {
        Task<Result<Unit>> Add(User user);

        Task<Result<User>> Get(TenantId tenant, UserId id);
    }
}