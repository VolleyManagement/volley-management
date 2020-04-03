using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Players.PlayerAggregate
{
	public class Player
	{
		public Player(TenantId tenant, PlayerId id, string firstName, string lastName)
		{
			Tenant = tenant;
			Id = id;
			FirstName = firstName;
			LastName = lastName;
		}
		public TenantId Tenant { get; }

		public PlayerId Id { get; }

		public string FirstName { get; }

		public string LastName { get; }
	}
}