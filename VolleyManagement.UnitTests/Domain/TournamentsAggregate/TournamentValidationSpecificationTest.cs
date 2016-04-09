namespace VolleyManagement.UnitTests.Domain.TournamentsAggregate
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Tests for <see cref="TournamentValidationSpecification"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentValidationSpecificationTest
    {
        /// <summary>
        /// Test for IsSatisfiedBy method. Valid data passed. Returns True.
        /// </summary>
        [TestMethod]
        public void IsSatisfiedBy_ValidTournament_ReturnsTrue()
        {
            // Arrange
            var tournament = new TournamentBuilder().Build();
            var sut = new TournamentValidationSpecification();

            // Act
            bool isSatisfied = sut.IsSatisfiedBy(tournament);

            // Assert
            Assert.IsTrue(isSatisfied);
        }

        /// <summary>
        /// Test for IsSatisfiedBy method. Null name passed. Returns False.
        /// </summary>
        [TestMethod]
        public void IsSatisfiedBy_NullName_ReturnsFalse()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithNullName().Build();
            var sut = new TournamentValidationSpecification();

            // Act
            bool isSatisfied = sut.IsSatisfiedBy(tournament);

            // Assert
            Assert.IsFalse(isSatisfied);
        }

        /// <summary>
        /// Test for IsSatisfiedBy method. Name with invalid length passed. Returns False.
        /// </summary>
        [TestMethod]
        public void IsSatisfiedBy_InvalidNameLength_ReturnsFalse()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithInvalidNameLength().Build();
            var sut = new TournamentValidationSpecification();

            // Act
            bool isSatisfied = sut.IsSatisfiedBy(tournament);

            // Assert
            Assert.IsFalse(isSatisfied);
        }

        /// <summary>
        /// Test for IsSatisfiedBy method. Null description passed. Returns False.
        /// </summary>
        [TestMethod]
        public void IsSatisfiedBy_DescriptionNull_ReturnsTrue()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithDescriptionNull().Build();
            var sut = new TournamentValidationSpecification();

            // Act
            bool isSatisfied = sut.IsSatisfiedBy(tournament);

            // Assert
            Assert.IsTrue(isSatisfied);
        }

        /// <summary>
        /// Test for IsSatisfiedBy method. Description with invalid length passed. Returns False.
        /// </summary>
        [TestMethod]
        public void IsSatisfiedBy_InvalidDescriptionLength_ReturnsFalse()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithInvalidDescriptionLength().Build();
            var sut = new TournamentValidationSpecification();

            // Act
            bool isSatisfied = sut.IsSatisfiedBy(tournament);

            // Assert
            Assert.IsFalse(isSatisfied);
        }

        /// <summary>
        /// Test for IsSatisfiedBy method. Null regulation link  passed. Returns False.
        /// </summary>
        [TestMethod]
        public void IsSatisfiedBy_RegulationLinkNull_ReturnsTrue()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithNullRegulationLink().Build();
            var sut = new TournamentValidationSpecification();

            // Act
            bool isSatisfied = sut.IsSatisfiedBy(tournament);

            // Assert
            Assert.IsTrue(isSatisfied);
        }

        /// <summary>
        /// Test for IsSatisfiedBy method. Regulation link with invalid length passed. Returns False.
        /// </summary>
        [TestMethod]
        public void IsSatisfiedBy_InvalidRegulationLinkLength_ReturnsFalse()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithInvalidRegulationLinkLength().Build();
            var sut = new TournamentValidationSpecification();

            // Act
            bool isSatisfied = sut.IsSatisfiedBy(tournament);

            // Assert
            Assert.IsFalse(isSatisfied);
        }

        /// <summary>
        /// Test for IsSatisfiedBy method. Invalid minimum season year passed. Returns False.
        /// </summary>
        [TestMethod]
        public void IsSatisfiedBy_InvalidSeasonMinYear_ReturnsFalse()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithInvalidSeasonMin().Build();
            var sut = new TournamentValidationSpecification();

            // Act
            bool isSatisfied = sut.IsSatisfiedBy(tournament);

            // Assert
            Assert.IsFalse(isSatisfied);
        }

        /// <summary>
        /// Test for IsSatisfiedBy method. Invalid maximum season year passed. Returns False.
        /// </summary>
        [TestMethod]
        public void IsSatisfiedBy_InvalidSeasonMaxYear_ReturnsFalse()
        {
            // Arrange
            var tournament = new TournamentBuilder().WithInvalidSeasonMax().Build();
            var sut = new TournamentValidationSpecification();

            // Act
            bool isSatisfied = sut.IsSatisfiedBy(tournament);

            // Assert
            Assert.IsFalse(isSatisfied);
        }
    }
}