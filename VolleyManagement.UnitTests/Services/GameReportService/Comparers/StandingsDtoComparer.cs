namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.GameReportsAggregate;

    public class StandingsDtoComparer : IComparer<StandingsDto>
    {
        public int Compare(StandingsDto x, StandingsDto y)
        {
            if (x.DivisionId != y.DivisionId)
            {
                return 1;
            }

            if (string.Compare(x.DivisionName, y.DivisionName, StringComparison.InvariantCulture) != 0)
            {
                return 1;
            }

            if (x.Standings.Count == y.Standings.Count)
            {
                var standingsComparer = new StandingsEntryComparer();
                var xStands = x.Standings.OrderBy(s => s.TeamId).ToList();
                var yStands = y.Standings.OrderBy(s => s.TeamId).ToList();
                for (var i = 0; i < xStands.Count; i++)
                {
                    if (!standingsComparer.AreEqual(xStands[i], yStands[i]))
                    {
                        return 1;
                    }
                }
            }
            else
            {
                return 1;
            }

            return 0;
        }
    }
}