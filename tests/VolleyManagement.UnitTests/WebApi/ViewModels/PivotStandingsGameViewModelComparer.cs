namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.WebApi.ViewModels.GameReports;

    [ExcludeFromCodeCoverage]
    internal static class PivotStandingsGameViewModelComparer
    {
        public static void AssertAreEqual(PivotStandingsGameViewModel expected, PivotStandingsGameViewModel actual, string messagePrefix = "")
        {
            Assert.AreEqual(expected.AwayTeamId, actual.AwayTeamId, $"{messagePrefix} AwayTeamId should match");
            Assert.AreEqual(expected.HomeTeamId, actual.HomeTeamId, $"{messagePrefix} HomeTeamId should match");

            Assert.AreEqual(expected.Results.Count, actual.Results.Count, $"{messagePrefix} AwayTeamId should match");

            var expectedResultsEnumerator = expected.Results.GetEnumerator();
            var actualResultsEnumerator = actual.Results.GetEnumerator();

            int i = 0;
            while (expectedResultsEnumerator.MoveNext() && actualResultsEnumerator.MoveNext())
            {
                string messagePrefix1 = $"{messagePrefix}[Result#{i}] ";
                AssertShortGameResultsAreEqual(expectedResultsEnumerator.Current, actualResultsEnumerator.Current, messagePrefix1);
                i++;
            }

            expectedResultsEnumerator.Dispose();
            actualResultsEnumerator.Dispose();
        }

        private static void AssertShortGameResultsAreEqual(
            ShortGameResultViewModel expectedResult,
            ShortGameResultViewModel actualResult,
            string messagePrefix = "")
        {
            Assert.AreEqual(expectedResult.HomeSetsScore, actualResult.HomeSetsScore, $"{messagePrefix} HomeSetsScore should match");
            Assert.AreEqual(expectedResult.AwaySetsScore, actualResult.AwaySetsScore, $"{messagePrefix} AwaySetsScore should match");
            Assert.AreEqual(expectedResult.IsTechnicalDefeat, actualResult.IsTechnicalDefeat, $"{messagePrefix} IsTechnicalDefeat should match");
        }
    }
}
