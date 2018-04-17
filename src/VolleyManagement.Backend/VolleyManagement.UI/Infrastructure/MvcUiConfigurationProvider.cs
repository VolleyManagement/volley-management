using System.Web.Configuration;
using VolleyManagement.Crosscutting.Contracts.Providers;

namespace VolleyManagement.UI.Infrastructure
{
    public class MvcUiConfigurationProvider : IConfigurationProvider
    {
        public string GetGoogleClientId()
        {
            throw new System.NotImplementedException();
        }

        public string GetVolleyManagementEntitiesConnectionString()
        {
            return WebConfigurationManager.ConnectionStrings["VolleyManagementEntities"].ConnectionString;
        }
    }
}