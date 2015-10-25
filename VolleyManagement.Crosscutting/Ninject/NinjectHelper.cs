namespace VolleyManagement.Crosscutting.Ninject
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

    using global::Ninject.Activation;

    using global::Ninject.Infrastructure;

    /// <summary>
    /// Provides common operation for Ninject registration
    /// </summary>
    public static class NinjectHelper
    {
        /// <summary>
        /// Applies scope provided by delegate to all bindings
        /// </summary>
        /// <param name="bindings">List of bindings</param>
        /// <param name="scopeCallback">Scope to apply</param>
        public static void InScope(
            this IEnumerable<IHaveBindingConfiguration> bindings,
            Func<IContext, object> scopeCallback)
        {
            if (scopeCallback != null)
            {
                bindings.ForEach(b => b.BindingConfiguration.ScopeCallback = scopeCallback);
            }
        }
    }
}