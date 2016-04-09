namespace VolleyManagement.UnitTests.Domain.TournamentsAggregate
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Tests for <see cref="Group"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class GroupTest
    {
        /// <summary>
        /// Test for SetName method. Null passed to method expected exception ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetName_NullName_ThrowArgumentException()
        {
            // Arrange
            var sut = new Group();

            // Act
            sut.Name = null;
        }
    }
}