﻿namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.UnitTests.Services.GameReportService.Comparers;

    internal class TeamStandingsDtoComparer : IComparer<TeamStandingsDto>
    {
        private ComparerBy comparer;

        public TeamStandingsDtoComparer() : this(ComparerBy.All) { }

        public TeamStandingsDtoComparer(ComparerBy comparer)
        {
            this.comparer = comparer;
        }
        public int Compare(TeamStandingsDto x, TeamStandingsDto y)
        {
            Assert.AreEqual(x.TeamName, y.TeamName, "TeamName do not match");
            Assert.AreEqual(x.TeamId, y.TeamId, $"[Team:{x.TeamName}] TeamId do not match");

            if (comparer == ComparerBy.Points || comparer == ComparerBy.All)
            {
                Assert.AreEqual(x.Points, y.Points, $"[Team:{x.TeamName}] Points do not match");
            }
            if (comparer == ComparerBy.Sets || comparer == ComparerBy.All)
            {
                Assert.IsTrue(AreNullableFloatsEqual(x.SetsRatio, y.SetsRatio), $"[Team:{x.TeamName}] SetsRatio do not match. Actual:<{x.SetsRatio}>, Expected:<{y.SetsRatio}>");
            }
            if (comparer == ComparerBy.Balls || comparer == ComparerBy.All)
            {
                Assert.IsTrue(AreNullableFloatsEqual(x.BallsRatio, y.BallsRatio), $"[Team:{x.TeamName}] SetsRatio do not match. Actual:<{x.BallsRatio}>, Expected:<{y.BallsRatio}>");
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

            if (float.IsInfinity(xVal) && float.IsInfinity(yVal))
            {
                return true;
            }

            return Math.Abs(xVal - yVal) < 0.001;
        }
    }
}