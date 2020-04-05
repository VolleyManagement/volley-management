using AutoMapper.Configuration;
using SimpleInjector;
using VolleyM.Infrastructure.Bootstrap;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace VolleyM.API.Players
{
	public class PlayersApiAssemblyBootstrapper : IAssemblyBootstrapper
	{
		public void RegisterDependencies(Container container, IConfiguration config)
		{
			// do nothing
		}

		public bool HasDomainComponents { get; } = false;

		public IDomainComponentDependencyRegistrar DomainComponentDependencyRegistrar { get; } = null;
		public void RegisterMappingProfiles(MapperConfigurationExpression mce)
		{
			//do nothing
		}
	}
}