using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SimpleInjector;

namespace VolleyM.API.Authorization
{
	public class VolleyMDefaultAuthorizationHandler : AuthorizationHandler<DefaultVolleyMAuthorizationRequirement>
    {
        private readonly Container _container;

        public VolleyMDefaultAuthorizationHandler(Container container)
        {
            _container = container;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultVolleyMAuthorizationRequirement requirement)
        {
            var authZHandler = _container.GetInstance<Domain.Contracts.Crosscutting.IAuthorizationHandler>();

            return authZHandler.AuthorizeUser(context.User)
	            .Match(
                _ => context.Succeed(requirement),
                _ => context.Fail()
            );
        }
    }
}