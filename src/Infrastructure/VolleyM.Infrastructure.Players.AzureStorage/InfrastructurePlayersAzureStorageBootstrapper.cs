using System.Composition;
using System.Reflection;
using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Infrastructure.Bootstrap;
using VolleyM.Infrastructure.Players.AzureStorage.TableConfiguration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace VolleyM.Infrastructure.Players.AzureStorage
{
	[Export(typeof(IAssemblyBootstrapper))]
	public class InfrastructurePlayersAzureStorageBootstrapper : IAssemblyBootstrapper
	{
		public void RegisterDependencies(Container container, IConfiguration config)
		{
			container.Register<IPlayersRepository, PlayersRepository>(Lifestyle.Scoped);
			container.Register<IPlayersRepositoryOld, PlayersRepositoryOld>(Lifestyle.Scoped);

			var options = config.GetSection("PlayersContextTableStorageOptions")
				.Get<PlayersContextTableStorageOptions>();

			container.RegisterInstance(options);

			container.Register(typeof(IQueryOld<,>), Assembly.GetAssembly(GetType()), Lifestyle.Scoped);
			container.Register(typeof(IQuery<,>), Assembly.GetAssembly(GetType()), Lifestyle.Scoped);
		}

		public bool HasDomainComponents { get; } = false;

		public IDomainComponentDependencyRegistrar DomainComponentDependencyRegistrar { get; } = null;

		public void RegisterMappingProfiles(MapperConfigurationExpression mce)
			=> mce.AddProfile<PlayersAzureStorageMappingProfile>();
	}
}