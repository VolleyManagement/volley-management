using Destructurama.Attributed;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Players.PlayerAggregate
{
	[LogAsScalar]
	public class PlayerId : ImmutableBase<string>
	{
		public PlayerId(string id) : base(id) { }
	}
}