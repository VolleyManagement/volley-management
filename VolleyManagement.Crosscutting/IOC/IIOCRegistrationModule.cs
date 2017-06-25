namespace VolleyManagement.Crosscutting.IOC
{
    /// <summary>
    /// Objects which implements this interface could be used to register 
    /// dependencies during application instantiation
    /// </summary>
    public interface IIOCRegistrationModule
    {
        /// <summary>
        /// Allows to register dependencies during application instantiation
        /// </summary>
        /// <param name="container">Bridge to application IOC container</param>
        void RegisterDependencies(IOCContainer container);
    }
}
