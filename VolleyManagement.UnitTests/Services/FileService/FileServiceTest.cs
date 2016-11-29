namespace VolleyManagement.UnitTests.Services.FileService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
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

        [TestMethod]
        public void FileExists_FileDoesNotExists_ReturnFalse()
        {
            bool expected = false;

            // Arrange
            var sut = _kernel.Get<FileService>();

            // Act
            bool actual = sut.FileExists(null);

            // Assert
            Assert.AreEqual(expected, actual, "File exists");
        }

        #region Upload

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Upload_InvalidNullFile_FileLoadException()
        {
            // Arrange
            var sut = _kernel.Get<FileService>();

            // Act
            sut.Upload(null, null);
        }

        #endregion

        #region Delete

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Delete_FileDoesNotExist_ExceptionThrown()
        {
            // Arrange
            var sut = _kernel.Get<FileService>();

            // Act
            sut.Delete(null);
        }

        #endregion
    }
}