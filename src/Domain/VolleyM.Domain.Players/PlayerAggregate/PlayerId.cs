using Destructurama.Attributed;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.Players.PlayerAggregate
{
	[LogAsScalar]
	public class PlayerId : IdBase<string>
	{
		public PlayerId(string id) : base(id) { }
	}
}