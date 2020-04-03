using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SimpleInjector;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.UnitTests
{
	public class AzureCloudPlayersTestFixture : IPlayersTestFixture
	{
		private readonly Container _container;

		public AzureCloudPlayersTestFixture(Container container)
		{
			_container = container;
		}

		public void RegisterScenarioDependencies(Container container)
		{
			// do nothing
		}

		public Task ScenarioSetup()
		{
			return Task.CompletedTask;
		}

		public Task ScenarioTearDown()
		{
			return Task.CompletedTask;
		}

		public void MockSeveralPlayersExist(List<PlayerDto> testData)
		{
		}

		public async Task VerifyPlayerCreated(Player expectedPlayer)
		{
			var repo = _container.GetInstance<IPlayersRepository>();

			var savedPlayer = await repo.Get(expectedPlayer.Tenant, expectedPlayer.Id);

			savedPlayer.IsRight.Should().BeTrue("user should be created");
			savedPlayer.IfRight(u => u.Should().BeEquivalentTo(expectedPlayer, "all attributes should be saved correctly"));
		}
	}
}