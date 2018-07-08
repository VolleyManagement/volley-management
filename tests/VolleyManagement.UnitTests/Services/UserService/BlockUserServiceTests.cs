namespace VolleyManagement.UnitTests.Services.UserService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Contracts.Authorization;
    using Data.Contracts;
    using Data.Queries.User;
    using Domain.PlayersAggregate;
    using Xunit;
    using Moq;
    using Contracts;
    using Contracts.Exceptions;
    using Data.Queries.Common;
    using Domain.UsersAggregate;
    using VolleyManagement.Services.Authorization;
    using FluentAssertions;

    [ExcludeFromCodeCoverage]
    public class BlockUserServiceTests
    {
        private const int INVALID_USER_ID = -1;
        private const int VALID_USER_ID = 1;
        private const string TEST_NAME = "Test Name";
        private const bool USER_STATUS_IS_BLOCKED = true;
        private const bool USER_STATUS_IS_UNBLOCKED = false;

        private Mock<IUserRepository> _userRepositoryMock;

        private Mock<IAuthorizationService> _authorizationServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<ICacheProvider> _cacheProviderMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IQuery<User, FindByIdCriteria>> _getUserByIdQueryMock;
        private Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock;
        private Mock<IQuery<ICollection<User>, GetAllCriteria>> _getAllUserQueryMock;
        private Mock<IQuery<List<User>, UniqueUserCriteria>> _getUserListQueryMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;

        public BlockUserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _userServiceMock = new Mock<IUserService>();
            _cacheProviderMock = new Mock<ICacheProvider>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _getUserByIdQueryMock = new Mock<IQuery<User, FindByIdCriteria>>();
            _getPlayerByIdQueryMock = new Mock<IQuery<Player, FindByIdCriteria>>();
            _getAllUserQueryMock = new Mock<IQuery<ICollection<User>, GetAllCriteria>>();
            _getUserListQueryMock = new Mock<IQuery<List<User>, UniqueUserCriteria>>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();

            _userRepositoryMock.Setup(fr => fr.UnitOfWork)
                    .Returns(_unitOfWorkMock.Object);
        }

        [Fact]
        public void SetUserBlocked_UserExist_UpdatedUserReturned()
        {
            // Arrange
            var user = new BlockUserBuilder().WithId(VALID_USER_ID).Build();
            MockCurrentUser(user, VALID_USER_ID);
            var sut = BuildSUT();

            // Act
            sut.ChangeUserBlocked(VALID_USER_ID, true);

            // Assert
            VerifyEditUser(user, Times.Once());
        }

        [Fact]
        public void SetUserBlocked_UserExist_UserStatusIsBlocked()
        {
            // Arrange
            var user = new BlockUserBuilder().WithBlockStatus(USER_STATUS_IS_BLOCKED).Build();
            MockCurrentUser(user, VALID_USER_ID);
            var sut = BuildSUT();

            // Act
            sut.ChangeUserBlocked(VALID_USER_ID, true);

            // Assert
            VerifyEditUser(user, Times.Once());
        }

        [Fact]
        public void SetUserBlocked_UserDoesNotExist_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            MockUserServiceThrowsException(INVALID_USER_ID);
            var sut = BuildSUT();

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

        [Fact]
        public void SetUserUnblocked_UserExist_UpdatedUserReturned()
        {
            // Arrange
            var user = new BlockUserBuilder().WithId(VALID_USER_ID).Build();
            MockCurrentUser(user, VALID_USER_ID);
            var sut = BuildSUT();

            // Act
            sut.ChangeUserBlocked(VALID_USER_ID, false);

            // Assert
            VerifyEditUser(user, Times.Once());
        }

        [Fact]
        public void SetUserUnblocked_UserExist_UserStatusIsUnblocked()
        {
            // Arrange
            var user = new BlockUserBuilder().WithBlockStatus(USER_STATUS_IS_UNBLOCKED).Build();
            MockCurrentUser(user, VALID_USER_ID);
            var sut = BuildSUT();

            // Act
            sut.ChangeUserBlocked(VALID_USER_ID, false);

            // Assert
            VerifyEditUser(user, Times.Once());
        }

        [Fact]
        public void SetUserUnblocked_UserDoesNotExist_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            MockUserServiceThrowsException(INVALID_USER_ID);
            var sut = BuildSUT();

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

        private UserService BuildSUT()
        {
            return new UserService(
                _authorizationServiceMock.Object,
                _getUserByIdQueryMock.Object,
                _getAllUserQueryMock.Object,
                _getPlayerByIdQueryMock.Object,
                _cacheProviderMock.Object,
                _getUserListQueryMock.Object,
                _userRepositoryMock.Object,
                _currentUserServiceMock.Object);
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
            exception.Should().NotBeNull("There is no exception thrown");
            Assert.True(
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
