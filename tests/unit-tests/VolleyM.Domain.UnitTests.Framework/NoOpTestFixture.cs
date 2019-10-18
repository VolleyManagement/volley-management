using SimpleInjector;

namespace VolleyM.Domain.UnitTests.Framework
{
    public class NoOpTestFixture : ITestFixture
    {
        public void RegisterScenarioDependencies(Container container)
        {
            //do nothing
        }

        public void ScenarioSetup()
        {
            //do nothing
        }

        public void ScenarioTearDown()
        {
            //do nothing
        }
    }
}