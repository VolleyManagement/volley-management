namespace VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC
{
    /// <summary>
    /// Contains type of lifetimes for types which should be resolved by IOC
    /// </summary>
    public enum IocLifetimeEnum
    {
        Singleton,
        Transient,
        Scoped
    }
}
