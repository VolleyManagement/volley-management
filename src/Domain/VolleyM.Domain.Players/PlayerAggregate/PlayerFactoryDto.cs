using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Players.PlayerAggregate
{
	public class PlayerFactoryDto
	{
		public TenantId Tenant { get; set; }

		public PlayerId Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}