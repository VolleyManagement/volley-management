namespace VolleyManagement.UnitTests.Services.FileService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VolleyManagement.Services;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FileServiceTests
    {
        #region Delete

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Delete_InvalidNullFile_FileNotFoundException()
        {
            // Arrange
            var sut = BuildSUT();

            // Act
            sut.Delete(null);
        }

        #endregion

        #region FileExists

        [TestMethod]
        public void FileExists_NoFile_FileNotFound()
        {
            var expected = false;

            // Arrange
            var sut = BuildSUT();

            // Act
            var actual = sut.FileExists(null);

            // Assert
            Assert.AreEqual(expected, actual, "There is no file on server");
        }

        #endregion

        #region Upload

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Upload_InvalidNullFile_ArgumentException()
        {
            // Arrange
            var sut = BuildSUT();

            // Act
            sut.Upload(null, null);
        }

        #endregion

        private FileService BuildSUT()
        {
            return new FileService();
        }
    }
}