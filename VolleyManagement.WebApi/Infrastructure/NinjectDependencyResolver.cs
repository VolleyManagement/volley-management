namespace VolleyManagement.WebApi.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;

    using Ninject;

    /// <summary>
    /// Represents the DI container
    /// </summary>
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
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
            : base(kernel)
        {
            _kernel = kernel;
        }

        /// <summary>
        /// Begins the scope
        /// </summary>
        /// <returns>IDependencyScope object</returns>
        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(_kernel.BeginBlock());
        }
    }
}