using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework
{
	public class EntityId : ImmutableBase<string>
	{
		public EntityId(string value) : base(value)
		{
		}
	}
}