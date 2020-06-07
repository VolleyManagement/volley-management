using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.Players.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests
{
	public class AzureCloudPlayersTestFixture : PlayersTestFixtureBase, IPlayersTestFixture
	{
		private List<(TenantId Tenant, PlayerId Id)> _playersToTeardown;

		public AzureCloudPlayersTestFixture(Container container)
			: base(container)
		{
		}

		public Task ScenarioSetup()
		{
			_playersToTeardown = new List<(TenantId Tenant, PlayerId Id)>();
			return Task.CompletedTask;
		}

		public async Task ScenarioTearDown()
		{
			await CleanUpPlayers();
		}

		public Task MockPlayerExists(TestPlayerDto player)
		{
			return Task.CompletedTask;
		}

		public async Task MockSeveralPlayersExist(List<Player> testData)
		{
			var repo = _container.GetInstance<IPlayersRepository>();
			foreach (var player in testData)
			{
				var createResult = await repo.Add(player);
				createResult.IsRight.Should().BeTrue("no error in player creation should be detected");
			}
		}

		public async Task VerifyPlayerCreated(Player expectedPlayer)
		{
			var repo = _container.GetInstance<IPlayersRepository>();

			var savedPlayer = await repo.Get(expectedPlayer.Tenant, expectedPlayer.Id);

			savedPlayer.IsRight.Should().BeTrue("player should be created");
			savedPlayer.IfRight(u => u.Should().BeEquivalentTo(expectedPlayer, "all attributes should be saved correctly"));

			_playersToTeardown.Add((expectedPlayer.Tenant, expectedPlayer.Id));
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