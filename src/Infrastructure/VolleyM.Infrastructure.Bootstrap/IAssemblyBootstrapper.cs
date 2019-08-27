using System;
using System.Threading.Tasks;

namespace VolleyM.Infrastructure.Bootstrap
{
    /// <summary>
    /// Used by assemblies to register themselves in the application
    /// </summary>
    public interface IAssemblyBootstrapper
    {
        Task Register();
    }
}
