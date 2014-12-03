namespace VolleyManagement.Mvc.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Ninject;

    /// <summary>
    /// Represents the DI container
    /// </summary>
    public class NinjectDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// IKernel kernel
        /// </summary>
        private readonly IKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDependencyResolver"/> class.
        /// </summary>
        /// <param name="kernel">Kernel interface</param>
        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        /// <summary>
        /// Get service method.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>Return service.</returns>
        public object GetService(Type serviceType)
        {
            return this._kernel.TryGet(serviceType);
        }

        /// <summary>
        /// Get services service method.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>Return services.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._kernel.GetAll(serviceType);
        }
    }
}