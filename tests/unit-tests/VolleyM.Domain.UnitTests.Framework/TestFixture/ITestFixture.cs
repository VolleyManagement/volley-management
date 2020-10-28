using System;
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

		/// <summary>
		/// Domain Entities doe snot require aggregated Id, but for tests it's useful to aggregate those into single one.
		/// </summary>
		/// <param name="instance">Domain instance</param>
		/// <returns>Id or null</returns>
        EntityId GetEntityId(object instance);
    }

    public interface IOneTimeTestFixture
    {
        void OneTimeSetup(IConfiguration configuration);

        void OneTimeTearDown();
    }
}