using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using VolleyManagement.Contracts.Authorization;
using VolleyManagement.Data.Contracts;
using VolleyManagement.Data.Queries.Common;
using VolleyManagement.Data.Queries.Team;
using VolleyManagement.Domain.PlayersAggregate;
using VolleyManagement.Domain.UsersAggregate;
using VolleyManagement.Contracts;
using VolleyManagement.Contracts.Exceptions;
using VolleyManagement.Domain.RolesAggregate;
using VolleyManagement.Services.Authorization;
using VolleyManagement.UI.Infrastructure;
using VolleyManagement.UnitTests.Services.FeedbackService;

namespace VolleyManagement.UnitTests.Services.UsersService
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    class UserServiceTests
    {
        private const int EXISTING_ID = 1;

        private readonly Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();

        private readonly Mock<IQuery<List<User>, GetAllCriteria>> _getAllQueryMock =
          new Mock<IQuery<List<User>, GetAllCriteria>>();

        private readonly Mock<IQuery<Player, FindByIdCriteria>> _getPlayerByIdQueryMock =
            new Mock<IQuery<Player, FindByIdCriteria>>();

        private readonly Mock<IQuery<User, FindByIdCriteria>> _getByIdQueryMock =
            new Mock<IQuery<User, FindByIdCriteria>>();

        private readonly UserServiceTestFixture _testFixture = new UserServiceTestFixture();


        private IKernel _kernel;

        /// <summary>
        /// Initializes test data.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IQuery<List<User>, GetAllCriteria>>().ToConstant(_getAllQueryMock.Object);
            _kernel.Bind<IQuery<User, FindByIdCriteria>>().ToConstant(_getByIdQueryMock.Object);
            _kernel.Bind<IQuery<Player, FindByIdCriteria>>().ToConstant(_getPlayerByIdQueryMock.Object);
            _kernel.Bind<IAuthorizationService>().ToConstant(_authServiceMock.Object);
        }

        [TestMethod]
        public void GetAll_UsersExist_UsersReturned()
        {
            // Arrange
            var testData = _testFixture.TestUsers().Build();
            MockGetAllUsersQuery(testData);

            var expected = new UserServiceTestFixture()
                                            .TestUsers()
                                            .Build()
                                            .ToList();

            var sut = _kernel.Get<UserService>();

            // Act
            var actual = sut.GetAllUsers();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new UserComparer());
        }

        [TestMethod]
        public void GetAll_NoViewRights_AuthorizationExceptionThrow()
        {
            // Arrange
            Exception exception = null;
            var testData = _testFixture.TestUsers().Build();
            MockAuthServiceThrowsExeption(AuthOperations.AllUsers.ViewList);

            var sut = _kernel.Get<UserService>();

            // Act
            try
            {
                var actual = sut.GetAllUsers();
            }
            catch (AuthorizationException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "Requested operation is not allowed");
        }

        [TestMethod]
        public void GetById_UserExists_UserReturned()
        {
            // Arrange
            var expected = new UserBuilder().WithId(EXISTING_ID).Build();
            MockGetUserByIdQuery(expected);

            var sut = _kernel.Get<UserService>();

            // Act
            var actual = sut.GetUser(EXISTING_ID);

            // Assert
            TestHelper.AreEqual<User>(expected, actual, new UserComparer());
        }

        [TestMethod]
        public void GetUserDetails_NoViewRights_AuthorizationExceptionThrow()
        {
            // Arrange
            Exception exception = null;
            var testData = _testFixture.TestUsers().Build();
            MockAuthServiceThrowsExeption(AuthOperations.AllUsers.ViewList);

            var sut = _kernel.Get<UserService>();

            // Act
            try
            {
                var actual = sut.GetUserDetails(EXISTING_ID);
            }
            catch (AuthorizationException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "Requested operation is not allowed");
        }


        private void MockAuthServiceThrowsExeption(AuthOperation operation)
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

        private void VerifyExceptionThrown(Exception exception, string expectedMessage)
        {
            Assert.IsNotNull(exception, "There is no exception thrown");
            Assert.IsTrue(exception.Message.Equals(expectedMessage), "Expected and actual exceptions messages aren't equal");
        }


    }
}
