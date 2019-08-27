using System;
using System.Composition;
using System.Threading.Tasks;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Infrastructure.Hardcoded
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class InfrastructureHardcodedAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public Task Register()
        {
            Console.WriteLine("Hardcoded assembly bootstrapper!");
            return Task.CompletedTask;
        }
    }
}
