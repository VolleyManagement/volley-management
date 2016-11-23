namespace VolleyManagement.UnitTests.Services.TournamentRequestService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Crosscutting.Contracts.MailService;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Domain.RolesAggregate;
    using Domain.TournamentRequestAggregate;
    using Domain.TournamentsAggregate;
    using Domain.UsersAggregate;
    using MailService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using UserManager;
    using VolleyManagement.Services;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentRequestServiceTests
    {
        private const int EXISTING_ID = 1;
        private const int INVALID_REQUEST_ID = -1;
        private readonly TournamentRequestServiceTestFixture _testFixture
            = new TournamentRequestServiceTestFixture();

        private readonly Mock<ITournamentRequestRepository> _tournamentRequestRepositoryMock
            = new Mock<ITournamentRequestRepository>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock
            = new Mock<IUnitOfWork>();

        private readonly Mock<IAuthorizationService> _authServiceMock
            = new Mock<IAuthorizationService>();

        private readonly Mock<IQuery<List<TournamentRequest>, GetAllCriteria>> _getAllRequestsQueryMock
            = new Mock<IQuery<List<TournamentRequest>, GetAllCriteria>>();

        private readonly Mock<IQuery<TournamentRequest, FindByIdCriteria>> _getRequestByIdQueryMock
            = new Mock<IQuery<TournamentRequest, FindByIdCriteria>>();

        private readonly Mock<ITournamentRepository> _tournamentRepositoryMock
            = new Mock<ITournamentRepository>();

        private readonly Mock<IMailService> _mailServiceMock
            = new Mock<IMailService>();

        private readonly Mock<IUserService> _userServiceMock
            = new Mock<IUserService>();

        private IKernel _kernel;
        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ITournamentRequestRepository>()
                .ToConstant(_tournamentRequestRepositoryMock.Object);
            _kernel.Bind<IUnitOfWork>()
                .ToConstant(_unitOfWorkMock.Object);
            _kernel.Bind<IAuthorizationService>()
                .ToConstant(_authServiceMock.Object);
            _kernel.Bind<IQuery<List<TournamentRequest>, GetAllCriteria>>()
                .ToConstant(_getAllRequestsQueryMock.Object);
            _kernel.Bind<IQuery<TournamentRequest, FindByIdCriteria>>()
                .ToConstant(_getRequestByIdQueryMock.Object);
            _kernel.Bind<ITournamentRepository>()
                .ToConstant(_tournamentRepositoryMock.Object);
            _kernel.Bind<IMailService>()
                .ToConstant(_mailServiceMock.Object);
            _kernel.Bind<IUserService>()
                .ToConstant(_userServiceMock.Object);
            _tournamentRequestRepositoryMock.Setup(tr => tr.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
            _tournamentRepositoryMock.Setup(tr => tr.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
        }

        [TestMethod]
        public void GetAll_RequestsExist_RequestsReturned()
        {
            // Arrange
            var expected = _testFixture.TestRequests().Build();
            var sut = _kernel.Get<TournamentRequestService>();
            MockGetAllTournamentRequestQuery(expected);

            // Act
            var actual = sut.Get();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TournamentRequestComparer());
        }

        [TestMethod]
        public void GetAll_NoViewListRights_AuthorizationExceptionThrows()
        {
            // Arrange
            Exception exception = null;
            MockAuthServiceThrownException(AuthOperations.TournamentRequests.ViewList);
            var sut = _kernel.Get<TournamentRequestService>();

            // Act
            try
            {
                sut.Get();
            }
            catch (AuthorizationException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "Requested operation is not allowed");
        }

        [TestMethod]
        public void GetById_RequestExists_RequestReturned()
        {
            // Arrange
            var expected = new TournamentRequestBuilder().WithId(EXISTING_ID).Build();
            MockGetRequestByIdQuery(expected);

            var sut = _kernel.Get<TournamentRequestService>();

            // Act
            var actual = sut.Get(EXISTING_ID);

            // Assert
            TestHelper.AreEqual<TournamentRequest>(expected, actual, new TournamentRequestComparer());
        }

        [TestMethod]
        public void Create_InvalidUserId_ExceptionThrows()
        {
            // Arrange
            Exception exception = null;
            var sut = _kernel.Get<TournamentRequestService>();

            // Act
            try
            {
                sut.Create(INVALID_REQUEST_ID, EXISTING_ID, EXISTING_ID);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "User's id is wrong");
        }

        [TestMethod]
        public void Create_ValidTournamentRequest_RequestAdded()
        {
            // Arrange
            var newTournamentRequest = new TournamentRequestBuilder()
               .WithId(EXISTING_ID)
               .WithTeamId(EXISTING_ID)
               .WithTournamentId(EXISTING_ID)
               .WithUserId(EXISTING_ID)
               .Build();
            var emailMessage = new EmailMessageBuilder().Build();
            MockMailService(emailMessage);
            MockUserService();
            _tournamentRequestRepositoryMock.Setup(
                tr => tr.Add(
                    It.IsAny<TournamentRequest>()))
                    .Callback<TournamentRequest>(t => t.Id = EXISTING_ID);
            var sut = _kernel.Get<TournamentRequestService>();

            // Act
            sut.Create(EXISTING_ID, EXISTING_ID, EXISTING_ID);

            // Assert
            VerifyCreateTournamentRequest(newTournamentRequest, Times.Once(), "Parameter request is not equal to Instance of request");
        }

        [TestMethod]
        public void Confirm_NoConfirmRights_AuthorizationExceptionThrows()
        {
            Exception exception = null;
            MockAuthServiceThrownException(AuthOperations.TournamentRequests.Confirm);
            var sut = _kernel.Get<TournamentRequestService>();
            try
            {
                sut.Confirm(EXISTING_ID);
            }
            catch (AuthorizationException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyCheckAccess(AuthOperations.TournamentRequests.Confirm, Times.Once());
        }

        [TestMethod]
        public void Confirm_RequestExists_TeamAdded()
        {
            // Assert
            var expected = new TournamentRequestBuilder().WithId(EXISTING_ID).Build();
            MockGetRequestByIdQuery(expected);
            _tournamentRepositoryMock.Setup(tr => tr.AddTeamToTournament(It.IsAny<int>(), It.IsAny<int>()));
            var emailMessage = new EmailMessageBuilder().Build();
            MockGetUser();
            MockMailService(emailMessage);
            var sut = _kernel.Get<TournamentRequestService>();

            // Act
            sut.Confirm(EXISTING_ID);

            // Arrange
            VerifyAddedTeam(expected.Id, expected.TournamentId, Times.Once());
        }

        [TestMethod]
        public void Confirm_RequestDoesNotExist_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            var sut = _kernel.Get<TournamentRequestService>();

            // Act
            try
            {
                sut.Confirm(INVALID_REQUEST_ID);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "A tournament request with specified identifier was not found");
        }

        [TestMethod]
        public void Decline_RequestExist_RequestDeleted()
        {
            // Arrange
            var expected = new TournamentRequestBuilder().Build();
            MockGetRequestByIdQuery(expected);
            EmailMessage emailMessage = new EmailMessageBuilder().Build();
            MockMailService(emailMessage);
            MockGetUser();
            var sut = _kernel.Get<TournamentRequestService>();

            // Act
            sut.Decline(EXISTING_ID, emailMessage.Body);

            // Assert
            VerifyDeleteRequest(EXISTING_ID, Times.Once());
        }

        [TestMethod]
        public void Decline_RequestDoesNotExist_DbNotChanged()
        {
            // Arrange
            MockRequestServiceThrowsInvalidKeyValueException();
            Exception exception = null;
            var sut = _kernel.Get<TournamentRequestService>();
            EmailMessage emailMessage = new EmailMessageBuilder().Build();
            MockMailService(emailMessage);
            var expected = new TournamentRequestBuilder().Build();
            MockGetRequestByIdQuery(expected);
            MockGetUser();
            // Act
            try
            {
                sut.Decline(INVALID_REQUEST_ID, emailMessage.Body);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyDeleteRequest(INVALID_REQUEST_ID, Times.Once(), Times.Never());
        }

        private void VerifyCreateTournamentRequest(
            TournamentRequest request,
            Times times,
            string message)
        {
            _tournamentRequestRepositoryMock
                .Verify(
                tr => tr.Add(
                    It.Is<TournamentRequest>(p => TournamentRequestAreEquals(p, request))),
                    times,
                    message);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private bool TournamentRequestAreEquals(TournamentRequest x, TournamentRequest y)
        {
            return new TournamentRequestComparer().Compare(x, y) == 0;
        }

        private void MockRequestServiceThrowsInvalidKeyValueException()
        {
            _tournamentRequestRepositoryMock.Setup(tr => tr.Remove(It.IsAny<int>())).Throws<InvalidKeyValueException>();
        }

        private void MockGetAllTournamentRequestQuery(IEnumerable<TournamentRequest> testData)
        {
            _getAllRequestsQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.ToList());
        }

        private void VerifyExceptionThrown(Exception exception, string expectedMessage)
        {
            Assert.IsNotNull(exception, "There is no exception thrown");
            Assert.IsTrue(exception.Message.Equals(expectedMessage), "Expected and actual messages aren't equal");
        }

        private void VerifyAddedTeam(int requestId, int tournamentId, Times times)
        {
            _tournamentRepositoryMock.Verify(tr => tr.AddTeamToTournament(requestId, tournamentId), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void MockAuthServiceThrownException(AuthOperation operation)
        {
            _authServiceMock.Setup(tr => tr.CheckAccess(operation)).Throws<AuthorizationException>();
        }

        private void MockGetRequestByIdQuery(TournamentRequest request)
        {
            _getRequestByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(request);
        }

        private void MockMailService(EmailMessage message)
        {
            _mailServiceMock.Setup(tr => tr.Send(message));
        }

        private void MockUserService()
        {
            User user = new UserBuilder().Build();
            List<User> userList = new List<User> { user };
            _userServiceMock.Setup(tr => tr.GetAdminsList()).Returns(userList);
        }

        private void MockGetUser()
        {
            User user = new UserBuilder().Build();
            _userServiceMock.Setup(tr => tr.GetUser(It.IsAny<int>())).Returns(user);
        }

        private void VerifyCheckAccess(AuthOperation operation, Times times)
        {
            _authServiceMock.Verify(tr => tr.CheckAccess(operation), times);
        }

        private void VerifyDeleteRequest(int requestid, Times repositoryTimes)
        {
            _tournamentRequestRepositoryMock.Verify(tr => tr.Remove(It.Is<int>(id => id == requestid)), repositoryTimes);
            _unitOfWorkMock.Verify(uow => uow.Commit(), repositoryTimes);
        }

        private void VerifyDeleteRequest(int requestid, Times repositoryTimes, Times unitOfWorkTimes)
        {
            _tournamentRequestRepositoryMock.Verify(tr => tr.Remove(It.Is<int>(id => id == requestid)), repositoryTimes);
            _unitOfWorkMock.Verify(uow => uow.Commit(), unitOfWorkTimes);
        }
    }
}
