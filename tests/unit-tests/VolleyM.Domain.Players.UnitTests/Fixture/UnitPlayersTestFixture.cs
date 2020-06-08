using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
		private IMapper _mapper;

		private Player _actualPlayer;

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

		public override Task ScenarioSetup()
		{
			_mapper = _container.GetInstance<IMapper>();
			return Task.CompletedTask;
		}

		public override Task ScenarioTearDown()
		{
			return Task.CompletedTask;
		}

		public Task MockSeveralPlayersExist(TenantId tenant, List<Player> testData)
		{
			var mappedData = testData.Select(p => new PlayerDto
			{
				Tenant = p.Tenant,
				Id = p.Id,
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
