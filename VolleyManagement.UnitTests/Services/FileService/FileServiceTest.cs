namespace VolleyManagement.UnitTests.Services.FileService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Services;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FileServiceTest
    {
        #region Fields and constants

        public const int MAX_FILE_SIZE = 1048576;

        private readonly Mock<IFileService> _fileServiceMock
            = new Mock<IFileService>();

        private IKernel _kernel;

        #endregion

        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IFileService>().ToConstant(_fileServiceMock.Object);
        }

        #region Delete

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Delete_InvalidNullFile_FileNotFoundException()
        {
            // Arrange
            var sut = _kernel.Get<FileService>();

            // Act
            sut.Delete(null);
        }

        #endregion

        #region FileExists

        [TestMethod]
        public void FileExists_NoFile_FileNotFound()
        {
            bool expected = false;

            // Arrange
            var sut = _kernel.Get<FileService>();

            // Act
            bool actual = sut.FileExists(null);

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
            var sut = _kernel.Get<FileService>();

            // Act
            sut.Upload(null, null);
        }

        #endregion
    }
}