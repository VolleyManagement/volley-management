using System.Collections.Generic;
using System.Reflection;
using SimpleInjector;

namespace VolleyM.Infrastructure.Bootstrap
{
    /// <summary>
    /// Provides logic for common registration of Domain Components
    /// </summary>
    public interface IDomainComponentDependencyRegistrar
    {
        void RegisterCommonDependencies(Container container, List<Assembly> domainComponentsAssemblies);
    }
}