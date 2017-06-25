using System;
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace VolleyManagement.Crosscutting.IOC
{
    /// <summary>
    /// Warapper over application IOC container
    /// </summary>
    public class IOCContainer
    {
        private readonly Container _container;

        /// <summary>
        /// Creates an instance of <see cref="IOCContainer"/> object
        /// </summary>
        public IOCContainer()
        {
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
        }

        /// <summary>
        /// Returns object which could be used as dependency resolver in application
        /// </summary>
        /// <returns>object which could be used as dependency resolver in application</returns>
        public IDependencyResolver GetResolver()
        {
            return new SimpleInjectorDependencyResolver(_container);
        }

        /// <summary>
        /// Allows to register dependencies in particular tier of application
        /// during application instantiation
        /// </summary>
        /// <param name="module">Dependencies registration module</param>
        /// <returns>Current application IOC container</returns>
        public IOCContainer Register(IIOCRegistrationModule module)
        {
            module.RegisterDependencies(this);
            return this;
        }

        /// <summary>
        /// Register dependency as singleton (same instance will be used for all dependent objects)
        /// </summary>
        /// <typeparam name="TContract">Type of contract</typeparam>
        /// <typeparam name="TImpl">Type of object which implements contact</typeparam>
        /// <returns>Current application IOC container</returns>
        public IOCContainer RegisterSingleton<TContract, TImpl>()
            where TImpl : class, TContract
            where TContract : class
        {
            _container.Register<TContract, TImpl>(Lifestyle.Singleton);
            return this;
        }

        /// <summary>
        /// Register dependency as singleton (same instance will be used for all dependent objects
        /// for all requests)
        /// </summary>
        /// <param name="contract">Type of contract</param>
        /// <param name="implementation">Type of object which implements contact</param>
        /// <returns>Current application IOC container</returns>
        public IOCContainer RegisterSingleton(Type contract, Type implementation)
        {
            _container.Register(contract, implementation, Lifestyle.Singleton);
            return this;
        }

        /// <summary>
        /// Register dependency as scoped (each request will be provided
        /// with its own instance of contract implementation, which will be used for all dependent objects)
        /// </summary>
        /// <typeparam name="TContract">Type of contract</typeparam>
        /// <typeparam name="TImpl">Type of object which implements contact</typeparam>
        /// <returns>Current application IOC container</returns>
        public IOCContainer RegisterScoped<TContract, TImpl>()
            where TImpl : class, TContract
            where TContract : class
        {
            _container.Register<TContract, TImpl>(Lifestyle.Scoped);
            return this;
        }

        /// <summary>
        /// Register dependency as scoped(each request will be provided
        /// with its own instance of contract implementation, which will be used for all dependent objects)
        /// </summary>
        /// <param name="contract">Type of contract</param>
        /// <param name="implementation">Type of object which implements contact</param>
        /// <returns>Current application IOC container</returns>
        public IOCContainer RegisterScoped(Type contract, Type implementation)
        {
            _container.Register(contract, implementation, Lifestyle.Scoped);
            return this;
        }

        /// <summary>
        /// Register dependency as transient (for each dependent object will be provided with new instance of
        /// contract implementation)
        /// </summary>
        /// <typeparam name="TContract">Type of contract</typeparam>
        /// <typeparam name="TImpl">Type of object which implements contact</typeparam>
        /// <returns>Current application IOC container</returns>
        public IOCContainer RegisterTransient<TContract, TImpl>()
            where TImpl : class, TContract
            where TContract : class
        {
            _container.Register<TContract, TImpl>(Lifestyle.Transient);
            return this;
        }

        /// <summary>
        /// Register dependency as transient (for each dependent object will be provided with new instance of
        /// contract implementation 
        /// </summary>
        /// <param name="service">Type of contract</param>
        /// <param name="implementation">Type of object which implements contact</param>
        /// <returns>Current application IOC container</returns>
        public IOCContainer RegisterTransient(Type service, Type implementation)
        {
            _container.Register(service, implementation, Lifestyle.Transient);
            return this;
        }

        /// <summary>
        /// Returns instance of object which implements <typeparamref name="TResult"/> 
        /// if all its dependencies could be resolved
        /// </summary>
        /// <typeparam name="TResult">Type to be resolved</typeparam>
        /// <returns>instance of object which implements <typeparamref name="TResult"/></returns>
        public TResult Resolve<TResult>()
        {
            return GetResolver().GetService<TResult>();
        }

    }
}
