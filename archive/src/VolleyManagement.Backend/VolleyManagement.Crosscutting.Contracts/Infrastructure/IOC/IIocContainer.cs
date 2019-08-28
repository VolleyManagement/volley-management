namespace VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC
{
    using System;

    public interface IIocContainer
    {
        /// <summary>
        /// Allows to register dependencies in particular tier of application
        /// during application instantiation
        /// </summary>
        /// <param name="module">Dependencies registration module</param>
        /// <returns>Current application IOC container</returns>
        IIocContainer Register(IIocRegistrationModule module);

        /// <summary>
        /// Register dependency as singleton (same instance will be used for all dependent objects)
        /// </summary>
        /// <param name="lifetime">Life time of instance for implementation type</param>
        /// <typeparam name="TContract">Type of contract</typeparam>
        /// <typeparam name="TImpl">Type of object which implements contact</typeparam>
        /// <returns>Current application IOC container</returns>
        IIocContainer Register<TContract, TImpl>(IocLifetimeEnum lifetime)
            where TImpl : class, TContract
            where TContract : class;

        /// <summary>
        /// Register dependency as singleton (same instance will be used for all dependent objects
        /// for all requests)
        /// </summary>
        /// <param name="contract">Type of contract</param>
        /// <param name="implementation">Type of object which implements contact</param>
        /// <param name="lifetime">Life time of instance for implementation type</param>
        /// <returns>Current application IOC container</returns>
        IIocContainer Register(Type contract, Type implementation, IocLifetimeEnum lifetime);

        /// <summary>
        /// Resolves instance of specified type
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>Resolved instance</returns>
        T Get<T>() where T : class;
    }
}