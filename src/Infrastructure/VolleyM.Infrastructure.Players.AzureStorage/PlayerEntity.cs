using Microsoft.Azure.Cosmos.Table;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Infrastructure.Players.AzureStorage
{
	public class PlayerEntity : TableEntity
	{

		public PlayerEntity()
		{
			
		}

		public PlayerEntity(TenantId tenant, PlayerId id)
		{
			PartitionKey = tenant.ToString();
			RowKey = id.ToString();
		}

		public PlayerEntity(Player player)
		: this(player.Tenant, player.Id)
		{
			FirstName = player.FirstName;
			LastName = player.LastName;
		}

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}