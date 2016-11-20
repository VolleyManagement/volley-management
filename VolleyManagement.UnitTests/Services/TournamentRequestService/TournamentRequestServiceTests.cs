namespace VolleyManagement.UnitTests.Services.TournamentRequestService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Queries.Common;
    using Domain.RolesAggregate;
    using Domain.TournamentRequestAggregate;
    using Domain.TournamentsAggregate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Services;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TournamentRequestServiceTests
    {
        private const int EXISTING_ID = 1;
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
            _tournamentRequestRepositoryMock.Setup(tr => tr.UnitOfWork)
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
        public void Create_ValidTournamentRequest_RequestAdded()
        {
            // Arrange
            var newTournamentRequest = new TournamentRequestBuilder()
               .WithId(EXISTING_ID)
               .WithTeamId(EXISTING_ID)
               .WithTournamentId(EXISTING_ID)
               .WithUserId(EXISTING_ID)
               .Build();

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

        private void MockGetAllTournamentRequestQuery(IEnumerable<TournamentRequest> testData)
        {
            _getAllRequestsQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.ToList());
        }

        private void VerifyExceptionThrown(Exception exception, string expectedMessage)
        {
            Assert.IsNotNull(exception, "There is no exception thrown");
            Assert.IsTrue(exception.Message.Equals(expectedMessage), "Expected and actual messages aren't equal");
        }

        private void MockAuthServiceThrownException(AuthOperation operation)
        {
            _authServiceMock.Setup(tr => tr.CheckAccess(operation)).Throws<AuthorizationException>();
        }

        private void MockGetRequestByIdQuery(TournamentRequest request)
        {
            _getRequestByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(request);
        }
    }
}
