using Microsoft.Extensions.Configuration;

namespace VolleyM.Domain.UnitTests.Framework
{
    public class NoOpOneTimeTestFixture : IOneTimeTestFixture
    {
        public void OneTimeSetup(IConfiguration configuration)
        {
            // no op
        }

        public void OneTimeTearDown()
        {
            // no op
        }

        public static IOneTimeTestFixture Instance { get; } = new NoOpOneTimeTestFixture();
    }
}