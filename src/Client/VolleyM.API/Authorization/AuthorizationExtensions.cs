using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace VolleyM.API.Authorization
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddDefaultVolleyMAuthorization(this IServiceCollection services, Container container)
        {
            services.AddAuthorizationCore(config =>
            {
                config.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .AddRequirements(new DefaultVolleyMAuthorizationRequirement())
                    .Build();
            });

            services.AddScoped<IAuthorizationHandler>(service => new VolleyMDefaultAuthorizationHandler(container));

            return services;
        }
    }
}