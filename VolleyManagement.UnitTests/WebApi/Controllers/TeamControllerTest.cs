namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Web.Http.Results;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.UI.Areas.WebApi.ApiControllers;

    /// <summary>
    /// Tests for TeamController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TeamControllerTest
    {
        private const int SPECIFIC_TEAM_ID = 2;
        private const string EXCEPTION_MESSAGE = "Test exception message.";

        /// <summary>
        /// Teams Service Mock
        /// </summary>
        private readonly Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();

        /// <summary>
        /// IoC for tests
        /// </summary>
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ITeamService>()
                   .ToConstant(_teamServiceMock.Object);
        }

        /// <summary>
        /// Test for Delete() method
        /// </summary>
        [TestMethod]
        public void Delete_ValidId_NoContentReturned()
        {
            // Arrange
            var controller = _kernel.Get<TeamsController>();

            // Act
            var response = controller.Delete(SPECIFIC_TEAM_ID) as StatusCodeResult;

            // Assert
            _teamServiceMock.Verify(ps => ps.Delete(It.Is<int>(id => id == SPECIFIC_TEAM_ID)), Times.Once());
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        /// <summary>
        /// Test for Delete() method
        /// </summary>
        [TestMethod]
        public void Delete_MissingEntityException_BadRequestReturned()
        {
            // Arrange
            var controller = _kernel.Get<TeamsController>();
            _teamServiceMock.Setup(ps => ps.Delete(It.IsAny<int>()))
                .Throws(new MissingEntityException(EXCEPTION_MESSAGE));

            // Act
            var response = controller.Delete(SPECIFIC_TEAM_ID) as BadRequestErrorMessageResult;

            // Assert
            _teamServiceMock.Verify(ps => ps.Delete(It.Is<int>(id => id == SPECIFIC_TEAM_ID)), Times.Once());
            Assert.IsNotNull(response);
            Assert.AreEqual<string>(response.Message, EXCEPTION_MESSAGE);
        }
    }
}
