using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.Events
{
	public class PlayerCreated : IPublicEvent
	{
		public TenantId TenantId { get; set; }

		public PlayerId PlayerId { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}