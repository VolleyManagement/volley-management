namespace VolleyManagement.UnitTests.Services.GameReportService.Comparers
{  
    using System.Collections.Generic;
    using VolleyManagement.Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    class TeamStandingsDtoComparerByPoints : IComparer<TeamStandingsDto>
    {
        public int Compare(TeamStandingsDto x, TeamStandingsDto y)
        {
            Assert.AreEqual(x.TeamName, y.TeamName, "TeamName do not match");
            Assert.AreEqual(x.TeamId, y.TeamId, $"[Team:{x.TeamName}] TeamId do not match");

            Assert.AreEqual(x.Points, y.Points, $"[Team:{x.TeamName}] Points do not match");

            return 0;
        }
    }
}