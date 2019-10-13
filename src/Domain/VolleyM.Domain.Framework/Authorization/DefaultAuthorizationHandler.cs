using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.Framework.Authorization
{
    public class DefaultAuthorizationHandler : IAuthorizationHandler
    {
        private readonly UserId _predefinedAnonymousUserId = new UserId("anonym@volleym.idp");

        private readonly IRequestHandler<GetUser.Request, User> _getUserHandler;
        private readonly IRequestHandler<CreateUser.Request, User> _createUserHandler;
        private readonly ICurrentUserManager _currentUserManager;

        private readonly List<string> _idClaimTypes = new List<string>
        {
            "sub",
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        };

        public DefaultAuthorizationHandler(
            IRequestHandler<CreateUser.Request, User> createUserHandler,
            ICurrentUserManager currentUserManager,
            IRequestHandler<GetUser.Request, User> getUserHandler)
        {
            _createUserHandler = createUserHandler;
            _currentUserManager = currentUserManager;
            _getUserHandler = getUserHandler;
        }

        public async Task<Result<Unit>> AuthorizeUser(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                SetCurrentContext(GetAnonymousVisitor(_predefinedAnonymousUserId));
                return Unit.Value;
            }

            var idValue = GetUserIdFromClaims(user);

            if (idValue == null)
            {
                return Error.NotAuthorized("UserId claim is missing");
            }

            if (IsPredefinedSystemId(idValue))
            {
                return Error.NotAuthorized("User Id is invalid");
            }

            Result<Unit> result;
            var getRequest = new GetUser.Request
            {
                UserId = new UserId(idValue),
                Tenant = TenantId.Default
            };
            var getUser = await _getUserHandler.Handle(getRequest);

            if (getUser.IsSuccessful)
            {
                result = Unit.Value;
                SetCurrentContext(getUser.Value);
            }
            else if (getUser.Error.Type == ErrorType.NotFound)
            {
                var createRequest = new CreateUser.Request
                {
                    UserId = new UserId(idValue),
                    Tenant = TenantId.Default
                };
                var createUser = await _createUserHandler.Handle(createRequest);
                if (createUser.IsSuccessful)
                {
                    result = Unit.Value;
                    SetCurrentContext(createUser.Value);
                }
                else
                {
                    result = createUser.Error;
                }
            }
            else
            {
                result = getUser.Error;
            }

            return result;
        }

        private void SetCurrentContext(User user)
        {
            _currentUserManager.Context = new CurrentUserContext
            {
                User = user
            }; ;
        }

        private string GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(type => _idClaimTypes.Contains(type.Type));
            return idClaim?.Value;
        }

        private bool IsPredefinedSystemId(string idValue)
        {
            return _predefinedAnonymousUserId.ToString()
                .Equals(idValue, StringComparison.OrdinalIgnoreCase);
        }

        private static User GetAnonymousVisitor(UserId userId)
        {
            var result = new User(userId, TenantId.Default);

            result.AssignRole(new RoleId("visitor"));

            return result;
        }
    }
}