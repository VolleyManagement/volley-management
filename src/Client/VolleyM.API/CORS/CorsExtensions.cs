using System;
using Microsoft.Extensions.DependencyInjection;

namespace VolleyM.API.CORS
{
    public static class CorsExtensions
    {
        public static void AddCorsFromSettings(this IServiceCollection services, CorsOptions corsOpts)
        {
            var configuredOrigins = ParseOrigins(corsOpts.AllowedOrigins);
            services.AddCors(opts =>
            {
                opts.AddDefaultPolicy(pol =>
                {
                    pol.WithOrigins(configuredOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        private static string[] ParseOrigins(string allowedOriginsStr)
            => allowedOriginsStr.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }
}