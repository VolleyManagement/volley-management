using System.Composition;
using AutoMapper.Configuration;
using SimpleInjector;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.Players
{
	[Export(typeof(IAssemblyBootstrapper))]
	public class DomainPlayersAssemblyBootstrapper : IAssemblyBootstrapper
	{
		public void RegisterDependencies(Container container, Microsoft.Extensions.Configuration.IConfiguration config)
		{
			container.Register<PlayerFactory>(Lifestyle.Singleton);
		}

		public bool HasDomainComponents { get; } = true;
		public IDomainComponentDependencyRegistrar DomainComponentDependencyRegistrar { get; } = null;

		public void RegisterMappingProfiles(MapperConfigurationExpression mce)
			=> mce.AddProfile<DomainPlayersMappingProfile>();
	}
}
