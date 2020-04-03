using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests
{
	public interface IPlayersTestFixture : ITestFixture
	{
		void MockSeveralPlayersExist(List<PlayerDto> testData);

		Task VerifyPlayerCreated(Player expectedPlayer);
	}
}
