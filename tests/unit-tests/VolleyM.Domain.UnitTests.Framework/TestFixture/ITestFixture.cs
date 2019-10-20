using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace VolleyM.Domain.UnitTests.Framework
{
    /// <summary>
    /// Designates a fixture used to setup test state and manage test data
    /// </summary>
    public interface ITestFixture
    {
        void RegisterScenarioDependencies(Container container);

        Task ScenarioSetup();

        Task ScenarioTearDown();
    }

    public interface IOneTimeTestFixture
    {
        void OneTimeSetup(IConfiguration configuration);

        void OneTimeTearDown();
    }
}