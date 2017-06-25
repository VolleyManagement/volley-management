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
    using Data.Queries.TournamentRequest;
    using Domain.RolesAggregate;
    using Domain.TournamentRequestAggregate;
    using Domain.TournamentsAggregate;
    using Domain.UsersAggregate;
    using MailService;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MSTestExtensions;
    using UserManager;
    using VolleyManagement.Contracts;
    using VolleyManagement.Services;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentRequestServiceTests : BaseTest
    {
        private const int EXISTING_ID = 1;
        private const int INVALID_REQUEST_ID = -1;
        private readonly TournamentRequestServiceTestFixture _testFixture
            = new TournamentRequestServiceTestFixture();

        private Mock<ITournamentRequestRepository> _tournamentRequestRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<IQuery<List<TournamentRequest>, GetAllCriteria>> _getAllRequestsQueryMock;
        private Mock<IQuery<TournamentRequest, FindByIdCriteria>> _getRequestByIdQueryMock;
        private Mock<IQuery<TournamentRequest, FindByTeamTournamentCriteria>> _getRequestByAllQueryMock;
        private Mock<ITournamentRepository> _tournamentRepositoryMock;
        private Mock<IMailService> _mailServiceMock;
        private Mock<IUserService> _userServiceMock;

        [TestInitialize]
        public void TestInit()
        {
            _tournamentRequestRepositoryMock
              = new Mock<ITournamentRequestRepository>();

            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _authServiceMock = new Mock<IAuthorizationService>();

            _getAllRequestsQueryMock = new Mock<IQuery<List<TournamentRequest>, GetAllCriteria>>();

            _getRequestByIdQueryMock = new Mock<IQuery<TournamentRequest, FindByIdCriteria>>();

            _getRequestByAllQueryMock = new Mock<IQuery<TournamentRequest, FindByTeamTournamentCriteria>>();

            _tournamentRepositoryMock = new Mock<ITournamentRepository>();

            _mailServiceMock = new Mock<IMailService>();

            _userServiceMock = new Mock<IUserService>();


            _tournamentRepositoryMock.Setup(tr => tr.UnitOfWork)
                    .Returns(_unitOfWorkMock.Object);
            _tournamentRequestRepositoryMock.Setup(tr => tr.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
        }

        [TestMethod]
        public void GetAll_RequestsExist_RequestsReturned()
        {
            // Arrange
            var expected = _testFixture.TestRequests().Build();

            MockGetAllTournamentRequestQuery(expected);

            var sut = BuildSUT();
            // Act
            var actual = sut.Get();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new TournamentRequestComparer());
        }

        [TestMethod]
        public void GetAll_NoViewListRights_AuthorizationExceptionThrows()
        {
            // Arrange
            MockAuthServiceThrownException(AuthOperations.TournamentRequests.ViewList);
            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<AuthorizationException>(
                () =>
                sut.Get(),
                "Requested operation is not allowed");
        }

        [TestMethod]
        public void GetById_RequestExists_RequestReturned()
        {
            // Arrange
            var expected = new TournamentRequestBuilder().WithId(EXISTING_ID).Build();
            MockGetRequestByIdQuery(expected);

            var sut = BuildSUT();

            // Act
            var actual = sut.Get(EXISTING_ID);

            // Assert
            TestHelper.AreEqual<TournamentRequest>(expected, actual, new TournamentRequestComparer());
        }

        [TestMethod]
        public void Create_InvalidUserId_ExceptionThrows()
        {
            // Arrange
            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<ArgumentException>(
                () =>
                 sut.Create(INVALID_REQUEST_ID, EXISTING_ID, EXISTING_ID),
                "User's id is wrong");
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
            var sut = BuildSUT();

            // Act
            sut.Create(EXISTING_ID, EXISTING_ID, EXISTING_ID);

            // Assert
            VerifyCreateTournamentRequest(newTournamentRequest, Times.Once(), "Parameter request is not equal to Instance of request");
        }

        [TestMethod]
        public void Create_TournamentRequesExist_RequestNotAdded()
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
            MockGetAllTournamentRequestQuery(newTournamentRequest);
            var sut = BuildSUT();

            // Act
            sut.Create(EXISTING_ID, EXISTING_ID, EXISTING_ID);

            // Assert
            VerifyCreateTournamentRequest(newTournamentRequest, Times.Never(), "Parameter request is not equal to Instance of request");
        }

        [TestMethod]
        public void Confirm_NoConfirmRights_AuthorizationExceptionThrows()
        {
            // Arrange
            MockAuthServiceThrownException(AuthOperations.TournamentRequests.Confirm);
            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<AuthorizationException>(
                () =>
                sut.Confirm(EXISTING_ID),
                "Requested operation is not allowed");
        }

        [TestMethod]
        public void Confirm_RequestExists_TeamAdded()
        {
            // Arrange
            var expected = new TournamentRequestBuilder().WithId(EXISTING_ID).Build();
            MockGetRequestByIdQuery(expected);
            _tournamentRepositoryMock.Setup(tr => tr.AddTeamToTournament(It.IsAny<int>(), It.IsAny<int>()));
            var emailMessage = new EmailMessageBuilder().Build();
            MockGetUser();
            MockRemoveTournamentRequest();
            MockMailService(emailMessage);
            var sut = BuildSUT();

            // Act
            sut.Confirm(EXISTING_ID);

            // Assert
            VerifyAddedTeam(expected.Id, expected.TournamentId, Times.Once(), Times.AtLeastOnce());
        }

        [TestMethod]
        public void Confirm_RequestDoesNotExist_ExceptionThrown()
        {
            // Arrange
            var sut = BuildSUT();

            // Act => Assert
            Assert.Throws<MissingEntityException>(
                () =>
                sut.Confirm(INVALID_REQUEST_ID),
                "A tournament request with specified identifier was not found");
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
            var sut = BuildSUT();

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
            EmailMessage emailMessage = new EmailMessageBuilder().Build();
            MockMailService(emailMessage);
            var expected = new TournamentRequestBuilder().Build();
            MockGetRequestByIdQuery(expected);
            MockGetUser();

            var sut = BuildSUT();

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

        private TournamentRequestService BuildSUT()
        {
            return new TournamentRequestService(
                _tournamentRequestRepositoryMock.Object,
                _authServiceMock.Object,
                _getAllRequestsQueryMock.Object,
                _getRequestByIdQueryMock.Object,
                _getRequestByAllQueryMock.Object,
                _tournamentRepositoryMock.Object,
                _mailServiceMock.Object,
                _userServiceMock.Object);
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

        private void MockGetAllTournamentRequestQuery(TournamentRequest testData)
        {
            _getRequestByAllQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByTeamTournamentCriteria>())).Returns(testData);
        }

        private void VerifyAddedTeam(int requestId, int tournamentId, Times times, Times unitOfWorkTimes)
        {
            _tournamentRepositoryMock.Verify(tr => tr.AddTeamToTournament(requestId, tournamentId), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), unitOfWorkTimes);
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

        private void MockRemoveTournamentRequest()
        {
            _tournamentRequestRepositoryMock.Setup(tr => tr.Remove(It.IsAny<int>()));
        }
    }
}
