namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.Mvc.ViewModels.GameReports;

    /// <summary>
    /// Represents an equality comparer for <see cref="PivotTeamStandingsViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotTeamStandingsViewModelEqualityComparer : IEqualityComparer<PivotTeamStandingsViewModel>
    {
        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the objects are considered equal; otherwise, false.</returns>
        public bool Equals(PivotTeamStandingsViewModel x, PivotTeamStandingsViewModel y)
        {
            return AssertAreEqual(x, y);
        }

        /// <summary>
        /// Gets hash code for the specified <see cref="PivotTeamStandingsViewModel"/> object.
        /// </summary>
        /// <param name="obj"><see cref="PivotTeamStandingsViewModel"/> object.</param>
        /// <returns>Hash code for the specified <see cref="PivotTeamStandingsViewModel"/>.</returns>
        public int GetHashCode(PivotTeamStandingsViewModel obj)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(obj.TeamId);
            stringBuilder.Append(obj.TeamName);
            stringBuilder.Append(obj.Points);
            stringBuilder.Append(obj.SetsRatio);
            stringBuilder.Append(obj.Position);
            stringBuilder.Append(obj.BallsRatio);

            return stringBuilder.ToString().GetHashCode();
        }

        public static bool AssertAreEqual(PivotTeamStandingsViewModel expected, PivotTeamStandingsViewModel actual, string messagePrefix = "")
        {
            Assert.AreEqual(expected.TeamId, actual.TeamId, $"{messagePrefix} TeamId should match");
            Assert.AreEqual(expected.TeamName, actual.TeamName, $"{messagePrefix} TeamName should match");
            Assert.AreEqual(expected.Points, actual.Points, $"{messagePrefix} Points should match");
            AssertFloatNullablesAreEqual(expected.SetsRatio, actual.SetsRatio, $"{messagePrefix} SetsRatio should match");
            Assert.AreEqual(expected.Position, actual.Position, $"{messagePrefix} Position should match");
            AssertFloatNullablesAreEqual(expected.BallsRatio, actual.BallsRatio, $"{messagePrefix} BallsRatio should match");
            return true;
        }

        public static bool AssertFloatNullablesAreEqual(float? expected, float? actual, string message)
        {
            if (!expected.HasValue && !actual.HasValue)
            {
                return true;
            }

            if (!expected.HasValue || !actual.HasValue)
            {
                Assert.Fail($"{message}. Expected: <{expected}>, Actual: <{actual}>");
            }

            Assert.AreEqual(expected.GetValueOrDefault(), actual.GetValueOrDefault(), 0.001f, message);
            return true;
        }
    }
}
