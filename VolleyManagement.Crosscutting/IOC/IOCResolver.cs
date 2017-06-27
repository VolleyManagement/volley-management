using System;
using System.Collections.Generic;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;

namespace VolleyManagement.Crosscutting.IOC
{
    public class IOCResolver
    {
        private SimpleInjectorDependencyResolver _resolver;

        /// <summary>
        /// Creates an instance of <see cref="IOCResolver"/> class
        /// </summary>
        /// <param name="container">Container to get resolver</param>
        public IOCResolver(Container container)
        {
            _resolver = new SimpleInjectorDependencyResolver(container);
        }

        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <param name="serviceType">The type of the requested service or object.</param>
        /// <returns>The requested service or object.</returns>
        public object GetService(Type serviceType)
        {
            return _resolver.GetService(serviceType);
        }

        /// <summary>
        ///  Resolves multiply registered services
        /// </summary>
        /// <param name="serviceType">The type of the requested services.</param>
        /// <returns>The requested services.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _resolver.GetServices(serviceType);
        }
    }
}
