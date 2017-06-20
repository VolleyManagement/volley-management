namespace VolleyManagement.UnitTests.Services.UserService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.User;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.Services.Authorization;

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

        private readonly Mock<IAuthorizationService> _authorizationServiceMock =
            new Mock<IAuthorizationService>();

        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<ICacheProvider> _cacheProviderMock = new Mock<ICacheProvider>();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IQuery<User, FindByIdCriteria>> _getUserByIdQueryMock =
            new Mock<IQuery<User, FindByIdCriteria>>();

        private readonly Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock =
            new Mock<IQuery<Player, FindByIdCriteria>>();

        private readonly Mock<IQuery<List<User>, GetAllCriteria>> _getAllUserQueryMock =
           new Mock<IQuery<List<User>, GetAllCriteria>>();

        private readonly Mock<IQuery<List<User>, UniqueUserCriteria>> _getUserListQueryMock =
           new Mock<IQuery<List<User>, UniqueUserCriteria>>();

        private IKernel _kernel;

        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IUserService>().ToConstant(_userServiceMock.Object);
            _kernel.Bind<IQuery<List<User>, UniqueUserCriteria>>().ToConstant(_getUserListQueryMock.Object);
            _kernel.Bind<ICacheProvider>().ToConstant(_cacheProviderMock.Object);
            _kernel.Bind<IUserRepository>().ToConstant(_userRepositoryMock.Object);
            _kernel.Bind<IAuthorizationService>().ToConstant(_authorizationServiceMock.Object);
            _userRepositoryMock.Setup(fr => fr.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
            _kernel.Bind<IQuery<User, FindByIdCriteria>>()
                .ToConstant(_getUserByIdQueryMock.Object);
            _kernel.Bind<IQuery<Player, FindByIdCriteria>>()
                .ToConstant(_getPlayerByIdQueryMock.Object);
            _kernel.Bind<IQuery<List<User>, GetAllCriteria>>()
                .ToConstant(_getAllUserQueryMock.Object);
        }

        [TestMethod]
        public void SetUserBlocked_UserExist_UpdatedUserReturned()
        {
            // Arrange
            var user = new BlockUserBuilder().WithId(VALID_USER_ID).Build();
            MockCurrentUser(user, VALID_USER_ID);
            var sut = _kernel.Get<UserService>();

            // Act
            sut.ChangeUserBlocked(VALID_USER_ID, true);

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
            sut.ChangeUserBlocked(VALID_USER_ID, true);

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
                sut.ChangeUserBlocked(INVALID_USER_ID, true);
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
            sut.ChangeUserBlocked(VALID_USER_ID, false);

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
            sut.ChangeUserBlocked(VALID_USER_ID, false);

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
                sut.ChangeUserBlocked(INVALID_USER_ID, false);
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
