namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class TournamentStandingsComparer<T> : IComparer<TournamentStandings<T>>
    {
        private readonly IComparer<T> _groupItemComparer;

        public TournamentStandingsComparer(IComparer<T> groupItemComparer)
        {
            _groupItemComparer = groupItemComparer;
        }

        public int Compare(TournamentStandings<T> x, TournamentStandings<T> y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null || y == null)
            {
                throw new AssertFailedException("One instance is null");
            }

            if (x.Divisions.Count == y.Divisions.Count)
            {
                for (var i = 0; i < x.Divisions.Count; i++)
                {
                    if (_groupItemComparer.Compare(x.Divisions[i], y.Divisions[i]) != 0)
                    {
                        return 1;
                    }
                }
            }
            else
            {
                throw new AssertFailedException("Number of divisions do not match.");
            }

            return 0;
        }
    }
}