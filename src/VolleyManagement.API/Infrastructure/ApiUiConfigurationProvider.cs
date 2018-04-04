using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using VolleyManagement.Crosscutting.Contracts.Providers;
using IConfigurationProvider = VolleyManagement.Crosscutting.Contracts.Providers.IConfigurationProvider;

namespace VolleyManagement.API.Infrastructure
{
    public class ApiUiConfigurationProvider : IConfigurationProvider, ISecretsProvider
    {
        public string GetVolleyManagementEntitiesConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", true).Build();
            return builder.GetConnectionString("VolleyManagementEntities");
        }

        public IConfigurationSection GetGoogleClientId()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Path.Combine(Environment.CurrentDirectory,"../../..")).AddJsonFile("secrets.json", true).Build();
            return builder.GetSection("Authentication:GoogleClientId");
        }
    }
}