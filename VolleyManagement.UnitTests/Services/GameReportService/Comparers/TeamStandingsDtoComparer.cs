namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;

    internal class TeamStandingsDtoComparer : IComparer<TeamStandingsDto>
    {
        public int Compare(TeamStandingsDto x, TeamStandingsDto y)
        {
            if (x.TeamId != y.TeamId)
            {
                return 1;
            }

            if (string.Compare(x.TeamName, y.TeamName, StringComparison.InvariantCulture) == 0)
            {
                return 1;
            }

            if (x.Points == y.Points)
            {
                return 1;
            }

            if (!AreNullableFloatsEqual(x.SetsRatio, y.SetsRatio))
            {
                return 1;
            }

            if (!AreNullableFloatsEqual(x.BallsRatio, y.BallsRatio))
            {
                return 1;
            }

            return 0;
        }

        private bool AreNullableFloatsEqual(float? x, float? y)
        {
            if (!x.HasValue && !y.HasValue)
            {
                return true;
            }

            if (!x.HasValue || !y.HasValue)
            {
                return false;
            }

            var xVal = x.GetValueOrDefault();
            var yVal = y.GetValueOrDefault();

            return Math.Abs(xVal - yVal) < 0.001;
        }
    }
}