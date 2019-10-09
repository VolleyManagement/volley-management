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

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultVolleyMAuthorizationRequirement requirement)
        {
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}