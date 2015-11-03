namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Contracts;
    using Contracts.Exceptions;
    using Domain.ContributorsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;

    using VolleyManagement.UI.Areas.Mvc.Controllers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.ContributorsTeam;
    using VolleyManagement.UnitTests.Mvc.ViewModels;
    using VolleyManagement.UnitTests.Services.ContributorService;

    /// <summary>
    /// Tests for MVC ContributorTeamController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ContributorsTeamControllerTests
    {
        private readonly ContributorTeamServiceTestFixture _testFixture = new ContributorTeamServiceTestFixture();
        private readonly Mock<IContributorTeamService> _contributorTeamServiceMock = new Mock<IContributorTeamService>();

        private IKernel _kernel;

        /// <summary>
        /// Initializes test data
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IContributorTeamService>()
                   .ToConstant(_contributorTeamServiceMock.Object);
        }

        /// <summary>
        /// Test for Index action. The action should return not empty contributors team list
        /// </summary>
        [TestMethod]
        public void Index_ContributorsExist_ContributorsReturned()
        {
            // Arrange
            var testData = _testFixture.TestContributors()
                                       .Build();
            MockContributorsTeam(testData);
            var sut = this._kernel.Get<ContributorsTeamController>();

            var expected = CreateContributorsTeamViewModelList().ToList();

            // Act
            var actual = TestExtensions.GetModel<IEnumerable<ContributorsTeamViewModel>>(sut.Index()).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ContributorTeamViewModelNonGenericComparer());
        }

        /// <summary>
        /// Test with negative scenario for Index action.
        /// The action should thrown Argument null exception
        /// </summary>
        [TestMethod]
        public void Index_ContributorsDoNotExist_ExceptionThrown()
        {
            // Arrange
            this._contributorTeamServiceMock.Setup(tr => tr.Get())
                .Throws(new ArgumentNullException());

            var sut = this._kernel.Get<ContributorsTeamController>();
            var expected = (int)HttpStatusCode.NotFound;

            // Act
            var actual = (sut.Index() as HttpNotFoundResult).StatusCode;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Mocks test data
        /// </summary>
        /// <param name="testData">Data to mock</param>
        private void MockContributorsTeam(IEnumerable<ContributorTeam> testData)
        {
            this._contributorTeamServiceMock.Setup(cn => cn.Get())
                .Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Creates new list of ContributorsTeamViewModel test data
        /// </summary>
        /// <returns>List of ContributorsTeamViewModel</returns>
        private IList<ContributorsTeamViewModel> CreateContributorsTeamViewModelList()
        {
            var contributorTeams = _testFixture.TestContributors().Build();
            return contributorTeams.Select(c => new ContributorTeamMvcViewModelBuilder()
                                                .WithId(c.Id)
                                                .WithName(c.Name)
                                                .WithcourseDirection(c.CourseDirection)
                                                .Withcontributors(c.Contributors.ToList())
                                                .Build()).ToList();
        }
    }
}