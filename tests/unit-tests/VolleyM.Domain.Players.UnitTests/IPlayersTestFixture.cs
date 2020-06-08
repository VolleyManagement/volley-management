using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.Players.UnitTests.Fixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests
{
	public interface IPlayersTestFixture : ITenantTestFixture
	{
		Task MockPlayerExists(TestPlayerDto player);

		Task MockSeveralPlayersExist(TenantId tenant, List<Player> testData);

		Task VerifyPlayerCreated(Player expectedPlayer);

		void MockNextRandomId(string id);

		Task VerifyPlayerNotCreated(Player expectedPlayer);
	}
}
