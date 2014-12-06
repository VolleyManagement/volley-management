namespace VolleyManagement.WebApi.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;

    using Ninject;
    using Ninject.Syntax;

    /// <summary>
    /// Implements Dependency Scope
    /// </summary>
    public class NinjectDependencyScope : IDependencyScope
    {
        /// <summary>
        /// Resolves instances
        /// </summary>
        private IResolutionRoot _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDependencyScope" /> class.
        /// </summary>
        /// <param name="resolver">Path to resolve instances</param>
        public NinjectDependencyScope(IResolutionRoot resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Get service method.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>Return service.</returns>
        public object GetService(Type serviceType)
        {
            if (_resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has been disposed");
            }

            return _resolver.TryGet(serviceType);
        }

        /// <summary>
        /// Get services service method.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        /// <returns>Return services.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (_resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has been disposed");
            }

            return _resolver.GetAll(serviceType);
        }

        /// <summary>
        /// Disposes the Dependency Scope
        /// </summary>
        public void Dispose()
        {
            IDisposable disposable = _resolver as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }

            _resolver = null;
        }
    }
}