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
            this._kernel = new StandardKernel();
            this._kernel.Bind<IContributorTeamService>()
                   .ToConstant(this._contributorTeamServiceMock.Object);
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

            ////this.MockContributors(testData);
            var sut = this._kernel.Get<ContributorsTeamController>();

            var expected = new ContributorTeamServiceTestFixture()
                                            .TestContributors()
                                            .Build()
                                            .ToList();

            // Act
            var actual = TestExtensions.GetModel<IEnumerable<ContributorTeam>>(sut.Index()).ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ContributorTeamComparer());
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
    }
}