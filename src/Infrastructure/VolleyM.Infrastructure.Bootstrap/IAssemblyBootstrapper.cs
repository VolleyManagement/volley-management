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

        void RegisterMappingProfiles(MapperConfigurationExpression mce);
    }
}
