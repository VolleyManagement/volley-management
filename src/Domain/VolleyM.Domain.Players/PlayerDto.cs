using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players
{
	public class PlayerDto
    {
		public TenantId Tenant { get; set; }

		public Version Version { get; set; }

		public PlayerId Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
