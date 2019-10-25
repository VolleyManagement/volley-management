using System.Security.Claims;
using System.Threading.Tasks;
using LanguageExt;

namespace VolleyM.Domain.Contracts.Crosscutting
{
    public interface IAuthorizationHandler
    {
        Task<Either<Error, Unit>> AuthorizeUser(ClaimsPrincipal user);
    }
}