using AutoMapper.Configuration;
using SimpleInjector;
using System.Composition;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Contracts
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class DomainContractsAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            container.RegisterDecorator(typeof(IRequestHandler<,>), typeof(LoggingRequestHandlerDecorator<,>));

            container.RegisterDecorator(
                typeof(IQuery<,>),
                typeof(LoggingQueryObjectDecorator<,>), 
                Lifestyle.Scoped);
        }

        public void RegisterMappingProfiles(MapperConfigurationExpression mce)
        {
            // no need for mappers
        }
    }
}