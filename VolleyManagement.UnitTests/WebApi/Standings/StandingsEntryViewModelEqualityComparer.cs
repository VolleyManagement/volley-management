namespace VolleyManagement.UnitTests.WebApi.Standings
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.WebApi.ViewModels.GameReports;

    /// <summary>
    /// Represents an equality comparer for <see cref="StandingsEntryViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StandingsEntryViewModelEqualityComparer : IEqualityComparer<StandingsEntryViewModel>
    {
        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the objects are considered equal; otherwise, false.
        /// If both x and y are null, the method returns true.</returns>
        public bool Equals(StandingsEntryViewModel x, StandingsEntryViewModel y)
        {
            return AssertAreEqual(x, y);
        }

        /// <summary>
        /// Gets hash code for the specified <see cref="StandingsEntryViewModel"/> object.
        /// </summary>
        /// <param name="obj"><see cref="StandingsEntryViewModel"/> object.</param>
        /// <returns>Hash code for the specified <see cref="StandingsEntryViewModel"/>.</returns>
        public int GetHashCode(StandingsEntryViewModel obj)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(obj.TeamName);
            stringBuilder.Append(obj.Position);
            stringBuilder.Append(obj.Points);
            stringBuilder.Append(obj.GamesTotal);
            stringBuilder.Append(obj.GamesWon);
            stringBuilder.Append(obj.GamesLost);
            stringBuilder.Append(obj.GamesWithScoreThreeNil);
            stringBuilder.Append(obj.GamesWithScoreThreeOne);
            stringBuilder.Append(obj.GamesWithScoreThreeTwo);
            stringBuilder.Append(obj.GamesWithScoreTwoThree);
            stringBuilder.Append(obj.GamesWithScoreOneThree);
            stringBuilder.Append(obj.GamesWithScoreNilThree);
            stringBuilder.Append(obj.SetsWon);
            stringBuilder.Append(obj.SetsLost);
            stringBuilder.Append(obj.SetsRatio);
            stringBuilder.Append(obj.BallsWon);
            stringBuilder.Append(obj.BallsLost);
            stringBuilder.Append(obj.BallsRatio);

            return stringBuilder.ToString().GetHashCode();
        }

        public static bool AssertAreEqual(StandingsEntryViewModel expected, StandingsEntryViewModel actual, string messagePrefix = "")
        {
            Assert.AreEqual(expected.TeamName, actual.TeamName, $"[{messagePrefix}] TeamName should match");

            Assert.AreEqual(expected.Position, actual.Position, $"[{messagePrefix}Team:{expected.TeamName}] Position should match");
            Assert.AreEqual(expected.Points, actual.Points, $"[{messagePrefix}Team:{expected.TeamName}] Points should match");

            Assert.AreEqual(expected.GamesTotal, actual.GamesTotal, $"[{messagePrefix}Team:{expected.TeamName}] GamesTotal should match");
            Assert.AreEqual(expected.GamesWon, actual.GamesWon, $"[{messagePrefix}Team:{expected.TeamName}] GamesWon should match");
            Assert.AreEqual(expected.GamesLost, actual.GamesLost, $"[{messagePrefix}Team:{expected.TeamName}] GamesLost should match");

            Assert.AreEqual(expected.GamesWithScoreThreeNil, actual.GamesWithScoreThreeNil, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreThreeNil should match");
            Assert.AreEqual(expected.GamesWithScoreThreeOne, actual.GamesWithScoreThreeOne, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreThreeOne should match");
            Assert.AreEqual(expected.GamesWithScoreThreeTwo, actual.GamesWithScoreThreeTwo, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreThreeTwo should match");
            Assert.AreEqual(expected.GamesWithScoreTwoThree, actual.GamesWithScoreTwoThree, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreTwoThree should match");
            Assert.AreEqual(expected.GamesWithScoreOneThree, actual.GamesWithScoreOneThree, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreOneThree should match");
            Assert.AreEqual(expected.GamesWithScoreNilThree, actual.GamesWithScoreNilThree, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreNilThree should match");

            Assert.AreEqual(expected.SetsWon, actual.SetsWon, $"[{messagePrefix}Team:{expected.TeamName}] SetsWon should match");
            Assert.AreEqual(expected.SetsLost, actual.SetsLost, $"[{messagePrefix}Team:{expected.TeamName}] SetsLost should match");
            AssertFloatNullablesAreEqual(expected.SetsRatio, actual.SetsRatio, $"[{messagePrefix}Team:{expected.TeamName}] SetsRatio should match");

            Assert.AreEqual(expected.BallsWon, actual.BallsWon, $"[{messagePrefix}Team:{expected.TeamName}] BallsWon should match");
            Assert.AreEqual(expected.BallsLost, actual.BallsLost, $"[{messagePrefix}Team:{expected.TeamName}] BallsLost should match");
            AssertFloatNullablesAreEqual(expected.BallsRatio, actual.BallsRatio, $"[{messagePrefix}Team:{expected.TeamName}] BallsRatio should match");

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
