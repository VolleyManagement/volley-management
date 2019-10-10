using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.Handlers;

namespace VolleyM.Domain.Framework.Authorization
{
    public class DefaultAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IRequestHandler<CreateUser.Request, Unit> _createUserHandler;

        private readonly List<string> _idClaimTypes = new List<string>
        {
            "sub",
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        };

        public DefaultAuthorizationHandler(IRequestHandler<CreateUser.Request, Unit> createUserHandler)
        {
            _createUserHandler = createUserHandler;
        }

        public async Task<Result<Unit>> AuthorizeUser(ClaimsPrincipal user)
        {
            var idValue = GetUserIdFromClaims(user);

            if (idValue == null)
            {
                return Error.NotAuthorized("Id claim is missing");
            }

            var newUser = new CreateUser.Request
            {
                Id = new UserId(idValue),
                Tenant = TenantId.Default
            };

            var createResult = await _createUserHandler.Handle(newUser);

            return createResult;
        }

        private string GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(type => _idClaimTypes.Contains(type.Type));
            return idClaim?.Value;
        }
    }
}