namespace VolleyManagement.UnitTests.Services.FileService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
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

        public const int MAX_FILE_SIZE = 1000000;

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

        #region Upload

        [TestMethod]
        public void Upload_InvalidNullFile_FileLoadException()
        {
            // Arrange
            Exception exception = null;
            var sut = _kernel.Get<FileService>();

            // Act
            try
            {
                sut.Upload(1, null, null);
            }
            catch (NullReferenceException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                "File size must be less then 1 MB and greater then 0 MB");
        }

        #endregion

        #region Delete

        [TestMethod]
        public void Delete_FileDoesNotExist_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            var sut = _kernel.Get<FileService>();

            // Act
            try
            {
                sut.Delete(1, null);
            }
            catch (NullReferenceException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "Object reference not set to an instance of an object.");
        }

        #endregion

        #region Private methods

        private void VerifyExceptionThrown(Exception exception, string expectedMessage)
        {
            Assert.IsNotNull(exception, "There is no exception thrown");
            Assert.IsTrue(exception.Message.Equals(expectedMessage), "Expected and actual exceptions messages aren't equal");
        }

        #endregion
    }
}