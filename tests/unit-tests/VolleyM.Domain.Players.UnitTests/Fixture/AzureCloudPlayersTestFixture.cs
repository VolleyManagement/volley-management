using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.Players.UnitTests.Fixture;

namespace VolleyM.Domain.Players.UnitTests
{
	public class AzureCloudPlayersTestFixture : PlayersTestFixtureBase, IPlayersTestFixture
	{
		private List<Tuple<TenantId, PlayerId>> _playersToTeardown;

		public AzureCloudPlayersTestFixture(Container container)
			: base(container)
		{
		}

		public Task ScenarioSetup()
		{
			_playersToTeardown = new List<Tuple<TenantId, PlayerId>>();
			return Task.CompletedTask;
		}

		public async Task ScenarioTearDown()
		{
			await CleanUpPlayers();
		}

		public void MockSeveralPlayersExist(List<PlayerDto> testData)
		{
		}

		public async Task VerifyPlayerCreated(Player expectedPlayer)
		{
			var repo = _container.GetInstance<IPlayersRepository>();

			var savedPlayer = await repo.Get(expectedPlayer.Tenant, expectedPlayer.Id);

			savedPlayer.IsRight.Should().BeTrue("player should be created");
			savedPlayer.IfRight(u => u.Should().BeEquivalentTo(expectedPlayer, "all attributes should be saved correctly"));
		}

		public async Task VerifyPlayerNotCreated(Player expectedPlayer)
		{
			var repo = _container.GetInstance<IPlayersRepository>();

			var savedPlayer = await repo.Get(expectedPlayer.Tenant, expectedPlayer.Id);

			savedPlayer.IsRight.Should().BeFalse("player should not be created");
			savedPlayer.IfLeft(u => u.Should().BeEquivalentTo(Error.NotFound(), "NotFound error is expected"));
		}

		private async Task CleanUpPlayers()
		{
			var repo = _container.GetInstance<IPlayersRepository>();
			var deleteTasks = new List<Task>();
			foreach (var (tenant, player) in _playersToTeardown)
			{
				deleteTasks.Add(repo.Delete(tenant, player));
			}

			await Task.WhenAll(deleteTasks.ToArray());
		}
	}
}