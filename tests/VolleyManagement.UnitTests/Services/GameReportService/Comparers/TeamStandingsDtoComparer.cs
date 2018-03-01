﻿namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System;
    using System.Collections.Generic;
    using Domain.GameReportsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    internal class TeamStandingsDtoComparer : IComparer<TeamStandingsDto>
    {
        public bool HasComparerByPoints { get; set; }
        public bool HasComparerBySets { get; set; }
        public bool HasComparerByBalls { get; set; }

        public TeamStandingsDtoComparer()
        {
            HasComparerByPoints = true;
            HasComparerBySets = true;
            HasComparerByBalls = true;
        }
        public int Compare(TeamStandingsDto x, TeamStandingsDto y)
        {
            Assert.AreEqual(x.TeamName, y.TeamName, "TeamName do not match");
            Assert.AreEqual(x.TeamId, y.TeamId, $"[Team:{x.TeamName}] TeamId do not match");

            if (HasComparerByPoints)
            {
                Assert.AreEqual(x.Points, y.Points, $"[Team:{x.TeamName}] Points do not match");
            }
            if (HasComparerBySets)
            {
                Assert.IsTrue(AreNullableFloatsEqual(x.SetsRatio, y.SetsRatio), $"[Team:{x.TeamName}] SetsRatio do not match. Actual:<{x.SetsRatio}>, Expected:<{y.SetsRatio}>");
            }
            if (HasComparerByBalls)
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