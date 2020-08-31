using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.Players.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests
{
    public class AzureCloudPlayersTestFixture : PlayersTestFixtureBase, IPlayersTestFixture
	{
		private List<(TenantId Tenant, PlayerId Id)> _playersToTeardown;

		public override Task ScenarioSetup()
		{
			_playersToTeardown = new List<(TenantId Tenant, PlayerId Id)>();
			return Task.CompletedTask;
		}

		public override async Task ScenarioTearDown()
		{
			await CleanUpPlayers();
		}

		public async Task MockPlayerExists(TestPlayerDto player)
		{
			var repo = _container.GetInstance<IPlayersRepository>();
			var playerDomain = new Player(CurrentTenant, player.Id, player.FirstName, player.LastName);
			await EnsureSuccessfulCreation(repo, playerDomain);

			_playersToTeardown.Add((CurrentTenant, player.Id));
		}

		public async Task MockSeveralPlayersExist(TenantId tenant, List<Player> testData)
		{
			var repo = _container.GetInstance<IPlayersRepository>();
			foreach (var player in testData)
			{
				await EnsureSuccessfulCreation(repo, player);
			}
		}

		public async Task VerifyPlayerCreated(Player expectedPlayer)
		{
			var repo = _container.GetInstance<IPlayersRepository>();

			var savedPlayer = await repo.Get(expectedPlayer.Tenant, expectedPlayer.Id).ToEither();

			savedPlayer.ShouldBeEquivalent(expectedPlayer, "all attributes should be saved correctly");

			_playersToTeardown.Add((expectedPlayer.Tenant, expectedPlayer.Id));
		}

		public async Task VerifyPlayerNotCreated(Player expectedPlayer)
		{
			var repo = _container.GetInstance<IPlayersRepository>();

			var savedPlayer = await repo.Get(expectedPlayer.Tenant, expectedPlayer.Id).ToEither();

			savedPlayer.ShouldBeError(Error.NotFound(), "NotFound error is expected");
		}

		private async Task CleanUpPlayers()
		{
			var repo = _container.GetInstance<IPlayersRepository>();
			var deleteTasks = new List<Task>();
			foreach (var (tenant, player) in _playersToTeardown)
			{
				deleteTasks.Add(repo.Delete(tenant, player).ToEither());
			}

			await Task.WhenAll(deleteTasks.ToArray());
		}

		private static async Task EnsureSuccessfulCreation(IPlayersRepository repo, Player player)
		{
			var createResult = await repo.Add(player).ToEither();
			createResult.IsRight.Should().BeTrue("no error in player creation should be detected");
		}
	}
}