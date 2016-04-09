namespace VolleyManagement.UnitTests.Domain.TournamentsAggregate
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Tests for <see cref="DivisionValidation"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DivisionValidationTest
    {
        private const char CHAR_A = 'A';
        private const int MAX_NAME_LENGTH = 60;

        /// <summary>
        /// Test for ValidateName method. Valid name passed. False returned.
        /// </summary>
        [TestMethod]
        public void ValidateName_ValidName_ReturnsFalse()
        {
            // Arrange
            var divisionName = new string(CHAR_A, MAX_NAME_LENGTH);

            // Act
            var isNameValid = DivisionValidation.ValidateName(divisionName);

            // Assert
            Assert.IsFalse(isNameValid);
        }

        /// <summary>
        /// Test for ValidateName method. Null passed to method. True returned.
        /// </summary>
        [TestMethod]
        public void ValidateName_NullName_ReturnsTrue()
        {
            // Act
            var isNameValid = DivisionValidation.ValidateName(null);

            // Assert
            Assert.IsTrue(isNameValid);
        }

        /// <summary>
        /// Test for ValidateName method. Name with invalid length passed. True returned.
        /// </summary>
        [TestMethod]
        public void ValidateName_InvalidLengthName_ReturnsTrue()
        {
            // Arrange
            var divisionName = new string(CHAR_A, MAX_NAME_LENGTH + 1);

            // Act
            var isNameValid = DivisionValidation.ValidateName(divisionName);

            // Assert
            Assert.IsTrue(isNameValid);
        }
    }
}
