using System.Threading.Tasks;
using SimpleInjector;

namespace VolleyM.Infrastructure.EventBroker.UnitTests
{
    public class UnitEventBrokerTestFixture : IEventBrokerTestFixture
    {
        public void RegisterScenarioDependencies(Container container)
        {
        }

        public Task ScenarioSetup()
        {
            return Task.CompletedTask;
        }

        public Task ScenarioTearDown()
        {
            return Task.CompletedTask;
        }
    }
}