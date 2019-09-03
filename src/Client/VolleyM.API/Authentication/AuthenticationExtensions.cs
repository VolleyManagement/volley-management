using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace VolleyM.API.Authentication
{
    public static class AuthenticationExtensions
    {
        public static AuthenticationBuilder UseJwtAuth(this IServiceCollection services) =>
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            });

        public static AuthenticationBuilder AddAuth0JwtBearer(this AuthenticationBuilder builder, Auth0Options opts) =>
            builder.AddJwtBearer(options =>
            {
                options.Authority = opts.Domain;
                options.Audience = opts.ApiIdentifier;
            });
    }
}