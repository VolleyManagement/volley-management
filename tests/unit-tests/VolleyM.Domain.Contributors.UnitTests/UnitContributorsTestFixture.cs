using NSubstitute;
using SimpleInjector;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Contributors.UnitTests
{
    public class UnitContributorsTestFixture : IContributorsTestFixture
    {
        private IQuery<Unit, List<ContributorDto>> _queryMock;

        public void RegisterScenarioDependencies(Container container)
        {
            _queryMock = Substitute.For<IQuery<Unit, List<ContributorDto>>>();

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

        public void MockSeveralContributorsExist(List<ContributorDto> testData) =>
            _queryMock.Execute(Unit.Default).Returns(testData);
    }
}