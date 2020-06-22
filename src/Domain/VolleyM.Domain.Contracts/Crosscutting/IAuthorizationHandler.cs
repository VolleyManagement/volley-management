using System.Security.Claims;
using LanguageExt;

namespace VolleyM.Domain.Contracts.Crosscutting
{
    public interface IAuthorizationHandler
    {
        EitherAsync<Error, Unit> AuthorizeUser(ClaimsPrincipal user);
    }
}