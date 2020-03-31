using System.Threading.Tasks;
using SimpleInjector;
using VolleyM.Domain.Framework.EventBroker;

namespace VolleyM.Infrastructure.EventBroker.UnitTests
{
    public class UnitEventBrokerTestFixture : IEventBrokerTestFixture
    {
        public void RegisterScenarioDependencies(Container container)
        {
            container.Register<IEventPublisher, SimpleEventPublisher>();
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