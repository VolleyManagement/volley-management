﻿namespace VolleyManagement.UnitTests.Services.UserService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
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
            _kernel = new StandardKernel();
            _kernel.Bind<IUserRepository>()
                   .ToConstant(_userRepositoryMock.Object);
            _userRepositoryMock.Setup(tr => tr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        /// <summary>
        /// GetAll Test.
        /// </summary>
        [TestMethod]
        public void GetAll_UsersExist_UsersReturned()
        {
            // Arrange
            var testData = _testFixture.TestUsers()
                                       .Build();
            MockRepositoryFindAll(testData);
            var sut = _kernel.Get<UserService>();
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
        /// Test for FinById method.
        /// </summary>
        [TestMethod]
        public void FindById_ExistingUser_UserFound()
        {
            // Arrange
            var userService = _kernel.Get<UserService>();
            int id = 1;
            var user = new UserBuilder()
                .Build();
            MockRepositoryFindWhere(new List<User>() { user });

            //// Act
            var actualResult = userService.FindById(id);

            // Assert
            AssertExtensions.AreEqual<User>(user, actualResult, new UserComparer());
        }

        /// <summary>
        /// Test for FinById method. Null returned.
        /// </summary>
        [TestMethod]
        public void FindById_NotExistingUser_NullReturned()
        {
            // Arrange
            MockRepositoryFindWhere(new List<User>() { null });
            var userService = _kernel.Get<UserService>();

            // Act
            var tournament = userService.FindById(1);

            // Assert
            Assert.IsNull(tournament);
        }

        /// <summary>
        /// Test for Create() method. The method should create a new user.
        /// </summary>
        [TestMethod]
        public void Create_UserNotExist_UserCreated()
        {
            // Arrange
            var newUser = new UserBuilder().Build();

            // Act
            var sut = _kernel.Get<UserService>();
            sut.Create(newUser);

            // Assert
            _userRepositoryMock.Verify(
                ur => ur.Add(It.Is<User>(u => UsersAreEqual(u, newUser))));
            _unitOfWorkMock.Verify(u => u.Commit());
        }

        /// <summary>
        /// Test for Create() method where user name should be unique. The method should throw ArgumentException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_UserWithNonUniqueName_ExceptionThrown()
        {
            // Arrange
            var testData = new UserServiceTestFixture()
                                    .AddUser(new UserBuilder()
                                                        .WithId(10)
                                                        .WithUserName("User 5")
                                                        .WithEmail("test_1@email")
                                                        .Build())
                                    .Build();
            MockRepositoryFindWhere(testData);

            User nonUniqueNameUser = new UserBuilder()
                                                .WithId(0)
                                                .WithUserName("User 5")
                                                .WithEmail("test_2@email")
                                                .Build();

            // Act
            var sut = _kernel.Get<UserService>();
            sut.Create(nonUniqueNameUser);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Test for Create() method where user email should be unique. The method should throw ArgumentException
        /// and shouldn't invoke Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_UserWithNonUniqueEmail_ExceptionThrown()
        {
            // Arrange
            var testData = new UserServiceTestFixture()
                                    .AddUser(new UserBuilder()
                                                .WithId(10)
                                                .WithUserName("test user 1")
                                                .WithEmail("nonUnique@email")
                                                .Build())
                                                    .Build();
            MockRepositoryFindWhere(testData);

            User nonUniqueEmailUser = new UserBuilder()
                                                .WithId(0)
                                                .WithUserName("test user 2")
                                                .WithEmail("nonUnique@email")
                                                .Build();

            // Act
            var sut = _kernel.Get<UserService>();
            sut.Create(nonUniqueEmailUser);

            // Assert
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never());
        }

        /// <summary>
        /// Find out whether two user objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given users have the same properties.</returns>
        private bool UsersAreEqual(User x, User y)
        {
            UserComparer comparer = new UserComparer();
            return comparer.Compare(x, y) == 0;
        }

        /// <summary>
        /// Mocks FindWhere method.
        /// </summary>
        /// <param name="testData">Test data to mock.</param>
        private void MockRepositoryFindWhere(IEnumerable<User> testData)
        {
            _userRepositoryMock.Setup(ur => ur.FindWhere(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(testData.AsQueryable());
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