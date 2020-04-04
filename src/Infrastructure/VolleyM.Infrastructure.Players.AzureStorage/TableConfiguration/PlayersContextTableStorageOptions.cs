using VolleyM.Infrastructure.AzureStorage;

namespace VolleyM.Infrastructure.Players.AzureStorage.TableConfiguration
{
	public class PlayersContextTableStorageOptions : AzureTableStorageOptions
	{
		public string PlayersTable { get; set; } = "players";
	}
}