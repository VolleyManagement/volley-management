namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class StandingsDtoComparer : IComparer<StandingsDto>
    {
        public int Compare(StandingsDto x, StandingsDto y)
        {
            if (x.DivisionId != y.DivisionId)
            {
                throw new AssertFailedException("Division Ids do not match");
            }

            if (string.Compare(x.DivisionName, y.DivisionName, StringComparison.InvariantCulture) != 0)
            {
                throw new AssertFailedException("Division Names do not match");
            }

            if (x.Standings.Count == y.Standings.Count)
            {
                var standingsComparer = new StandingsEntryComparer();
                for (var i = 0; i < x.Standings.Count; i++)
                {
                    if (standingsComparer.Compare(x.Standings[i], y.Standings[i]) != 0)
                    {
                        return 1;
                    }
                }
            }
            else
            {
                throw new AssertFailedException($"[DivisionId={x.DivisionId}] Number of standing entries does not match.");
            }

            return 0;
        }
    }
}