namespace VolleyManagement.UnitTests.Services.ContributorService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.Exceptions;
    using VolleyManagement.Services;
    using VolleyManagement.Domain.Contributors;

    /// <summary>
    /// Tests for TournamentService class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ContributorServiceTests
    {
        /// <summary>
        /// Test Fixture.
        /// </summary>
        private readonly ContributorServiceTestFixture _testFixture = new ContributorServiceTestFixture();

        /// <summary>
        /// Contributors Repository Mock.
        /// </summary>
        private readonly Mock<IContributorRepository> _contributorRepositoryMock = new Mock<IContributorRepository>();

        /// <summary>
        /// Unit of work mock.
        /// </summary>
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        /// <summary>
        /// IContributor service mock
        /// </summary>
        private readonly Mock<IContributorService> _contributorServiceMock = new Mock<IContributorService>();

        /// <summary>
        /// IoC for tests.
        /// </summary>
        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IContributorRepository>()
                   .ToConstant(_contributorRepositoryMock.Object);
            _contributorRepositoryMock.Setup(cn => cn.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// Test for Get() method. The method should return existing contributors
        /// </summary>
        [TestMethod]
        public void GetAll_ContributorsExist_ContributorsReturned()
        {
            // Arrange
            var testData = _testFixture.TestContributors()
                                       .Build();
            MockRepositoryFindAll(testData);
            var sut = _kernel.Get<ContributorService>();
            var expected = new ContributorServiceTestFixture()
                                            .TestContributors()
                                            .Build()
                                            .ToList();

            // Act
            var actual = sut.Get().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ContributorComparer());
        }

        /// <summary>
        /// Mocks Find method.
        /// </summary>
        /// <param name="testData">Test data to mock.</param>
        private void MockRepositoryFindAll(IEnumerable<Contributor> testData)
        {
            _contributorRepositoryMock.Setup(cn => cn.Find()).Returns(testData.AsQueryable());
        }

        /// <summary>
        /// Find out whether two players objects have the same property values.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if the contributors have the same property values.</returns>
        private bool ContributorsAreEqual(Contributor x, Contributor y)
        {
            return new ContributorComparer().Compare(x, y) == 0;
        }
    }
}
