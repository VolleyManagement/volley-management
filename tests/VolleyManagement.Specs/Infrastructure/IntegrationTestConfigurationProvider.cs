using VolleyManagement.Crosscutting.Contracts.Providers;

namespace VolleyManagement.Specs.Infrastructure
{
    public class IntegrationTestConfigurationProvider : IConfigurationProvider
    {
        public const string IT_CONNECTION_STRING =
            @"data source=(localdb)\mssqllocaldb;initial catalog=VolleyManagement-integrationtests;integrated security=True";

        public string GetVolleyManagementEntitiesConnectionString()
        {
            return IT_CONNECTION_STRING;
        }
    }
}