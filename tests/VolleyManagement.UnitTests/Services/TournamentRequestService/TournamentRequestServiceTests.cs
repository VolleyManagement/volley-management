namespace VolleyManagement.UnitTests.Services.TournamentRequestService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Contracts.ExternalResources;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Data.Queries.TournamentRequest;
    using Domain.RolesAggregate;
    using Domain.TournamentRequestAggregate;
    using Domain.UsersAggregate;
    using FluentAssertions;
    using MailService;
    using Moq;
    using UserManager;
    using VolleyManagement.Services;
    using System.Collections;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class TournamentRequestServiceTests
    {
        private const int EXISTING_ID = 1;
        private const int INVALID_REQUEST_ID = -1;
        private readonly TournamentRequestServiceTestFixture _testFixture
            = new TournamentRequestServiceTestFixture();

        private Mock<ITournamentRequestRepository> _tournamentRequestRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IAuthorizationService> _authServiceMock;
        private Mock<IQuery<ICollection<TournamentRequest>, GetAllCriteria>> _getAllRequestsQueryMock;
        private Mock<IQuery<TournamentRequest, FindByIdCriteria>> _getRequestByIdQueryMock;
        private Mock<IQuery<TournamentRequest, FindByTeamTournamentCriteria>> _getRequestByAllQueryMock;
        private Mock<ITournamentService> _tournamentServiceMock = new Mock<ITournamentService>();
        private Mock<IMailService> _mailServiceMock;
        private Mock<IUserService> _userServiceMock;

        public TournamentRequestServiceTests()
        {
            _tournamentRequestRepositoryMock
              = new Mock<ITournamentRequestRepository>();

            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _authServiceMock = new Mock<IAuthorizationService>();

            _getAllRequestsQueryMock = new Mock<IQuery<ICollection<TournamentRequest>, GetAllCriteria>>();

            _getRequestByIdQueryMock = new Mock<IQuery<TournamentRequest, FindByIdCriteria>>();

            _getRequestByAllQueryMock = new Mock<IQuery<TournamentRequest, FindByTeamTournamentCriteria>>();

            _tournamentServiceMock = new Mock<ITournamentService>();

            _mailServiceMock = new Mock<IMailService>();

            _userServiceMock = new Mock<IUserService>();

            _tournamentRequestRepositoryMock.Setup(tr => tr.UnitOfWork)
                .Returns(_unitOfWorkMock.Object);
        }

        [Fact]
        public void GetAll_RequestsExist_RequestsReturned()
        {
            // Arrange
            var expected = _testFixture.TestRequests().Build();

            MockGetAllTournamentRequestQuery(expected);

            var sut = BuildSUT();

            // Act
            var actual = sut.Get();

            // Assert
            TestHelper.AreEqual(expected, actual, new TournamentRequestComparer());
        }

        [Fact]
        public void GetAll_NoViewListRights_AuthorizationExceptionThrows()
        {
            // Arrange
            MockAuthServiceThrownException(AuthOperations.TournamentRequests.ViewList);
            var sut = BuildSUT();

            // Act => Assert
            Action act = () => sut.Get();
            act.Should().Throw<AuthorizationException>("Requested operation is not allowed");
        }

        [Fact]
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

        [Fact]
        public void Create_InvalidUserId_ExceptionThrows()
        {
            var newTournamentRequest = new TournamentRequestBuilder()
               .Build();
            _tournamentRequestRepositoryMock.Setup(
                    tr => tr.Add(
                        newTournamentRequest))
                .Callback<TournamentRequest>(t => t.UserId = -1);

            // Arrange
            var sut = BuildSUT();

            // Act => Assert
            Action act = () => sut.Create(newTournamentRequest);
            act.Should().Throw<ArgumentException>("User's id is wrong");
        }

        [Fact]
        public void Create_ValidTournamentRequest_RequestAdded()
        {
            // Arrange
            var newTournamentRequest = new TournamentRequestBuilder()
               .WithId(EXISTING_ID)
               .WithTeamId(EXISTING_ID)
               .WithGroupId(EXISTING_ID)
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
            sut.Create(newTournamentRequest);

            // Assert
            VerifyCreateTournamentRequest(newTournamentRequest, Times.Once(), "Parameter request is not equal to Instance of request");
        }

        [Fact]
        public void Create_TournamentRequesExist_RequestNotAdded()
        {
            // Arrange
            var newTournamentRequest = new TournamentRequestBuilder()
               .WithId(EXISTING_ID)
               .WithTeamId(EXISTING_ID)
               .WithGroupId(EXISTING_ID)
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
            sut.Create(newTournamentRequest);

            // Assert
            VerifyCreateTournamentRequest(newTournamentRequest, Times.Never(), "Parameter request is not equal to Instance of request");
        }

        [Fact]
        public void Confirm_NoConfirmRights_AuthorizationExceptionThrows()
        {
            // Arrange
            MockAuthServiceThrownException(AuthOperations.TournamentRequests.Confirm);
            var sut = BuildSUT();

            // Act => Assert
            Action act = () => sut.Confirm(EXISTING_ID);
            act.Should().Throw<AuthorizationException>("Requested operation is not allowed");
        }

        [Fact]
        public void Confirm_RequestExists_TeamAdded()
        {
            // Arrange
            var expected = new TournamentRequestBuilder().WithId(EXISTING_ID).Build();
            MockGetRequestByIdQuery(expected);

            _tournamentServiceMock.Setup(tr => tr.AddTeamsToTournament(It.IsAny<List<TeamTournamentAssignmentDto>>()));

            var emailMessage = new EmailMessageBuilder().Build();
            MockGetUser();
            MockRemoveTournamentRequest();
            MockMailService(emailMessage);
            var sut = BuildSUT();

            // Act
            sut.Confirm(EXISTING_ID);

            // Assert
            VerifyAddedTeamToTournament(Times.Once());
        }

        [Fact]
        public void Confirm_RequestDoesNotExist_ExceptionThrown()
        {
            // Arrange
            var sut = BuildSUT();

            // Act => Assert
            Action act = () => sut.Confirm(INVALID_REQUEST_ID);
            act.Should().Throw<MissingEntityException>("A tournament request with specified identifier was not found");
        }

        [Fact]
        public void Decline_RequestExist_RequestDeleted()
        {
            // Arrange
            var expected = new TournamentRequestBuilder().Build();
            MockGetRequestByIdQuery(expected);
            var emailMessage = new EmailMessageBuilder().Build();
            MockMailService(emailMessage);
            MockGetUser();
            var sut = BuildSUT();

            // Act
            sut.Decline(EXISTING_ID, emailMessage.Body);

            // Assert
            VerifyDeleteRequest(EXISTING_ID, Times.Once());
        }

        [Fact]
        public void Decline_RequestDoesNotExist_DbNotChanged()
        {
            // Arrange
            MockRequestServiceThrowsInvalidKeyValueException();
            Exception exception = null;
            var emailMessage = new EmailMessageBuilder().Build();
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
                _tournamentServiceMock.Object,
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

        private void VerifyAddedTeamToTournament(Times times)
        {
            _tournamentServiceMock.Verify(tr => tr.AddTeamsToTournament(It.IsAny<List<TeamTournamentAssignmentDto>>()), times);
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
            var user = new UserBuilder().Build();
            var userList = new List<User> { user };
            _userServiceMock.Setup(tr => tr.GetAdminsList()).Returns(userList);
        }

        private void MockGetUser()
        {
            var user = new UserBuilder().Build();
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
