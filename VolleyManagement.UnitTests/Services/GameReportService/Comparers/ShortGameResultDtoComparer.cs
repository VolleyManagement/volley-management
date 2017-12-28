namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal class ShortGameResultDtoComparer : IComparer<ShortGameResultDto>
    {
        public int Compare(ShortGameResultDto x, ShortGameResultDto y)
        {
            Assert.AreEqual(x.HomeTeamId, y.HomeTeamId, "HomeTeamId do not match");
            Assert.AreEqual(x.HomeGameScore, y.HomeGameScore, "HomeGameScore do not match");

            Assert.AreEqual(x.AwayTeamId, y.AwayTeamId, "AwayTeamId do not match");
            Assert.AreEqual(x.AwayGameScore, y.AwayGameScore, "AwayGameScore do not match");

            Assert.AreEqual(x.IsTechnicalDefeat, y.IsTechnicalDefeat, "IsTechnicalDefeat do not match");

            Assert.AreEqual(x.RoundNumber, y.RoundNumber, "RoundNumber do not match");

            Assert.AreEqual(x.HasResult, y.HasResult, "HasResult do not match");

            return 0;
        }
    }
}