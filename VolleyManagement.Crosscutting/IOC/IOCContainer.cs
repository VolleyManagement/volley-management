namespace VolleyManagement.Crosscutting.IOC
{
    using System;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;

    /// <summary>
    /// Warapper over application IOC container
    /// </summary>
    public class IocContainer
    {
        private readonly Container _container;

        /// <summary>
        /// Creates an instance of <see cref="IocContainer"/> object
        /// </summary>
        public IocContainer()
        {
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
        }

        /// <summary>
        /// Returns object which could be used as dependency resolver in application
        /// </summary>
        /// <returns>object which could be used as dependency resolver in application</returns>
        public IocResolver GetResolver()
        {
            return new IocResolver(_container);
        }

        /// <summary>
        /// Allows to register dependencies in particular tier of application
        /// during application instantiation
        /// </summary>
        /// <param name="module">Dependencies registration module</param>
        /// <returns>Current application IOC container</returns>
        public IocContainer Register(IIocRegistrationModule module)
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
        public IocContainer Register<TContract, TImpl>(Lifetimes lifetime)
            where TImpl : class, TContract
            where TContract : class
        {
            var style = MapLifetime(lifetime);
            _container.Register<TContract, TImpl>(style);

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
        public IocContainer Register(Type contract, Type implementation, Lifetimes lifetime)
        {
            var style = MapLifetime(lifetime);
            _container.Register(contract, implementation, Lifestyle.Singleton);

            return this;
        }


        private static Lifestyle MapLifetime(Lifetimes lifetime)
        {
            Lifestyle style;

            switch (lifetime)
            {
                case Lifetimes.Singleton:
                    style = Lifestyle.Singleton;
                    break;
                case Lifetimes.Transient:
                    style = Lifestyle.Transient;
                    break;
                case Lifetimes.Scoped:
                    style = Lifestyle.Scoped;
                    break;
                default:
                    throw new ArgumentException("Unknown lifetime type");
            }

            return style;
        }
    }
}
