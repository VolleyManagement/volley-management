using FluentAssertions;
using Microsoft.Extensions.Configuration;
using VolleyM.Domain.UnitTests.Framework;
using VolleyM.Infrastructure.Players.AzureStorage.TableConfiguration;

namespace VolleyM.Domain.Players.UnitTests.Fixture
{
	public class AzureCloudPlayersOneTimeFixture : IOneTimeTestFixture
	{
		private TableConfiguration _tableConfig;
		private PlayersContextTableStorageOptions _options;

		public void OneTimeSetup(IConfiguration configuration)
		{
			_options = configuration.GetSection("PlayersContextTableStorageOptions")
				.Get<PlayersContextTableStorageOptions>();

			_tableConfig = new TableConfiguration(_options);
			var result = _tableConfig.ConfigureTables().Result;

			result.IsRight.Should().BeTrue("Azure Storage should be configured correctly");
		}

		public void OneTimeTearDown()
		{
			var result = _tableConfig.CleanTables().Result;
			result.IsRight.Should().BeTrue("Azure Storage should be cleaned up correctly");
		}
	}
}