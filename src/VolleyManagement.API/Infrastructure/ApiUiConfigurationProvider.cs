namespace VolleyManagement.API.Infrastructure
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;

    public class ApiUiConfigurationProvider : Crosscutting.Contracts.Providers.IConfigurationProvider
    {
        /// <summary>
        /// Gets connection string from appsettings file
        /// </summary>
        /// <returns>Connection string</returns>
        public string GetVolleyManagementEntitiesConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", true).Build();
            return builder.GetConnectionString("VolleyManagementEntities");
        }

        /// <summary>
        /// Gets google client id from secrets file
        /// </summary>
        /// <returns>IConfigurationSection</returns>
        public string GetGoogleClientId()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Path.Combine(Environment.CurrentDirectory,"../../..")).AddJsonFile("secrets.json", true).Build();
            return builder.GetSection("Authentication:GoogleClientId").Value;
        }
    }
}