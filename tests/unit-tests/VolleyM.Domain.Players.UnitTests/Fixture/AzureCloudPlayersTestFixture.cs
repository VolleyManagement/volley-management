using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt.UnsafeValueAccess;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Domain.UnitTests.Framework.Transforms.Common;

namespace VolleyM.Domain.Players.UnitTests.Fixture
{
	public class AzureCloudPlayersTestFixture : PlayersTestFixtureBase, IPlayersTestFixture
	{
		private List<(TenantId Tenant, PlayerId Id)> _playersToTeardown;

		private NonMockableVersionMap _versionMap;

		public override Task ScenarioSetup()
		{
			_playersToTeardown = new List<(TenantId Tenant, PlayerId Id)>();
			_versionMap = _container.GetInstance<NonMockableVersionMap>();
			return Task.CompletedTask;
		}

		public override async Task ScenarioTearDown()
		{
			await CleanUpPlayers();
		}

		public async Task<Player> MockPlayerExists(TestPlayerDto player)
		{
			var repo = _container.GetInstance<IPlayersRepository>();
			var result = await EnsureSuccessfulCreation(repo, player);

			_playersToTeardown.Add((CurrentTenant, player.Id));

			return result;
		}

		public async Task MockSeveralPlayersExist(TenantId tenant, List<TestPlayerDto> testData)
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

		private async Task<Player> EnsureSuccessfulCreation(IPlayersRepository repo, TestPlayerDto player)
		{
			var playerDomain = new Player(CurrentTenant, Version.Initial, player.Id, player.FirstName, player.LastName);
			var createResult = await repo.Add(playerDomain)
				.Do(created =>
				{
					_versionMap.AssociateTestVersions(GetEntityId(player), created.Version, player.Version);
				})
				.ToEither();
			createResult.IsRight.Should().BeTrue("no error in player creation should be detected");

			return createResult.ValueUnsafe();
		}
	}
}