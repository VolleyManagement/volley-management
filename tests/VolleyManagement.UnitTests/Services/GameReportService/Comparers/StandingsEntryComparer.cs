namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GameReportsAggregate;
    using Xunit;
    using FluentAssertions;

    /// <summary>
    /// Represents a comparer for <see cref="StandingsEntry"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StandingsEntryComparer : IComparer<StandingsEntry>, IComparer, IEqualityComparer<StandingsEntry>
    {
        private bool HasComparerByPoints { get; set; } = true;
        private bool HasComparerByGames { get; set; } = true;
        private bool HasComparerBySets { get; set; } = true;
        private bool HasComparerByBalls { get; set; } = true;

        public void CleanComparerFlags()
        {
            HasComparerByPoints = false;
            HasComparerByGames = false;
            HasComparerBySets = false;
            HasComparerByBalls = false;
        }

        public void WithPointsComparer()
        {
            CleanComparerFlags();
            HasComparerByPoints = true;
        }

        public void WithGamesComparer()
        {
            CleanComparerFlags();
            HasComparerByGames = true;
        }

        public void WithSetsComparer()
        {
            CleanComparerFlags();
            HasComparerBySets = true;
        }

        public void WithBallsComparer()
        {
            CleanComparerFlags();
            HasComparerByBalls = true;
        }

        /// <summary>
        /// Compares two <see cref="StandingsEntry"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="StandingsEntry"/> x and y.</returns>
        public int Compare(StandingsEntry x, StandingsEntry y)
        {
            y.TeamName.Should().Be(x.TeamName, "TeamNames do not match");

            if (HasComparerByPoints)
            {
                PointsComparer(x, y);
            }
            if (HasComparerByGames)
            {
                GamesComparer(x, y);
            }
            if (HasComparerBySets)
            {
                SetsComparer(x, y);
            }
            if (HasComparerByBalls)
            {
                BallsComparer(x, y);
            }
            return 0;
        }

        public void PointsComparer(StandingsEntry x, StandingsEntry y)
        {
            y.Points.Should().Be(x.Points, "Points do not match");
        }

        public void GamesComparer(StandingsEntry x, StandingsEntry y)
        {
            y.GamesTotal.Should().Be(x.GamesTotal, "GamesTotal do not match");
            y.GamesWon.Should().Be(x.GamesWon, "GamesWon do not match");
            y.GamesLost.Should().Be(x.GamesLost, "GamesLost do not match");
        }

        public void SetsComparer(StandingsEntry x, StandingsEntry y)
        {
            y.GamesWithScoreThreeNil.Should().Be(x.GamesWithScoreThreeNil, "GamesWithScoreThreeNil do not match");
            y.GamesWithScoreThreeOne.Should().Be(x.GamesWithScoreThreeOne, "GamesWithScoreThreeOne do not match");
            y.GamesWithScoreThreeTwo.Should().Be(x.GamesWithScoreThreeTwo, "GamesWithScoreThreeTwo do not match");
            y.GamesWithScoreTwoThree.Should().Be(x.GamesWithScoreTwoThree, "GamesWithScoreTwoThree do not match");
            y.GamesWithScoreOneThree.Should().Be(x.GamesWithScoreOneThree, "GamesWithScoreOneThree do not match");
            y.GamesWithScoreNilThree.Should().Be(x.GamesWithScoreNilThree, "GamesWithScoreNilThree do not match");

            y.SetsWon.Should().Be(x.SetsWon, "SetsWon do not match");
            y.SetsLost.Should().Be(x.SetsLost, "SetsLost do not match");
        }

        public void BallsComparer(StandingsEntry x, StandingsEntry y)
        {
            y.BallsWon.Should().Be(x.BallsWon, "BallsWon do not match");
            y.BallsLost.Should().Be(x.BallsLost, "BallsLost do not match");
        }

        /// <summary>
        /// Compares two <see cref="StandingsEntry"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="StandingsEntry"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            var firstStandingsEntry = x as StandingsEntry;
            var secondStandingsEntry = y as StandingsEntry;

            if (firstStandingsEntry == null)
            {
                return -1;
            }
            if (secondStandingsEntry == null)
            {
                return 1;
            }

            return Compare(firstStandingsEntry, secondStandingsEntry);
        }

        public bool Equals(StandingsEntry x, StandingsEntry y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(StandingsEntry obj)
        {
            return obj.TeamId.GetHashCode();
        }
    }
}
