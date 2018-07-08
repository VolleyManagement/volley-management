namespace VolleyManagement.UnitTests.Services.FileService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Xunit;
    using VolleyManagement.Services;
    using FluentAssertions;

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
            Action act = () => sut.Delete(null);

            //Assert
            act.Should().Throw<FileNotFoundException>();
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
            Action act = () => sut.Upload(null, null);

            //Assert
            act.Should().Throw<ArgumentException>();
        }

        #endregion

        private FileService BuildSUT()
        {
            return new FileService();
        }
    }
}