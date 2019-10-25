using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using Unit = VolleyM.Domain.Contracts.Unit;

namespace VolleyM.Domain.IdentityAndAccess
{
    public interface IUserRepository
    {
        Task<Result<User>> Add(User user);
        Task<Either<Error, User>> Add1(User user);

        Task<Result<User>> Get(TenantId tenant, UserId id);
        Task<Either<Error, User>> Get1(TenantId tenant, UserId id);

        Task<Result<Unit>> Delete(TenantId tenant, UserId id);
    }
}