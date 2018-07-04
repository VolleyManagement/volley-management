namespace VolleyManagement.UnitTests.Services.FileService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using FluentAssertions;
    using VolleyManagement.Services;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class FileServiceTests
    {
        #region Delete

        [Fact]
        public void Delete_InvalidNullFile_FileNotFoundException()
        {
            // Arrange
            var sut = BuildSUT();

            // Act
            Assert.Throws<FileNotFoundException>(() => sut.Delete(null));
        }

        #endregion

        #region FileExists

        [Fact]
        public void FileExists_NoFile_FileNotFound()
        {
            var expected = false;

            // Arrange
            var sut = BuildSUT();

            // Act
            var actual = sut.FileExists(null);

            // Assert
            actual.Should().Be(expected, "There is no file on server");
        }

        #endregion

        #region Upload

        [Fact]
        public void Upload_InvalidNullFile_ArgumentException()
        {
            // Arrange
            var sut = BuildSUT();

            // Act
            Assert.Throws<ArgumentException>(() => sut.Upload(null, null));
        }

        #endregion

        private FileService BuildSUT()
        {
            return new FileService();
        }
    }
}