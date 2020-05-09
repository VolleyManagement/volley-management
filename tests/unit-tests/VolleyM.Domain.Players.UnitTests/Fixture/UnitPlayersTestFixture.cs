using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.UnitTests.Fixture
{
	public class UnitPlayersTestFixture : PlayersTestFixtureBase, IPlayersTestFixture
	{
		private IQuery<TenantId, List<PlayerDto>> _queryMock;
		private IPlayersRepository _repoMock;

		private Player _actualPlayer;

		public UnitPlayersTestFixture(Container container) : base(container)
		{
		}

		public override void RegisterScenarioDependencies(Container container)
		{
			base.RegisterScenarioDependencies(container);

			_queryMock = Substitute.For<IQuery<TenantId, List<PlayerDto>>>();
			container.Register(() => _queryMock, Lifestyle.Scoped);


			_repoMock = Substitute.For<IPlayersRepository>();
			_repoMock.Add(Arg.Any<Player>())
				.Returns(ci => ci.Arg<Player>())
				.AndDoes(ci => { _actualPlayer = ci.Arg<Player>(); });

			container.Register(() => _repoMock, Lifestyle.Scoped);
		}

		public Task ScenarioSetup()
		{
			return Task.CompletedTask;
		}

		public Task ScenarioTearDown()
		{
			return Task.CompletedTask;
		}

		public void MockSeveralPlayersExist(List<PlayerDto> testData) =>
			_queryMock.Execute(TenantId.Default).Returns(testData);

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
