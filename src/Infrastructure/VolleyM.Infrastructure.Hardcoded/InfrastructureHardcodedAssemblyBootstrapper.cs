using System.Composition;
using System.Reflection;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Infrastructure.Hardcoded
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class InfrastructureHardcodedAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void Register(Container container)
        {
            container.Register(
                typeof(IQuery<,>), 
                Assembly.GetAssembly(GetType()), 
                Lifestyle.Singleton);
        }
    }
}
