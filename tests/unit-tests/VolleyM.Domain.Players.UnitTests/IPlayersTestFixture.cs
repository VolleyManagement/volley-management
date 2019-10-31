using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests
{
    public interface IPlayersTestFixture : ITestFixture
    {
        void MockSeveralPlayersExist(List<PlayerDto> testData);
    }
}
