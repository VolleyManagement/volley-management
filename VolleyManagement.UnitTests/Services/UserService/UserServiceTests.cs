namespace VolleyManagement.UnitTests.Services.UserService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Users;
    using VolleyManagement.Services;

    /// <summary>
    /// Tests for UserService class.
    /// </summary>
    [TestClass]
    public class UserServiceTests
    {
        /// <summary>
        /// Test Fixture.
        /// </summary>
        private readonly UserServiceTestFixture _testFixture = new UserServiceTestFixture();

        /// <summary>
        /// Tournaments Repository Mock.
        /// </summary>
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        /// <summary>
        /// Unit of work mock.
        /// </summary>
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

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
            this._kernel = new StandardKernel();
            this._kernel.Bind<IUserRepository>()
                   .ToConstant(this._userRepositoryMock.Object);
            this._userRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// GetAll Test.
        /// </summary>
        [TestMethod]
        public void GetAll_UsersExist_UsersReturned()
        {
            // Arrange
            var testData = this._testFixture.TestUsers()
                                       .Build();
            MockRepositoryFindAll(testData);
            var sut = this._kernel.Get<UserService>();
            var expected = new UserServiceTestFixture()
                                            .TestUsers()
                                            .Build()
                                            .ToList();

            // Act
            var actual = sut.GetAll().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new UserComparer());
        }

        /// <summary>
        /// Mocks FindAll method.
        /// </summary>
        /// <param name="testData">Test data to mock.</param>
        private void MockRepositoryFindAll(IEnumerable<User> testData)
        {
            _userRepositoryMock.Setup(tr => tr.FindAll()).Returns(testData.AsQueryable());
        }
    }
}
