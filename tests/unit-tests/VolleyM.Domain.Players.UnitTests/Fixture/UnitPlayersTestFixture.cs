using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using SimpleInjector;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.UnitTests.Fixture
{
	public class UnitPlayersTestFixture : PlayersTestFixtureBase, IPlayersTestFixture
    {
        private IQuery<TenantId, List<PlayerDto>> _queryMock;

        public UnitPlayersTestFixture(Container container) : base(container)
        {
        }

		public override void RegisterScenarioDependencies(Container container)
		{
			base.RegisterScenarioDependencies(container);

			_queryMock = Substitute.For<IQuery<TenantId, List<PlayerDto>>>();

            container.Register(() => _queryMock, Lifestyle.Scoped);
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
	        throw new System.NotImplementedException();
        }
    }
}
