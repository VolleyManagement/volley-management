using Microsoft.Extensions.Configuration;

namespace VolleyManagement.Crosscutting.Contracts.Providers
{
    /// <summary>
    /// Provides configurations for external auth secrets
    /// </summary>
    public interface ISecretsProvider
    {
        IConfigurationSection GetGoogleClientId();
    }
}
