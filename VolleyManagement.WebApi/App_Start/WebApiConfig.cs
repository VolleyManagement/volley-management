﻿namespace VolleyManagement.WebApi
{
    using System.Web.Http;
    using System.Web.Http.OData.Builder;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.WebApi.ViewModels.Tournaments;
    using VolleyManagement.WebApi.ViewModels.Users;

    /// <summary>
    /// Defines WebAPIConfig
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register configuration
        /// </summary>
        /// <param name="config">Http configuration</param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<TournamentViewModel>("Tournaments");
            builder.EntitySet<UserViewModel>("Users");
            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
