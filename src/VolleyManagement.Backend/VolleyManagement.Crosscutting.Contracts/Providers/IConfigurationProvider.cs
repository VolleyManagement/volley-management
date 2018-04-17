namespace VolleyManagement.Crosscutting.Contracts.Providers
{
    /// <summary>
    /// Provides specific configurations for different services like DB which can be differnt depending on environment
    /// and configuration for external auth secrets
    /// </summary>
    public interface IConfigurationProvider
    {
        string GetVolleyManagementEntitiesConnectionString();

        string GetGoogleClientId();
    }
}