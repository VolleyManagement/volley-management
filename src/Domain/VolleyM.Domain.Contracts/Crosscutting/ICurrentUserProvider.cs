namespace VolleyM.Domain.Contracts.Crosscutting
{
    /// <summary>
    /// Provides information about currently logged in user
    /// </summary>
    public interface ICurrentUserProvider
    {
        UserId UserId { get; }

        TenantId Tenant { get; }
    }
}