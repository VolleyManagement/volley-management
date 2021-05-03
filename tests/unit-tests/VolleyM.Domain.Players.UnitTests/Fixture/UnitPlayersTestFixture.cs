using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.UnitTests.Framework.Transforms.Common;

namespace VolleyM.Domain.Players.UnitTests.Fixture
{
	public class UnitPlayersTestFixture : PlayersTestFixtureBase, IPlayersTestFixture
	{
		private Container _container;

		private IQuery<TenantId, List<PlayerDto>> _queryMock;
		private IPlayersRepository _repoMock;
		private NonMockableVersionMap _versionMap;

		private Player _actualPlayer;

		public override void RegisterScenarioDependencies(Container container)
		{
			base.RegisterScenarioDependencies(container);
			_container = container;

			_queryMock = Substitute.For<IQuery<TenantId, List<PlayerDto>>>();
			container.Register(() => _queryMock, Lifestyle.Scoped);

			_repoMock = Substitute.For<IPlayersRepository>();
			_repoMock.Add(Arg.Any<Player>())
				.Returns(ci => ci.Arg<Player>())
				.AndDoes(ci => { _actualPlayer = ci.Arg<Player>(); });

			_repoMock.Update(Arg.Any<Player>(), Arg.Any<Version>())
				.Returns(ci =>
				{
					var original= ci.Arg<Player>();
					var updatedVersion = new Version($"updated-{original.Version}");
					
					_versionMap.RecordVersionChange(this.GetEntityId(original), updatedVersion);

					var updated = new Player(original.Tenant, updatedVersion, original.Id, original.FirstName,
						original.LastName);

					return updated;
				})
				.AndDoes(ci =>
				{
					if (_actualPlayer == null) return;
					_actualPlayer = ci.Arg<Player>();
				});

			container.Register(() => _repoMock, Lifestyle.Scoped);
		}

		public override Task ScenarioSetup()
		{
			_versionMap = _container.GetInstance<NonMockableVersionMap>();
			return Task.CompletedTask;
		}

		public override Task ScenarioTearDown()
		{
			return Task.CompletedTask;
		}

		public Task<Player> MockPlayerExists(TestPlayerDto player)
		{
			var p = new Player(CurrentTenant, player.Version,
				player.Id, player.FirstName, player.LastName);

			_repoMock.Get(CurrentTenant, player.Id)
				.Returns(p);

			return Task.FromResult(p);
		}

		public Task MockSeveralPlayersExist(TenantId tenant, List<TestPlayerDto> testData)
		{
			var mappedData = testData.Select(p => new PlayerDto
			{
				Tenant = tenant,
				Id = p.Id,
				Version = new Version("<some-version>"),
				FirstName = p.FirstName,
				LastName = p.LastName
			}).ToList();
			_queryMock.Execute(tenant).Returns(mappedData);

			return Task.CompletedTask;
		}

		public Task VerifyPlayerCreated(Player expectedPlayer)
		{
			_actualPlayer.Should().BeEquivalentTo(expectedPlayer, "player should be created");

			return Task.CompletedTask;
		}

		public Task VerifyPlayerNotCreated(Player expectedPlayer)
		{
			_actualPlayer.Should().BeNull("player should not be created");

			return Task.CompletedTask;
		}
	}
}
