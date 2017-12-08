using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VolleyManagement.UnitTests.Services.GameReportService
{
    using Domain.GameReportsAggregate;

    /// <summary>
    /// As we've made SetsRatio and BallsRatio properties calculatable we have to test that
    /// </summary>
    [TestClass]
    public class StandingsEntryTests
    {
        [TestMethod]
        public void SetsRatio_NoWonNoLost_RatioIsNull()
        {
            // Arrange
            var entry = new StandingsEntry
            {
                SetsWon = 0,
                SetsLost = 0
            };

            // Act
            var actual = entry.SetsRatio;

            // Assert
            Assert.IsNull(actual, "Sets ratio should be null when no won no lost sets");
        }

        [TestMethod]
        public void SetsRatio_HasWonButNoLost_RatioIsInfinity()
        {
            // Arrange
            var entry = new StandingsEntry
            {
                SetsWon = 3,
                SetsLost = 0
            };

            // Act
            var actual = entry.SetsRatio;

            // Assert
            Assert.IsTrue(actual.HasValue, "Ratio should not be null");
            Assert.IsTrue(float.IsInfinity(actual.GetValueOrDefault()), "Sets ratio should be Infinity when there are no lost sets");
        }

        [TestMethod]
        public void SetsRatio_HasWonAndLost_RatioIsCalculatedProperly()
        {
            // Arrange
            var entry = new StandingsEntry
            {
                SetsWon = 3,
                SetsLost = 1
            };

            float expected = 3;

            // Act
            var actual = entry.SetsRatio;

            // Assert
            Assert.IsTrue(actual.HasValue, "Ratio should not be null");
            Assert.AreEqual(expected, actual.GetValueOrDefault(), 0.001f, "Sets ratio should be properly calculated");
        }

        [TestMethod]
        public void BallsRatio_NoWonNoLost_RatioIsNull()
        {
            // Arrange
            var entry = new StandingsEntry
            {
                BallsWon = 0,
                BallsLost = 0
            };

            // Act
            var actual = entry.BallsRatio;

            // Assert
            Assert.IsNull(actual, "Balls ratio should be null when no won no lost balls");
        }

        [TestMethod]
        public void BallsRatio_HasWonButNoLost_RatioIsInfinity()
        {
            // Arrange
            var entry = new StandingsEntry
            {
                BallsWon = 3,
                BallsLost = 0
            };

            // Act
            var actual = entry.BallsRatio;

            // Assert
            Assert.IsTrue(actual.HasValue, "Ratio should not be null");
            Assert.IsTrue(float.IsInfinity(actual.GetValueOrDefault()), "Balls ratio should be Infinity when there are no lost balls");
        }

        [TestMethod]
        public void BallsRatio_HasWonAndLost_RatioIsCalculatedProperly()
        {
            // Arrange
            var entry = new StandingsEntry
            {
                BallsWon = 3,
                BallsLost = 1
            };

            float expected = 3;

            // Act
            var actual = entry.BallsRatio;

            // Assert
            Assert.IsTrue(actual.HasValue, "Ratio should not be null");
            Assert.AreEqual(expected, actual.GetValueOrDefault(), 0.001f, "Balls ratio should be properly calculated");
        }
    }
}
