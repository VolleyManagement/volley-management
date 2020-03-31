using AutoMapper.Configuration;
using SimpleInjector;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace VolleyM.Infrastructure.Bootstrap
{
    /// <summary>
    /// Used by assemblies to register themselves in the application
    /// </summary>
    public interface IAssemblyBootstrapper
    {
        void RegisterDependencies(Container container, IConfiguration config);

        /// <summary>
        /// Indicates whether this assembly is a target for common registrations
        /// </summary>
        bool HasDomainComponents { get; }

        /// <summary>
        /// Provides registrar for common dependencies; or null
        /// </summary>
        IDomainComponentDependencyRegistrar DomainComponentDependencyRegistrar { get; }

        void RegisterMappingProfiles(MapperConfigurationExpression mce);
    }
}
