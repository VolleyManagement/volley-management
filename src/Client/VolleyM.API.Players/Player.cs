using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.API.Players
{
	public class Player
	{
		public string Tenant { get; set; }

		public string Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}