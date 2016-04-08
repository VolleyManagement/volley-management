namespace VolleyManagement.UnitTests.Domain.GameAggregate
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.GamesAggregate;

    /// <summary>
    /// Tests for <see cref="ResultValidationTest"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ResultValidationTest
    {
        private const byte ZERO = 0;
        private const byte TWO = 2;
        private const byte THREE = 3;
        private const byte TWENTY_FIVE = 25;

        /// <summary>
        /// Test for IsSetsScoreValid method. Technical defeat set to true, score set to 3:0. Set score valid true returned.
        /// </summary>
        [TestMethod]
        public void IsSetsScoreValid_TechnicalDefeatTrueHomeValidScore_TrueReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(THREE, ZERO).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsSetsScoreValid(setsScore, true);

            // Assert
            Assert.IsTrue(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsSetsScoreValid method. Technical defeat set to true, score set to 0:3. Set score valid true returned.
        /// </summary>
        [TestMethod]
        public void IsSetsScoreValid_TechnicalDefeatTrueAwayValidScore_TrueReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(ZERO, THREE).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsSetsScoreValid(setsScore, true);

            // Assert
            Assert.IsTrue(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsSetsScoreValid method. Technical defeat set to true, score set to 3:2. Set score invalid false returned.
        /// </summary>
        [TestMethod]
        public void IsSetsScoreValid_TechnicalDefeatTrueHomeInvalidScore_FalseReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(THREE, TWO).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsSetsScoreValid(setsScore, true);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsSetsScoreValid method. Technical defeat set to true, score set to 2:3. Set score invalid false returned.
        /// </summary>
        [TestMethod]
        public void IsSetsScoreValid_TechnicalDefeatTrueAwayInvalidScore_FalseReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(TWO, THREE).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsSetsScoreValid(setsScore, true);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsSetsScoreValid method. Technical defeat set to false, score set to 3:0. Set score valid true returned.
        /// </summary>
        [TestMethod]
        public void IsSetsScoreValid_TechnicalDefeatFalseHomeValidScore_TrueReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(THREE, ZERO).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsSetsScoreValid(setsScore, false);

            // Assert
            Assert.IsTrue(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsSetsScoreValid method. Technical defeat set to false, score set to 3:3. Set score invalid false returned.
        /// </summary>
        [TestMethod]
        public void IsSetsScoreValid_TechnicalDefeatFalseInvalidScore_FalseReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(THREE, THREE).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsSetsScoreValid(setsScore, false);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsOptionalSetScoreValid method. Technical defeat set to true, score set to 0:0. Set score valid true returned.
        /// </summary>
        [TestMethod]
        public void IsOptionalSetScoreValid_TechnicalDefeatTrueValidScore_TrueReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(ZERO, ZERO).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsOptionalSetScoreValid(setsScore, true);

            // Assert
            Assert.IsTrue(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsOptionalSetScoreValid method. Technical defeat set to true, score set to 3:3. Set score invalid true returned.
        /// </summary>
        [TestMethod]
        public void IsOptionalSetScoreValid_TechnicalDefeatTrueInvalidScore_FalseReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(THREE, THREE).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsOptionalSetScoreValid(setsScore, true);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsOptionalSetScoreValid method. Technical defeat set to false, score set to 25:0. Set score valid true returned.
        /// </summary>
        [TestMethod]
        public void IsOptionalSetScoreValid_TechnicalDefeatFalseHomeValidScore_TrueReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(TWENTY_FIVE, ZERO).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsOptionalSetScoreValid(setsScore, false);

            // Assert
            Assert.IsTrue(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsOptionalSetScoreValid method. Technical defeat set to false, score set to 0:25. Set score valid true returned.
        /// </summary>
        [TestMethod]
        public void IsOptionalSetScoreValid_TechnicalDefeatFalseAwayValidScore_TrueReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(ZERO, TWENTY_FIVE).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsOptionalSetScoreValid(setsScore, false);

            // Assert
            Assert.IsTrue(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsOptionalSetScoreValid method. Technical defeat set to false, score set to 3:2. Set score invalid true returned.
        /// </summary>
        [TestMethod]
        public void IsOptionalSetScoreValid_TechnicalDefeatFalseInvalidScore_FalseReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(THREE, TWO).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsOptionalSetScoreValid(setsScore, false);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsRequiredSetScoreValid method. Technical defeat set to true, score set to 25:0. Set score valid true returned.
        /// </summary>
        [TestMethod]
        public void IsRequiredSetScoreValid_TechnicalDefeatTrueHomeValidScore_TrueReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(TWENTY_FIVE, ZERO).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsRequiredSetScoreValid(setsScore, true);

            // Assert
            Assert.IsTrue(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsRequiredSetScoreValid method. Technical defeat set to true, score set to 0:25. Set score valid true returned.
        /// </summary>
        [TestMethod]
        public void IsRequiredSetScoreValid_TechnicalDefeatTrueAwayValidScore_TrueReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(ZERO, TWENTY_FIVE).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsRequiredSetScoreValid(setsScore, true);

            // Assert
            Assert.IsTrue(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsRequiredSetScoreValid method. Technical defeat set to true, score set to 0:0. Set score invalid false returned.
        /// </summary>
        [TestMethod]
        public void IsRequiredSetScoreValid_TechnicalDefeatTrueInvalidScore_FalseReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(ZERO, ZERO).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsRequiredSetScoreValid(setsScore, true);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for IsRequiredSetScoreValid method. Technical defeat set to true, score set to 3:3. Set score invalid false returned.
        /// </summary>
        [TestMethod]
        public void IsRequiredSetScoreValid_TechnicalDefeatTrueAwayInvalidScoreThreeThree_FalseReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(THREE, THREE).Build();

            // Act
            var isSetScoreValid = ResultValidation.IsRequiredSetScoreValid(setsScore, true);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for AreSetScoresMatched method. Score set to 3:0 and set scores set to null. False returned.
        /// </summary>
        [TestMethod]
        public void AreSetScoresMatched_NullSetScores_FalseReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(THREE, ZERO).Build();

            // Act
            var isSetScoreValid = ResultValidation.AreSetScoresMatched(setsScore, null);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for AreSetScoresMatched method. Score set to 3:0 and set scores not match. False returned.
        /// </summary>
        [TestMethod]
        public void AreSetScoresMatched_NotMatchingSetScores_FalseReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(THREE, ZERO).Build();

            // Act
            var isSetScoreValid = ResultValidation.AreSetScoresMatched(setsScore, null);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for AreSetScoresMatched method. Score set to 3:2 and set scores match. True returned.
        /// </summary>
        [TestMethod]
        public void AreSetScoresMatched_HomeValidSetScores_TrueReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(THREE, TWO).Build();
            var setScores = new SetScoresTestFixture().Build();

            // Act
            var isSetScoreValid = ResultValidation.AreSetScoresMatched(setsScore, setScores);

            // Assert
            Assert.IsTrue(isSetScoreValid);
        }

        /// <summary>
        /// Test for AreSetScoresMatched method. Score set to 3:0 and set scores not match. False returned.
        /// </summary>
        [TestMethod]
        public void AreSetScoresMatched_InvalidThreeZeroSetScores_FalseReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(THREE, ZERO).Build();
            var setScores = new SetScoresTestFixture().Build();

            // Act
            var isSetScoreValid = ResultValidation.AreSetScoresMatched(setsScore, setScores);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for AreSetScoresMatched method. Score set to 0:0 and set scores not match. False returned.
        /// </summary>
        [TestMethod]
        public void AreSetScoresMatched_InvalidZeroZeroSetScores_FalseReturned()
        {
            // Arrange
            var setsScore = new SetsScoreBuilder().WithScore(ZERO, ZERO).Build();
            var setScores = new SetScoresTestFixture().Build();

            // Act
            var isSetScoreValid = ResultValidation.AreSetScoresMatched(setsScore, setScores);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for AreSetScoresOrdered method. Score set null. True returned.
        /// </summary>
        [TestMethod]
        public void AreSetScoresOrdered_SetScoresNull_TrueReturned()
        {
            // Act
            var isSetScoreValid = ResultValidation.AreSetScoresOrdered(null);

            // Assert
            Assert.IsTrue(isSetScoreValid);
        }

        /// <summary>
        /// Test for AreSetScoresOrdered method. Score set to 4:1. False returned.
        /// </summary>
        [TestMethod]
        public void AreSetScoresOrdered_HomeInvalidSetScores_FalseReturned()
        {
            // Arrange
            var setScores = new SetScoresTestFixture().WithFourOneSetScores().Build();

            // Act
            var isSetScoreValid = ResultValidation.AreSetScoresOrdered(setScores);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for AreSetScoresOrdered method. Score set to 1:4. False returned.
        /// </summary>
        [TestMethod]
        public void AreSetScoresOrdered_AwayInvalidSetScores_FalseReturned()
        {
            // Arrange
            var setScores = new SetScoresTestFixture().WithOneFourSetScores().Build();

            // Act
            var isSetScoreValid = ResultValidation.AreSetScoresOrdered(setScores);

            // Assert
            Assert.IsFalse(isSetScoreValid);
        }

        /// <summary>
        /// Test for AreSetScoresOrdered method. Score set to 1:3. True returned.
        /// </summary>
        [TestMethod]
        public void AreSetScoresOrdered_AwayValidSetScores_TrueReturned()
        {
            // Arrange
            var setScores = new SetScoresTestFixture().WithOneThreeSetScores().Build();

            // Act
            var isSetScoreValid = ResultValidation.AreSetScoresOrdered(setScores);

            // Assert
            Assert.IsTrue(isSetScoreValid);
        }
    }
}