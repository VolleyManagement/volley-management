namespace VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC
{
    /// <summary>
    /// Objects which implements this interface could be used to register
    /// dependencies during application instantiation
    /// </summary>
    public interface IIocRegistrationModule
    {
        /// <summary>
        /// Allows to register dependencies during application instantiation
        /// </summary>
        /// <param name="container">Bridge to application IOC container</param>
        void RegisterDependencies(IIocContainer container);
    }
}
