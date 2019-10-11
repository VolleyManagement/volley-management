using System.Composition;
using AutoMapper.Configuration;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.Framework.Logging;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Framework
{
    [Export(typeof(IAssemblyBootstrapper))]
    public class DomainFrameworkAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            container.Register<IAuthorizationHandler, DefaultAuthorizationHandler>(Lifestyle.Scoped);

            container.Register<ICurrentUserProvider, CurrentUserProvider>(Lifestyle.Scoped);
            container.Register<ICurrentUserManager, CurrentUserProvider>(Lifestyle.Scoped);

            container.RegisterDecorator(
                typeof(IRequestHandler<,>),
                typeof(LoggingRequestHandlerDecorator<,>),
                Lifestyle.Scoped);

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