using Microsoft.AspNetCore.Authorization;
using SimpleInjector;
using System.Threading.Tasks;

namespace VolleyM.API.Authorization
{
    public class VolleyMDefaultAuthorizationHandler : AuthorizationHandler<DefaultVolleyMAuthorizationRequirement>
    {
        private readonly Container _container;

        public VolleyMDefaultAuthorizationHandler(Container container)
        {
            _container = container;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultVolleyMAuthorizationRequirement requirement)
        {
            var authZHandler = _container.GetInstance<VolleyM.Domain.Contracts.Crosscutting.IAuthorizationHandler>();

            var authResult = await authZHandler.AuthorizeUser(context.User);

            authResult.Match(
                _ => context.Succeed(requirement),
                _ => context.Fail()
            );
        }
    }
}