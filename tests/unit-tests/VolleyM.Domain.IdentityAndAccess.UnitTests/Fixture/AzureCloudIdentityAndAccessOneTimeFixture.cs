using FluentAssertions;
using Microsoft.Extensions.Configuration;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public class AzureCloudIdentityAndAccessOneTimeFixture : IOneTimeTestFixture
    {
        private TableConfiguration _tableConfig;
        private IdentityContextTableStorageOptions _options;

        public void OneTimeSetup(IConfiguration configuration)
        {
            _options = configuration.GetSection("IdentityContextTableStorageOptions")
                .Get<IdentityContextTableStorageOptions>();

            _tableConfig = new TableConfiguration(_options);
            var result = _tableConfig.ConfigureTables().ToEither().Result;

            result.IsRight.Should().BeTrue("Azure Storage should be configured correctly");
        }

        public void OneTimeTearDown()
        {
            var result = _tableConfig.CleanTables().ToEither().Result;
            result.IsRight.Should().BeTrue("Azure Storage should be cleaned up correctly");
        }
    }
}