using NSubstitute;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Players.UnitTests
{
    public class UnitPlayersTestFixture : IPlayersTestFixture
    {
        private IQuery<TenantId, List<PlayerDto>> _queryMock;

        public void RegisterScenarioDependencies(Container container)
        {
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
    }
}
