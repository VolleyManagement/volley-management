using AutoMapper.Configuration;
using SimpleInjector;

namespace VolleyM.Infrastructure.Bootstrap
{
    /// <summary>
    /// Used by assemblies to register themselves in the application
    /// </summary>
    public interface IAssemblyBootstrapper
    {
        void RegisterDependencies(Container container);

        void RegisterMappingProfiles(MapperConfigurationExpression mce);
    }
}
