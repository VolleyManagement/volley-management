// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NinjectDependencyResolver.cs" company="SoftServe">
//   Copyright (c) SoftServe. All rights reserved.
// </copyright>
// <summary>
//   Defines the Dependency Resolver.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace VolleyManagement.WebApi.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Ninject;
    using System.Web.Http.Dependencies;
    using VolleyManagement.Services;

    /// <summary>
    /// Repserents the DI container
    /// </summary>
    public class NinjectDependncyResolver: IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependncyResolver()
        {
            this.kernel = CreateKernel();
        }

        /// <summary>
        /// Get service method.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>Return service.</returns>
        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        /// <summary>
        /// Get services service method.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>Return services.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }

        /// <summary>
        /// Registers Ninject modules and creates a StandartKernel
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
                
            return kernel;
        }

        public IDependencyScope BeginScope()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}