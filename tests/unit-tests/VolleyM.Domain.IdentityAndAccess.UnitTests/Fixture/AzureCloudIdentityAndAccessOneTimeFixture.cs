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
            var result = _tableConfig.ConfigureTables().Result;

            result.Should().BeSuccessful("Azure Storage should be configured correctly");
        }

        public void OneTimeTearDown()
        {
            var result = _tableConfig.CleanTables().Result;
            result.Should().BeSuccessful("Azure Storage should be cleaned up correctly");
        }
    }
}