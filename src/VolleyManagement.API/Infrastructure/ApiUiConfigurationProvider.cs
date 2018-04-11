namespace VolleyManagement.API.Infrastructure
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;
    using VolleyManagement.Crosscutting.Contracts.Providers;
    using IConfigurationProvider = VolleyManagement.Crosscutting.Contracts.Providers.IConfigurationProvider;

    public class ApiUiConfigurationProvider : IConfigurationProvider, ISecretsProvider
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
        public IConfigurationSection GetGoogleClientId()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Path.Combine(Environment.CurrentDirectory,"../../..")).AddJsonFile("secrets.json", true).Build();
            return builder.GetSection("Authentication:GoogleClientId");
        }
    }
}