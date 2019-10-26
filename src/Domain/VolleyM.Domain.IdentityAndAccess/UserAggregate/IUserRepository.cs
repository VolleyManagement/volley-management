using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess
{
    public interface IUserRepository
    {
        Task<Either<Error, User>> Add(User user);

        Task<Either<Error, User>> Get(TenantId tenant, UserId id);

        Task<Either<Error, Unit>> Delete(TenantId tenant, UserId id);
    }
}