namespace VolleyManagement.UnitTests.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web.Http.Results;
    using System.Web.OData.Results;

    using Contracts;
    using Domain.ContributorsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using Services.ContributorService;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.UI.Areas.WebApi.ApiControllers;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Contributors;
    using VolleyManagement.UnitTests.WebApi.ViewModels;

    /// <summary>
    /// Tests for ContributorsController class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ContributorsControllerTests
    {
        /// <summary>
        /// Test Fixture
        /// </summary>
        private readonly ContributorServiceTestFixture _testFixture = new ContributorServiceTestFixture();

        /// <summary>
        /// Contributors Service Mock
        /// </summary>
        private readonly Mock<IContributorService> _contributorServiceMock = new Mock<IContributorService>();

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
            _kernel.Bind<IContributorService>()
                   .ToConstant(_contributorServiceMock.Object);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing contributor
        /// </summary>
        [TestMethod]
        public void Get_ContributorsExist_ContributorsReturned()
        {
            // Arrange
            var testData = _testFixture.TestContributors()
                                            .Build();
            //MockContributors(testData);
            var sut = _kernel.Get<ContributorsController>();

            //// Expected result
            var expected = new ContributorViewModelServiceTestFixture()
                                            .TestContributor()
                                            .Build()
                                            .ToList();

            //// Actual result
            var actual = sut.GetContributors().ToList();

            //// Assert
            _contributorServiceMock.Verify(cn => cn.Get(), Times.Once());
            CollectionAssert.AreEqual(expected, actual, new ContributorViewModelComparer());
        }

        /// <summary>
        /// Test for Map() method.
        /// The method should map contributor domain model to view model.
        /// </summary>
        [TestMethod]
        public void Map_ContributorAsParam_MappedToViewModelWebApi()
        {
            // Arrange
            var contributor = new ContributorBuilder()
                                        .WithId(1)
                                        .WithName("Name")
                                        .WithContributorTeamId(1)
                                        .Build();
            var expected = new ContributorViewModelBuilder()
                                        .WithId(1)
                                        .WithName("FirstName")
                                        .WithContributorTeamId(1)
                                        .Build();

            // Act
            var actual = ContributorViewModel.Map(contributor);

            // Assert
            AssertExtensions.AreEqual<ContributorViewModel>(expected, actual, new ContributorViewModelComparer());
        }

        ///// <summary>
        ///// Mock the Contributors
        ///// </summary>
        ///// <param name="testData">Data what will be returned</param>
        //private void MockContributors(IList<Contributor> testData)
        //{
        //    _contributorServiceMock.Setup(cn => cn.Get())
        //                                    .Returns(testData.AsQueryable());
        //}
    }
}
