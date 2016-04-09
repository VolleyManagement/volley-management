namespace VolleyManagement.UnitTests.Domain.TournamentsAggregate
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Tests for <see cref="GroupValidation"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GroupValidationTest
    {
        private const char CHAR_A = 'A';
        private const int MAX_NAME_LENGTH = 60;

        /// <summary>
        /// Test for ValidateName method. Valid name passed. True returned.
        /// </summary>
        [TestMethod]
        public void ValidateName_ValidName_ReturnsTrue()
        {
            // Arrange
            var groupName = new string(CHAR_A, MAX_NAME_LENGTH);

            // Act
            var isNameValid = GroupValidation.ValidateName(groupName);

            // Assert
            Assert.IsTrue(isNameValid);
        }

        /// <summary>
        /// Test for ValidateName method. Null passed to method. False returned.
        /// </summary>
        [TestMethod]
        public void ValidateName_NullName_ReturnsFalse()
        {
            // Act
            var isNameValid = GroupValidation.ValidateName(null);

            // Assert
            Assert.IsFalse(isNameValid);
        }

        /// <summary>
        /// Test for ValidateName method. Name with invalid length passed. False returned.
        /// </summary>
        [TestMethod]
        public void ValidateName_InvalidLengthName_ReturnsFalse()
        {
            // Arrange
            var groupName = new string(CHAR_A, MAX_NAME_LENGTH + 1);

            // Act
            var isNameValid = GroupValidation.ValidateName(groupName);

            // Assert
            Assert.IsFalse(isNameValid);
        }
    }
}