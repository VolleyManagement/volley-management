using System.Composition;
using System.Reflection;
using MediatR;
using SimpleInjector;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Contributors
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class DomainContributorsAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void Register(Container container)
        {
            container.Register(typeof(IRequestHandler<,>), Assembly.GetAssembly(GetType()), Lifestyle.Scoped);
        }
    }
}
