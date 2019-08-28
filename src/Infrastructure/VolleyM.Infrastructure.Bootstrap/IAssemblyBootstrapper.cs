using SimpleInjector;

namespace VolleyM.Infrastructure.Bootstrap
{
    /// <summary>
    /// Used by assemblies to register themselves in the application
    /// </summary>
    public interface IAssemblyBootstrapper
    {
        void Register(Container container);
    }
}
