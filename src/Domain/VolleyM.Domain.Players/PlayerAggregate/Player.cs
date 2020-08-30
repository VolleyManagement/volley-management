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

		public string FirstName { get; private set; }

		public string LastName { get; private set; }

		public void ChangeName(string firstName, string lastName)
		{
			this.FirstName = firstName;
			this.LastName = lastName;
		}
	}
}