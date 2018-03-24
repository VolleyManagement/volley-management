using System;
using VolleyManagement.Crosscutting.Contracts.Providers;

namespace VolleyManagement.Specs.Infrastructure
{
    public class IntegrationTestConfigurationProvider : IConfigurationProvider
    {
        private const string IT_CONNECTION_STRING =
            @"data source=(localdb)\mssqllocaldb;initial catalog=VolleyManagement-integrationtests;integrated security=True";

        private const string APPVEYOR_DB_CONNECTION_STRING =
            @"Server=(local)\SQL2017;Database=VolleyManagement-integrationtests;User ID=sa;Password=Password12!";

        string IConfigurationProvider.GetVolleyManagementEntitiesConnectionString()
        {
            return GetVolleyManagementEntitiesConnectionString();
        }

        public static string GetVolleyManagementEntitiesConnectionString()
        {
            return IsRunningOnAppVeyor() ? APPVEYOR_DB_CONNECTION_STRING : IT_CONNECTION_STRING;
        }

        private static bool IsRunningOnAppVeyor()
        {
            return Environment.GetEnvironmentVariable("APPVEYOR") != null;
        }
    }
}