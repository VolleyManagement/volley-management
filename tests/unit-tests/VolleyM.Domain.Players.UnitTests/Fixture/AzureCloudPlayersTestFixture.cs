using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SimpleInjector;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.Players.UnitTests.Fixture;

namespace VolleyM.Domain.Players.UnitTests
{
	public class AzureCloudPlayersTestFixture : PlayersTestFixtureBase, IPlayersTestFixture
	{
		public AzureCloudPlayersTestFixture(Container container) 
			: base(container)
		{
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