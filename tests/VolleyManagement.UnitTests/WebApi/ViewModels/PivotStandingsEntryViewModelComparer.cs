namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.WebApi.ViewModels.GameReports;

    [ExcludeFromCodeCoverage]
    internal class PivotStandingsEntryViewModelComparer
    {
        public static void AssertAreEqual(PivotStandingsTeamViewModel expected, PivotStandingsTeamViewModel actual, string messagePrefix = "")
        {
            Assert.AreEqual(expected.TeamName, actual.TeamName, $"{messagePrefix}TeamName should match");
            Assert.AreEqual(expected.TeamId, actual.TeamId, $"{messagePrefix}[TeamName{expected.TeamName}] TeamId should match");
            Assert.AreEqual(expected.Points, actual.Points, $"{messagePrefix}[TeamName{expected.TeamName}] Points should match");
            Assert.AreEqual(
                expected.SetsRatio.GetValueOrDefault(),
                actual.SetsRatio.GetValueOrDefault(),
                0.001f,
                $"{messagePrefix}[TeamName{expected.TeamName}] SetsRatio should match");
        }
    }
}
