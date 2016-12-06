namespace VolleyManagement.UnitTests.Services.FeedbackService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Crosscutting.Contracts.Providers;
    using Data.Contracts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MSTestExtensions;
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Crosscutting.Contracts.MailService;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.FeedbackAggregate;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.Services;
    using VolleyManagement.UnitTests.Services.MailService;
    using VolleyManagement.UnitTests.Services.UserManager;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FeedbackServiceTest : BaseTest
    {
        #region Fields and constants

        public const string ERROR_FOR_FEEDBACK_REPOSITORY_VERIFY
            = "Parameter feedback is not equal to Instance of feedback";

        public const string ERROR_FOR_UNIT_OF_WORK_VERIFY
            = "Can't save feedback to database";

        public const int SPECIFIC_FEEDBACK_ID = 2;

        private const int EXISTING_ID = 1;

        private const int INVALID_FEEDBACK_ID = -1;

        private const int VALID_USER_ID = 1;

        private const string TEST_NAME = "Nick";

        private const string MESSAGE = "Test reply message";

        private readonly Mock<IFeedbackRepository> _feedbackRepositoryMock
            = new Mock<IFeedbackRepository>();

        private readonly Mock<IFeedbackService> _feedbackServiceMock = new Mock<IFeedbackService>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock
            = new Mock<IUnitOfWork>();

        private readonly Mock<TimeProvider> _timeMock = new Mock<TimeProvider>();

        private readonly Mock<IQuery<List<Feedback>, GetAllCriteria>> _getAllFeedbacksQueryMock =
            new Mock<IQuery<List<Feedback>, GetAllCriteria>>();

        private readonly Mock<IQuery<Feedback, FindByIdCriteria>> _getFeedbackByIdQueryMock =
         new Mock<IQuery<Feedback, FindByIdCriteria>>();

        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();

        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();

        private readonly Mock<IMailService> _mailServiceMock = new Mock<IMailService>();

        private readonly Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();

        private readonly FeedbackServiceTestFixture _testFixture = new FeedbackServiceTestFixture();

        private IKernel _kernel;
        private DateTime _feedbackTestDate = new DateTime(2007, 05, 03);

        #endregion

        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IFeedbackRepository>().ToConstant(_feedbackRepositoryMock.Object);
            _kernel.Bind<IFeedbackService>().ToConstant(_feedbackServiceMock.Object);
            _feedbackRepositoryMock.Setup(fr => fr.UnitOfWork).Returns(_unitOfWorkMock.Object);
            TimeProvider.Current = _timeMock.Object;
            _kernel.Bind<IQuery<Feedback, FindByIdCriteria>>().ToConstant(_getFeedbackByIdQueryMock.Object);
            _kernel.Bind<IQuery<List<Feedback>, GetAllCriteria>>().ToConstant(_getAllFeedbacksQueryMock.Object);
            _kernel.Bind<ICurrentUserService>().ToConstant(_currentUserServiceMock.Object);
            _kernel.Bind<IAuthorizationService>().ToConstant(_authServiceMock.Object);
            _kernel.Bind<IUserService>().ToConstant(_userServiceMock.Object);
            _kernel.Bind<IMailService>().ToConstant(_mailServiceMock.Object);
            _timeMock.Setup(tp => tp.UtcNow).Returns(_feedbackTestDate);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TimeProvider.ResetToDefault();
        }

        #region GetAll

        [TestMethod]
        public void GetAll_FeedbacksExist_FeedbacksReturned()
        {
            // Arrange
            var testData = _testFixture.TestFeedbacks().Build();
            MockGetAllFeedbacksQuery(testData);

            var expected = new FeedbackServiceTestFixture()
                                            .TestFeedbacks()
                                            .Build()
                                            .ToList();

            var sut = _kernel.Get<FeedbackService>();

            // Act
            var actual = sut.Get();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new FeedbackComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void GetAll_NoReadRights_AuthorizationExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsException(AuthOperations.Feedbacks.Read);
            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Get();
        }

        #endregion

        #region GetById

        [TestMethod]
        public void GetById_FeedbackExists_FeedbackReturned()
        {
            // Arrange
            var expected = new FeedbackBuilder().WithId(EXISTING_ID).Build();
            MockGetFeedbackByIdQuery(expected);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            var actual = sut.Get(EXISTING_ID);

            // Assert
            TestHelper.AreEqual<Feedback>(expected, actual, new FeedbackComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Get_NoReadRights_AuthorizationExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsException(AuthOperations.Feedbacks.Read);
            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Get(EXISTING_ID);
        }

        #endregion

        #region Create

        [TestMethod]
        public void Create_FeedbackPassed_FeedbackCreated()
        {
            // Arrange
            var newFeedback = new FeedbackBuilder()
                .Build();
            var sut = _kernel.Get<FeedbackService>();

            var emailMessage = new EmailMessageBuilder().Build();
            MockMailService(emailMessage);
            MockUserService();

            // Act
            sut.Create(newFeedback);

            // Assert
            VerifyCreateFeedback(
                newFeedback,
                Times.Once(),
                "Parameter feedback is not equal to Instance of feedback");
            VerifySaveFeedback(
                newFeedback,
                Times.Once(),
                "DB should not be updated");
        }

        [TestMethod]
        public void Create_InvalidNullFeedback_ArgumentNullExceptionIsThrown()
        {
            // Arrange
            Exception exception = null;
            Feedback newFeedback = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.Create(newFeedback);
            }
            catch (ArgumentNullException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(
                exception,
                new ArgumentNullException("feedback"));
        }

        [TestMethod]
        public void Update_FeedbackPassed_FeedbackUpdated()
        {
            // Arrange
            var actual = new FeedbackBuilder()
                .Build();
            var expected = new FeedbackBuilder()
                .WithDate(_feedbackTestDate)
                .Build();
            var sut = _kernel.Get<FeedbackService>();
            MockUserService();

            // Act
            sut.Create(actual);

            // Assert
            AssertFeedbackAreEquals(expected, actual);
        }

        #endregion

        #region Close

        [TestMethod]
        public void Close_FeedbackDoesNotExist_ExceptionThrown()
        {
            // Arrange
            var sut = _kernel.Get<FeedbackService>();

            // Act => Assert
            Assert.Throws<MissingEntityException>(
                () =>
                 sut.Close(INVALID_FEEDBACK_ID),
                "A feedback with specified identifier was not found");
        }

        [TestMethod]
        public void Close_NewFeedback_UpdatedFeedbackStatusToClose()
        {
            // Arrange
            var feedback = new FeedbackBuilder().WithId(EXISTING_ID).Build();
            MockGetFeedbackByIdQuery(feedback);

            MockCurrentUser(VALID_USER_ID);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Close(EXISTING_ID);

            // Assert
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        public void Close_ReadFeedback_UpdatedFeedbackStatusToClose()
        {
            // Arrange
            var feedback = new FeedbackBuilder()
                       .WithId(EXISTING_ID)
                       .WithStatus(FeedbackStatusEnum.Read)
                       .Build();
            MockGetFeedbackByIdQuery(feedback);

            MockCurrentUser(VALID_USER_ID);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Close(EXISTING_ID);

            // Assert
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        public void Close_AnsweredFeedback_UpdatedFeedbackStatusToClose()
        {
            // Arrange
            var feedback = new FeedbackBuilder()
                       .WithId(EXISTING_ID)
                       .WithStatus(FeedbackStatusEnum.Answered)
                       .Build();
            MockGetFeedbackByIdQuery(feedback);

            MockCurrentUser(VALID_USER_ID);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Close(EXISTING_ID);

            // Assert
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        public void Close_ClosedFeedback_InvalidOperationExceptionThrown()
        {
            // Arrange
            var feedback = new FeedbackBuilder()
                        .WithId(EXISTING_ID)
                        .WithStatus(FeedbackStatusEnum.Closed)
                        .Build();
            MockGetFeedbackByIdQuery(feedback);
            MockCurrentUser(VALID_USER_ID);

            var sut = _kernel.Get<FeedbackService>();

            // Act => Assert
            Assert.Throws<InvalidOperationException>(
                () =>
                 sut.Close(EXISTING_ID),
                "Feedback status can't be changed to this status");
        }

        [TestMethod]
        public void Close_AnsweredFeedback_LastUpdateInfoSet()
        {
            // Arrange
            var feedback = new FeedbackBuilder()
                         .WithId(EXISTING_ID)
                         .WithStatus(FeedbackStatusEnum.Answered)
                         .Build();
            MockGetFeedbackByIdQuery(feedback);

            MockCurrentUser(VALID_USER_ID);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Close(EXISTING_ID);

            // Assert
            VerifyAdminName(feedback, TEST_NAME);
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        public void Close_NewFeedback_LastUpdateInfoSet()
        {
            // Arrange
            var feedback = new FeedbackBuilder()
                        .WithId(EXISTING_ID)
                        .Build();
            MockGetFeedbackByIdQuery(feedback);

            MockCurrentUser(VALID_USER_ID);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Close(EXISTING_ID);

            // Assert
            VerifyAdminName(feedback, TEST_NAME);
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        public void Close_NoCloseRights_AuthorizationExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsException(AuthOperations.Feedbacks.Close);
            var sut = _kernel.Get<FeedbackService>();

            // Act => Assert
            Assert.Throws<AuthorizationException>(
                () =>
                 sut.Close(EXISTING_ID),
                "Requested operation is not allowed");
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Close_NoCloseRights_DbNotChanged()
        {
            // Arrange
            var feedback = new FeedbackBuilder().WithId(EXISTING_ID).Build();
            MockAuthServiceThrowsException(AuthOperations.Feedbacks.Close);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Close(EXISTING_ID);

            // Assert
            VerifyEditFeedback(feedback, Times.Never());
            VerifyCheckAccess(AuthOperations.Feedbacks.Close, Times.Once());
        }
        #endregion

        #region GetDetails

        [TestMethod]
        public void GetDetails_NewFeedback_UpdatedFeedbackStatusToRead()
        {
            // Arrange
            var feedback = new FeedbackBuilder().WithId(EXISTING_ID).Build();
            MockGetFeedbackByIdQuery(feedback);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.GetDetails(EXISTING_ID);

            // Assert
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        public void GetDetails_FeedbackDoesNotExist_ExceptionThrown()
        {
            // Arrange
            var sut = _kernel.Get<FeedbackService>();

            // Act => Assert
            Assert.Throws<MissingEntityException>(
                () =>
                 sut.GetDetails(INVALID_FEEDBACK_ID),
                "A feedback with specified identifier was not found");
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void GetDetails_NoReadRights_AuthorizationExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsException(AuthOperations.Feedbacks.Read);
            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.GetDetails(EXISTING_ID);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void GetDetails_NoReadRights_DbNotChanged()
        {
            // Arrange
            var feedback = new FeedbackBuilder().WithId(EXISTING_ID).Build();
            MockAuthServiceThrowsException(AuthOperations.Feedbacks.Read);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.GetDetails(EXISTING_ID);

            // Assert
            VerifyEditFeedback(feedback, Times.Never());
            VerifyCheckAccess(AuthOperations.Feedbacks.Read, Times.Once());
        }

        #endregion

        #region Reply
        [TestMethod]
        public void Reply_FeedbackDoesNotExist_ExceptionThrown()
        {
            // Arrange
            var sut = _kernel.Get<FeedbackService>();

            // Act => Assert
            Assert.Throws<MissingEntityException>(
                () =>
                 sut.Reply(EXISTING_ID, MESSAGE),
                "A feedback with specified identifier was not found");
        }

        [TestMethod]
        public void Reply_NewFeedback_UpdatedFeedbackStatusToAnswered()
        {
            // Arrange
            var feedback = new FeedbackBuilder().WithId(EXISTING_ID).Build();
            MockGetFeedbackByIdQuery(feedback);
            var emailMessage = new EmailMessageBuilder().Build();
            MockMailService(emailMessage);
            MockCurrentUser(VALID_USER_ID);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Reply(EXISTING_ID, MESSAGE);

            // Assert
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        public void Reply_ReadFeedback_UpdatedFeedbackStatusToAnswered()
        {
            // Arrange
            var feedback = new FeedbackBuilder()
                         .WithId(EXISTING_ID)
                         .WithStatus(FeedbackStatusEnum.Read)
                         .Build();
            MockGetFeedbackByIdQuery(feedback);
            var emailMessage = new EmailMessageBuilder().Build();
            MockMailService(emailMessage);
            MockCurrentUser(VALID_USER_ID);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Reply(EXISTING_ID, MESSAGE);

            // Assert
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        public void Reply_ClosedFeedback_InvalidOperationExceptionThrown()
        {
            // Arrange
            var feedback = new FeedbackBuilder()
                         .WithId(EXISTING_ID)
                         .WithStatus(FeedbackStatusEnum.Closed)
                         .Build();
            MockGetFeedbackByIdQuery(feedback);

            MockCurrentUser(VALID_USER_ID);

            var sut = _kernel.Get<FeedbackService>();

            // Act => Assert
            Assert.Throws<InvalidOperationException>(
                () =>
                 sut.Reply(EXISTING_ID, MESSAGE),
                "Feedback status can't be changed to this status");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reply_ClosedFeedback_DbNotChanged()
        {
            // Arrange
            var feedback = new FeedbackBuilder()
                         .WithId(EXISTING_ID)
                         .WithStatus(FeedbackStatusEnum.Closed)
                         .Build();
            MockGetFeedbackByIdQuery(feedback);

            MockCurrentUser(VALID_USER_ID);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Reply(EXISTING_ID, MESSAGE);

            // Assert
            VerifyEditFeedback(feedback, Times.Never());
        }

        [TestMethod]
        public void Reply_NewFeedback_LastUpdateInfoSet()
        {
            // Arrange
            var feedback = new FeedbackBuilder().WithId(EXISTING_ID).Build();
            MockGetFeedbackByIdQuery(feedback);
            var emailMessage = new EmailMessageBuilder().Build();
            MockMailService(emailMessage);
            MockCurrentUser(VALID_USER_ID);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Reply(EXISTING_ID, MESSAGE);

            // Assert
            VerifyAdminName(feedback, TEST_NAME);
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Reply_NoReplyRights_AuthorizationExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsException(AuthOperations.Feedbacks.Reply);
            var sut = _kernel.Get<FeedbackService>();

             // Act
            sut.Reply(EXISTING_ID, MESSAGE);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Reply_NoReplyRights_DbNotChanged()
        {
            // Arrange
            MockAuthServiceThrowsException(AuthOperations.Feedbacks.Reply);
            var feedback = new FeedbackBuilder().WithId(EXISTING_ID).Build();
            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Reply(EXISTING_ID, MESSAGE);

            // Assert
            VerifyEditFeedback(feedback, Times.Never());
            VerifyCheckAccess(AuthOperations.Feedbacks.Reply, Times.Once());
        }

        #endregion

        #region Private methods

        private void VerifyCheckAccess(AuthOperation operation, Times times)
        {
            _authServiceMock.Verify(tr => tr.CheckAccess(operation), times);
        }

        private void SetupEditMissingEntityException(Feedback feedback)
        {
            _feedbackRepositoryMock.Setup(m =>
                m.Update(It.Is<Feedback>(grs => FeedbacksAreEqual(grs, feedback))))
                .Throws(new ConcurrencyException());
        }

        private void MockCurrentUser(int userId)
        {
            SetupCurrentUserServiceGetCurrentUserId(VALID_USER_ID);

            this._userServiceMock.Setup(
             us => us.GetUser(userId)).Returns(
             new User { PersonName = TEST_NAME });
        }

        private bool FeedbacksAreEqual(Feedback x, Feedback y)
        {
            return new FeedbackComparer().Compare(x, y) == 0;
        }

        private void AssertFeedbackAreEquals(Feedback expected, Feedback actual)
        {
            TestHelper.AreEqual<Feedback>(expected, actual, new FeedbackComparer());
        }

        private void VerifyCreateFeedback(Feedback feedback, Times times, string message)
        {
            _feedbackRepositoryMock.Verify(
                pr => pr.Add(
                It.Is<Feedback>(f =>
                FeedbacksAreEqual(f, feedback))),
                times,
                message);
        }

        private void VerifySaveFeedback(Feedback feedback, Times times, string message)
        {
            _unitOfWorkMock.Verify(
                uow => uow.Commit(),
                times,
                message);
        }

        private void VerifyExceptionThrown(Exception exception, Exception expected)
        {
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(exception);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(exception.Message.Equals(expected.Message));
        }

        private void VerifyEditFeedback(Feedback feedback, Times times)
        {
            _feedbackRepositoryMock.Verify(tr => tr.Update(It.Is<Feedback>(t => FeedbacksAreEqual(t, feedback))), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void VerifyAdminName(Feedback feedback, string expectedAdminName)
        {
            Assert.IsNotNull(feedback.AdminName, "Admin's name didn't set");
            Assert.IsTrue(feedback.AdminName.Equals(expectedAdminName), "Expected and actual admin names aren't equal");
        }

        private void MockAuthServiceThrowsException(AuthOperation operation)
        {
            _authServiceMock.Setup(tr => tr.CheckAccess(operation)).Throws<AuthorizationException>();
        }

        private void MockGetAllFeedbacksQuery(IEnumerable<Feedback> testData)
        {
            _getAllFeedbacksQueryMock.Setup(tr => tr.Execute(It.IsAny<GetAllCriteria>())).Returns(testData.ToList());
        }

        private void MockGetFeedbackByIdQuery(Feedback testData)
        {
            _getFeedbackByIdQueryMock.Setup(tr => tr.Execute(It.IsAny<FindByIdCriteria>())).Returns(testData);
        }

        private void SetupCurrentUserServiceGetCurrentUserId(int id)
        {
            this._currentUserServiceMock.Setup(cs => cs.GetCurrentUserId())
                .Returns(id);
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
        #endregion
    }
}