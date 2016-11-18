namespace VolleyManagement.UnitTests.Services.UserService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Data.Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.UI.Infrastructure;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class BlockUserServiceTests
    {
        private const int INVALID_USER_ID = -1;
        private const int VALID_USER_ID = 1;
        private const string TEST_NAME = "Test Name";
        private const bool USER_STATUS_IS_BLOCKED = true;
        private const bool USER_STATUS_IS_UNBLOCKED = false;

        private readonly Mock<IUserRepository> _userRepositoryMock =
            new Mock<IUserRepository>();

        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IQuery<User, FindByIdCriteria>> _getUserByIdQueryMock =
            new Mock<IQuery<User, FindByIdCriteria>>();

        private IKernel _kernel;

        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IUserService>().ToConstant(_userServiceMock.Object);
            _kernel.Bind<IUserRepository>().ToConstant(_userRepositoryMock.Object);
            _userRepositoryMock.Setup(fr => fr.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
            _kernel.Bind<IQuery<User, FindByIdCriteria>>()
                .ToConstant(_getUserByIdQueryMock.Object);
        }

        [TestMethod]
        public void SetUserBlocked_UserExist_UpdatedUserReturned()
        {
            // Arrange
            var user = new BlockUserBuilder().WithId(VALID_USER_ID).Build();
            MockCurrentUser(user, VALID_USER_ID);
            var sut = _kernel.Get<UserService>();

            // Act
            sut.SetUserBlocked(VALID_USER_ID);

            // Assert
            VerifyEditUser(user, Times.Once());
        }

        [TestMethod]
        public void SetUserBlocked_UserExist_UserStatusIsBlocked()
        {
            // Arrange
            var user = new BlockUserBuilder().WithBlockStatus(USER_STATUS_IS_BLOCKED).Build();
            MockCurrentUser(user, VALID_USER_ID);
            var sut = _kernel.Get<UserService>();

            // Act
            sut.SetUserBlocked(VALID_USER_ID);

            // Assert
            VerifyEditUser(user, Times.Once());
        }

        [TestMethod]
        public void SetUserBlocked_UserDoesNotExist_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            MockUserServiceThrowsException(INVALID_USER_ID);
            var sut = _kernel.Get<UserService>();

            // Act
            try
            {
                sut.SetUserBlocked(INVALID_USER_ID);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                "A user with specified identifier was not found");
        }

        [TestMethod]
        public void SetUserUnblocked_UserExist_UpdatedUserReturned()
        {
            // Arrange
            var user = new BlockUserBuilder().WithId(VALID_USER_ID).Build();
            MockCurrentUser(user, VALID_USER_ID);
            var sut = _kernel.Get<UserService>();

            // Act
            sut.SetUserUnblocked(VALID_USER_ID);

            // Assert
            VerifyEditUser(user, Times.Once());
        }

        [TestMethod]
        public void SetUserUnblocked_UserExist_UserStatusIsUnblocked()
        {
            // Arrange
            var user = new BlockUserBuilder().WithBlockStatus(USER_STATUS_IS_UNBLOCKED).Build();
            MockCurrentUser(user, VALID_USER_ID);
            var sut = _kernel.Get<UserService>();

            // Act
            sut.SetUserUnblocked(VALID_USER_ID);

            // Assert
            VerifyEditUser(user, Times.Once());
        }

        [TestMethod]
        public void SetUserUnblocked_UserDoesNotExist_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            MockUserServiceThrowsException(INVALID_USER_ID);
            var sut = _kernel.Get<UserService>();

            // Act
            try
            {
                sut.SetUserUnblocked(INVALID_USER_ID);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                "A user with specified identifier was not found");
        }

        private void MockCurrentUser(User testData, int userId)
        {
            _getUserByIdQueryMock
                .Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>()))
                .Returns(testData);
        }

        private void VerifyEditUser(User user, Times times)
        {
            _userRepositoryMock.Verify(
                ur => ur.Update(It.Is<User>(u => UsersAreEqual(u, user))),
                times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private bool UsersAreEqual(User x, User y)
        {
            return new BlockUserComparer().Compare(x, y) == 0;
        }

        private void VerifyExceptionThrown(
            Exception exception,
            string expectedMessage)
        {
            Assert.IsNotNull(exception, "There is no exception thrown");
            Assert.IsTrue(
                exception.Message.Equals(expectedMessage),
                "Expected and actual exceptions messages aren't equal");
        }

        private void MockUserServiceThrowsException(int userId)
        {
            _userServiceMock
                .Setup(tr => tr.GetUser(userId))
                .Throws<MissingEntityException>();
        }
    }
}
