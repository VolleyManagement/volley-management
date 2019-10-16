using Microsoft.Extensions.Configuration;

namespace VolleyM.Domain.UnitTests.Framework
{
    /// <summary>
    /// Designates a fixture used to setup test state and manage test data
    /// </summary>
    public interface ITestFixture
    {
    }

    public interface IOneTimeTestFixture
    {
        void OneTimeSetup(IConfiguration configuration);

        void OneTimeTearDown();
    }
}