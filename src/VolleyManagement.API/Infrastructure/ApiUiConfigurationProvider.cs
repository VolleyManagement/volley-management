using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using IConfigurationProvider = VolleyManagement.Crosscutting.Contracts.Providers.IConfigurationProvider;

namespace VolleyManagement.API.Infrastructure
{
    public class ApiUiConfigurationProvider : IConfigurationProvider
    {
        public string GetVolleyManagementEntitiesConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", true).Build();
            return builder.GetConnectionString("VolleyManagementEntities");
        }
    }
}