namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using Domain.GameReportsAggregate;
    using Xunit;
    using FluentAssertions;

    /// <summary>
    /// As we've made SetsRatio and BallsRatio properties calculatable we have to test that
    /// </summary>
    public class StandingsEntryTests
    {
        [Fact]
        public void SetsRatio_NoWonNoLost_RatioNull()
        {
            // Arrange
            var entry = new StandingsEntry {
                SetsWon = 0,
                SetsLost = 0
            };

            // Act
            var actual = entry.SetsRatio;

            // Assert
            actual.Should().BeNull("Sets ratio should be null when no won no lost sets");
        }

        [Fact]
        public void SetsRatio_HasWonButNoLost_RatioIsInfinity()
        {
            // Arrange
            var entry = new StandingsEntry {
                SetsWon = 3,
                SetsLost = 0
            };

            // Act
            var actual = entry.SetsRatio;

            // Assert
            Assert.True(actual.HasValue, "Ratio should not be null");
            Assert.True(float.IsInfinity(actual.GetValueOrDefault()), "Sets ratio should be Infinity when there are no lost sets");
        }

        [Fact]
        public void SetsRatio_HasWonAndLost_RatioIsCalculatedProperly()
        {
            // Arrange
            var entry = new StandingsEntry {
                SetsWon = 3,
                SetsLost = 1
            };

            float expected = 3;

            // Act
            var actual = entry.SetsRatio;

            // Assert
            Assert.True(actual.HasValue, "Ratio should not be null");
            actual.GetValueOrDefault().Should().BeApproximately(expected, 0.001f, "Sets ratio should be properly calculated");
        }

        [Fact]
        public void BallsRatio_NoWonNoLost_RatioNull()
        {
            // Arrange
            var entry = new StandingsEntry {
                BallsWon = 0,
                BallsLost = 0
            };

            // Act
            var actual = entry.BallsRatio;

            // Assert
            actual.Should().BeNull("Balls ratio should be null when no won no lost balls");
        }

        [Fact]
        public void BallsRatio_HasWonButNoLost_RatioIsInfinity()
        {
            // Arrange
            var entry = new StandingsEntry {
                BallsWon = 3,
                BallsLost = 0
            };

            // Act
            var actual = entry.BallsRatio;

            // Assert
            Assert.True(actual.HasValue, "Ratio should not be null");
            Assert.True(float.IsInfinity(actual.GetValueOrDefault()), "Balls ratio should be Infinity when there are no lost balls");
        }

        [Fact]
        public void BallsRatio_HasWonAndLost_RatioIsCalculatedProperly()
        {
            // Arrange
            var entry = new StandingsEntry {
                BallsWon = 3,
                BallsLost = 1
            };

            float expected = 3;

            // Act
            var actual = entry.BallsRatio;

            // Assert
            Assert.True(actual.HasValue, "Ratio should not be null");
            actual.GetValueOrDefault().Should().BeApproximately(expected, 0.001f, "Balls ratio should be properly calculated");
        }
    }
}
