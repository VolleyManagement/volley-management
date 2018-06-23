using System;
using FluentAssertions;

namespace VolleyManagement.UnitTests.Services.UsersService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Moq;
    using System.Collections;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Queries.Common;
    using Data.Queries.User;
    using Domain.PlayersAggregate;
    using Domain.RolesAggregate;
    using Domain.UsersAggregate;
    using VolleyManagement.Services.Authorization;
    using PlayerService;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class UserServiceTests
    {
        private const int EXISTING_ID = 1;

        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ICacheProvider> _cacheProviderMock;
        private Mock<IQuery<ICollection<User>, GetAllCriteria>> _getAllQueryMock;
        private Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock;
        private Mock<IQuery<User, FindByIdCriteria>> _getByIdQueryMock;
        private Mock<IQuery<ICollection<User>, UniqueUserCriteria>> _getAdminsListQueryMock;
        private Mock<ICurrentUserService> _currentUserServiceMock;

        private UserServiceTestFixture _testFixture = new UserServiceTestFixture();

        /// <summary>
        /// Initializes test data.
        /// </summary>
        public UserServiceTests()
        {
            _authServiceMock = new Mock<IAuthorizationService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _cacheProviderMock = new Mock<ICacheProvider>();
            _getAllQueryMock = new Mock<IQuery<ICollection<User>, GetAllCriteria>>();
            _getPlayerByIdQueryMock = new Mock<IQuery<Player, FindByIdCriteria>>();
            _getByIdQueryMock = new Mock<IQuery<User, FindByIdCriteria>>();
            _getAdminsListQueryMock = new Mock<IQuery<ICollection<User>, UniqueUserCriteria>>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
        }

        [Fact]
        public void GetAll_UsersExist_UsersReturned()
        {
            // Arrange
            var testData = _testFixture.TestUsers().Build();
            MockGetAllUsersQuery(testData);

            var expected = new UserServiceTestFixture()
                                            .TestUsers()
                                            .Build();

            var sut = BuildSUT();

            // Act
            var actual = sut.GetAllUsers();

            // Assert
            TestHelper.AreEqual(expected, actual, new UserComparer());
        }

        [Fact]
        public void GetAll_NoViewRights_AuthorizationExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsException(AuthOperations.AllUsers.ViewList);

            var sut = BuildSUT();

            // Act => Assert
            Action act = () => sut.GetAllUsers();
            act.Should().Throw<AuthorizationException>("Requested operation is not allowed");
        }

        [Fact]
        public void GetAllActiveUsers_NoViewRights_AuthorizationExceptionThrown()
        {
            // Arrange
            var testData = _testFixture.TestUsers().Build();
            MockAuthServiceThrowsException(AuthOperations.AllUsers.ViewActiveList);

            var sut = BuildSUT();

            // Act => Assert
            Action act = () => sut.GetAllActiveUsers();
            act.Should().Throw<AuthorizationException>("Requested operation is not allowed");
        }

        [Fact]
        public void GetById_UserExists_UserReturned()
        {
            // Arrange
            var expected = new UserBuilder().WithId(EXISTING_ID).Build();
            MockGetUserByIdQuery(expected);

            var sut = BuildSUT();

            // Act
            var actual = sut.GetUser(EXISTING_ID);

            // Assert
            TestHelper.AreEqual<User>(expected, actual, new UserComparer());
        }

        [Fact]
        public void GetUserDetails_NoViewRights_AuthorizationExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsException(AuthOperations.AllUsers.ViewDetails);

            var sut = BuildSUT();

            // Act => Assert
            Action act = () => sut.GetUserDetails(EXISTING_ID);
            act.Should().Throw<AuthorizationException>("Requested operation is not allowed");
        }

        [Fact]
        public void GetUserDetails_UserExists_UserReturned()
        {
            // Arrange
            var player = new PlayerBuilder(EXISTING_ID).Build();
            var expected = new UserBuilder().WithId(EXISTING_ID).WithPlayer(player).Build();
            MockGetUserByIdQuery(expected);
            MockGetPlayerByIdQuery(player);
            var sut = BuildSUT();

            // Act
            var actual = sut.GetUserDetails(EXISTING_ID);

            // Assert
            TestHelper.AreEqual<User>(expected, actual, new UserComparer());
        }

        [Fact]
        public void GetAdminsList_UsersExist_UsersReturned()
        {
            // Arrange
            var testData = _testFixture.TestUsers().Build();
            MockGetAdminsListQuery(testData);

            var expected = new UserServiceTestFixture()
                                            .TestUsers()
                                            .Build();
            var sut = BuildSUT();

            // Act
            var actual = sut.GetAdminsList();

            // Assert
            TestHelper.AreEqual(expected, actual, new UserComparer());
        }

        private UserService BuildSUT()
        {
            return new UserService(
                _authServiceMock.Object,
                _getByIdQueryMock.Object,
                _getAllQueryMock.Object,
                _getPlayerByIdQueryMock.Object,
                _cacheProviderMock.Object,
                _getAdminsListQueryMock.Object,
                _userRepositoryMock.Object,
                _currentUserServiceMock.Object);
        }

        private void MockAuthServiceThrowsException(AuthOperation operation)
        {
            _authServiceMock.Setup(tr => tr.CheckAccess(operation)).Throws<AuthorizationException>();
        }

        private void MockGetAllUsersQuery(IEnumerable<User> testData)
        {
            _getAllQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.ToList());
        }

        private void MockGetUserByIdQuery(User testData)
        {
            _getByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(testData);
        }

        private void MockGetPlayerByIdQuery(Player testData)
        {
            _getPlayerByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(testData);
        }

        private void MockGetAdminsListQuery(IEnumerable<User> testData)
        {
            _getAdminsListQueryMock.Setup(tr => tr.Execute(It.IsAny<UniqueUserCriteria>())).Returns(testData.ToList());
        }
    }
}
