namespace VolleyM.Domain.Contracts.Crosscutting
{
    /// <summary>
    /// Provides information about currently logged in user
    /// </summary>
    public interface ICurrentUserProvider
    {
        UserId User { get; }

        TenantId Tenant { get; }
    }
}