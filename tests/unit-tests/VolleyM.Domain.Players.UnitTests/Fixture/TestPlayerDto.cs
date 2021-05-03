using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.UnitTests.Fixture
{
	/// <summary>
	/// Represents player data to simplify creation of objects
	/// </summary>
	public class TestPlayerDto
	{
		public PlayerId Id { get; set; }

		public Version Version { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}