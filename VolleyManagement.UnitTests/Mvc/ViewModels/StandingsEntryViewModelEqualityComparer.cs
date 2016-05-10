namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;

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
            return x.TeamName == y.TeamName
                && x.Position == y.Position
                && x.Points == y.Points
                && x.GamesTotal == y.GamesTotal
                && x.GamesWon == y.GamesWon
                && x.GamesLost == y.GamesLost
                && x.GamesWithScoreThreeNil == y.GamesWithScoreThreeNil
                && x.GamesWithScoreThreeOne == y.GamesWithScoreThreeOne
                && x.GamesWithScoreThreeTwo == y.GamesWithScoreThreeTwo
                && x.GamesWithScoreTwoThree == y.GamesWithScoreTwoThree
                && x.GamesWithScoreOneThree == y.GamesWithScoreOneThree
                && x.GamesWithScoreNilThree == y.GamesWithScoreNilThree
                && x.SetsWon == y.SetsWon
                && x.SetsLost == y.SetsLost
                && x.SetsRatio == y.SetsRatio
                && x.BallsWon == y.BallsWon
                && x.BallsLost == y.BallsLost
                && x.BallsRatio == y.BallsRatio;
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
    }
}
