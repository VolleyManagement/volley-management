// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="SoftServe">
//   Copyright (c) SoftServe. All rights reserved.
// </copyright>
// <summary>
//   Defines RouteConfig.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace VolleyManagement.WebApi
{
    using System.Web.Http;

    /// <summary>
    /// Defines WebAPIConfig
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
