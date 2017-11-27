namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.GameReportsAggregate;

    public class TournamentStandingsComparer<T> : IComparer<TournamentStandings<T>>
    {
        private readonly IComparer<T> _groupItemComparer;

        public TournamentStandingsComparer(IComparer<T> groupItemComparer)
        {
            _groupItemComparer = groupItemComparer;
        }

        public int Compare(TournamentStandings<T> x, TournamentStandings<T> y)
        {
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
                return 1;
            }

            return 0;
        }
    }
}