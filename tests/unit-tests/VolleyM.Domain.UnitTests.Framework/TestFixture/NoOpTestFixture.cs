using System.Threading.Tasks;
using SimpleInjector;

namespace VolleyM.Domain.UnitTests.Framework
{
    public class NoOpTestFixture : ITestFixture
    {
        public void RegisterScenarioDependencies(Container container)
        {
            //do nothing
        }

        public Task ScenarioSetup()
        {
            //do nothing
            return Task.CompletedTask;
        }

        public Task ScenarioTearDown()
        {
            //do nothing
            return Task.CompletedTask;
        }
    }
}