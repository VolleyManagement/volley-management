namespace VolleyManagement.Crosscutting.Contracts.Providers
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Provides configurations for external auth secrets
    /// </summary>
    public interface ISecretsProvider
    {
        IConfigurationSection GetGoogleClientId();
    }
}
