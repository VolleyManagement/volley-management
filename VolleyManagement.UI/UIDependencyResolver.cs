namespace VolleyManagement.UI
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using VolleyManagement.Crosscutting.IOC;

    public class UIDependencyResolver : IDependencyResolver
    {
        private IOCResolver _resolver;

        /// <summary>
        /// Creates an instance of <see cref="UIDependencyResolver"/> class
        /// </summary>
        /// <param name="container">Container to get resolver</param>
        public UIDependencyResolver(IOCContainer container)
        {
            _resolver = container.GetResolver();
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