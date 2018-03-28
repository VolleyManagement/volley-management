using System;
using SimpleInjector;
using VolleyManagement.Crosscutting.Contracts.Infrastructure.IOC;

namespace VolleyManagement.Crosscutting.IOC
{
    /// <summary>
    /// Warapper over application IOC container
    /// </summary>
    public class SimpleInjectorContainer : IIocContainer
    {
        /// <summary>
        /// Creates an instance of <see cref="SimpleInjectorContainer"/> object
        /// </summary>
        public SimpleInjectorContainer()
        {
            InternalContainer = new Container();
        }

        /// <summary>
        ///  Gets SimpleInjector container instance
        /// </summary>
        public Container InternalContainer { get; }

        /// <summary>
        /// Allows to register dependencies in particular tier of application
        /// during application instantiation
        /// </summary>
        /// <param name="module">Dependencies registration module</param>
        /// <returns>Current application IOC container</returns>
        public IIocContainer Register(IIocRegistrationModule module)
        {
            module.RegisterDependencies(this);
            return this;
        }

        /// <summary>
        /// Register dependency as singleton (same instance will be used for all dependent objects)
        /// </summary>
        /// <param name="lifetime">Life time of instance for implementation type</param>
        /// <typeparam name="TContract">Type of contract</typeparam>
        /// <typeparam name="TImpl">Type of object which implements contact</typeparam>
        /// <returns>Current application IOC container</returns>
        public IIocContainer Register<TContract, TImpl>(IocLifetimeEnum lifetime)
            where TImpl : class, TContract
            where TContract : class
        {
            var style = MapLifetime(lifetime);
            InternalContainer.Register<TContract, TImpl>(style);

            return this;
        }

        /// <summary>
        /// Register dependency as singleton (same instance will be used for all dependent objects
        /// for all requests)
        /// </summary>
        /// <param name="contract">Type of contract</param>
        /// <param name="implementation">Type of object which implements contact</param>
        /// <param name="lifetime">Life time of instance for implementation type</param>
        /// <returns>Current application IOC container</returns>
        public IIocContainer Register(Type contract, Type implementation, IocLifetimeEnum lifetime)
        {
            var lifestyle = MapLifetime(lifetime);
            InternalContainer.Register(contract, implementation, lifestyle);

            return this;
        }

        public T Get<T>() where T : class
        {
            return InternalContainer.GetInstance<T>();
        }

        private static Lifestyle MapLifetime(IocLifetimeEnum lifetime)
        {
            Lifestyle style;

            switch (lifetime)
            {
                case IocLifetimeEnum.Singleton:
                    style = Lifestyle.Singleton;
                    break;
                case IocLifetimeEnum.Transient:
                    style = Lifestyle.Transient;
                    break;
                case IocLifetimeEnum.Scoped:
                    style = Lifestyle.Scoped;
                    break;
                default:
                    throw new ArgumentException("Unknown lifetime type");
            }

            return style;
        }
    }
}
