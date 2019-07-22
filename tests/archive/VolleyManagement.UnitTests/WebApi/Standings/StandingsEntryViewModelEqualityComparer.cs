namespace VolleyManagement.UnitTests.WebApi.Standings
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Xunit;
    using UI.Areas.WebApi.ViewModels.GameReports;
    using FluentAssertions;

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
            var stringBuilder = new StringBuilder();

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
            actual.TeamName.Should().Be(expected.TeamName, $"[{messagePrefix}] TeamName should match");

            actual.Position.Should().Be(expected.Position, $"[{messagePrefix}Team:{expected.TeamName}] Position should match");
            actual.Points.Should().Be(expected.Points, $"[{messagePrefix}Team:{expected.TeamName}] Points should match");

            actual.GamesTotal.Should().Be(expected.GamesTotal, $"[{messagePrefix}Team:{expected.TeamName}] GamesTotal should match");
            actual.GamesWon.Should().Be(expected.GamesWon, $"[{messagePrefix}Team:{expected.TeamName}] GamesWon should match");
            actual.GamesLost.Should().Be(expected.GamesLost, $"[{messagePrefix}Team:{expected.TeamName}] GamesLost should match");

            actual.GamesWithScoreThreeNil.Should().Be(expected.GamesWithScoreThreeNil, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreThreeNil should match");
            actual.GamesWithScoreThreeOne.Should().Be(expected.GamesWithScoreThreeOne, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreThreeOne should match");
            actual.GamesWithScoreThreeTwo.Should().Be(expected.GamesWithScoreThreeTwo, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreThreeTwo should match");
            actual.GamesWithScoreTwoThree.Should().Be(expected.GamesWithScoreTwoThree, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreTwoThree should match");
            actual.GamesWithScoreOneThree.Should().Be(expected.GamesWithScoreOneThree, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreOneThree should match");
            actual.GamesWithScoreNilThree.Should().Be(expected.GamesWithScoreNilThree, $"[{messagePrefix}Team:{expected.TeamName}] GamesWithScoreNilThree should match");

            actual.SetsWon.Should().Be(expected.SetsWon, $"[{messagePrefix}Team:{expected.TeamName}] SetsWon should match");
            actual.SetsLost.Should().Be(expected.SetsLost, $"[{messagePrefix}Team:{expected.TeamName}] SetsLost should match");
            AssertFloatNullablesAreEqual(expected.SetsRatio, actual.SetsRatio, $"[{messagePrefix}Team:{expected.TeamName}] SetsRatio should match");

            actual.BallsWon.Should().Be(expected.BallsWon, $"[{messagePrefix}Team:{expected.TeamName}] BallsWon should match");
            actual.BallsLost.Should().Be(expected.BallsLost, $"[{messagePrefix}Team:{expected.TeamName}] BallsLost should match");
            AssertFloatNullablesAreEqual(expected.BallsRatio, actual.BallsRatio, $"[{messagePrefix}Team:{expected.TeamName}] BallsRatio should match");

            return true;
        }

        public static bool AssertFloatNullablesAreEqual(float? expected, float? actual, string message)
        {
            if (!expected.HasValue && !actual.HasValue)
            {
                return true;
            }

            expected.HasValue.Should().BeTrue();
            actual.HasValue.Should().BeTrue();

            actual.GetValueOrDefault().Should().BeApproximately(expected.GetValueOrDefault(), 0.001f, message);
            return true;
        }
    }
}
