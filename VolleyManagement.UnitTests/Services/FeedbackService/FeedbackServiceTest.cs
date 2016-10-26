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
    using Ninject;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.FeedbackAggregate;
    using VolleyManagement.Domain.MailsAggregate;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Domain.UsersAggregate;
    using VolleyManagement.Services;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FeedbackServiceTest
    {
        #region Fields and constants

        public const string ERROR_FOR_FEEDBACK_REPOSITORY_VERIFY
            = "Parameter feedback is not equal to Instance of feedback";

        public const string ERROR_FOR_UNIT_OF_WORK_VERIFY
            = "Can't save feedback to database";

        private const string FEEDBACK_NOT_FOUND = "A feedback with specified identifier was not found";

        private const int SPECIFIC_FEEDBACK_ID = 1;

        private const int INVALID_ID = -1;

        private const string TEST_NAME = "Nick";

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

        private readonly Mock<IMailService> _mailServiceMock = new Mock<IMailService>();

        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();

        private readonly Mock<IAuthorizationService> _authServiceMock = new Mock<IAuthorizationService>();

        private readonly FeedbackServiceTestFixture _testFixture = new FeedbackServiceTestFixture();

        private IKernel _kernel;
        private DateTime _date = new DateTime(2007, 05, 03);

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
            _kernel.Bind<IMailService>().ToConstant(_mailServiceMock.Object);
            _kernel.Bind<IAuthorizationService>().ToConstant(_authServiceMock.Object);
            _kernel.Bind<IUserService>().ToConstant(_userServiceMock.Object);
            _timeMock.Setup(tp => tp.UtcNow).Returns(_date);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TimeProvider.ResetToDefault();
        }

        [TestMethod]
        public void GetAll_FeedbackExist_FeedbacksReturned()
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
            var actual = sut.Get().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new FeedbackComparer());
        }

        [TestMethod]
        public void GetById_FeedbackExist_FeedbackReturned()
        {
            // Arrange
            var expected = new FeedbackBuilder().WithId(SPECIFIC_FEEDBACK_ID).Build();
            MockGetFeedbackByIdQuery(expected);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            var actual = sut.Get(SPECIFIC_FEEDBACK_ID);

            // Assert
            TestHelper.AreEqual<Feedback>(expected, actual, new FeedbackComparer());
        }

        [TestMethod]
        public void Create_CorrectFeedback_FeedbackCreated()
        {
            var newFeedback = new FeedbackBuilder().Build();

            var sut = _kernel.Get<FeedbackService>();
            sut.Create(newFeedback);

            VerifyCreateFeedback(newFeedback, Times.Once());
        }

        [TestMethod]
        public void Create_InvalidNullFeedback_ArgumentNullExceptionIsThrown()
        {
            bool gotException = false;

            // Arrange
            Feedback newFeedback = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.Create(newFeedback);
            }
            catch (ArgumentNullException)
            {
                gotException = true;
            }

            // Assert
            Assert.IsTrue(gotException);
            VerifyCreateFeedback(newFeedback, Times.Never());
        }

        [TestMethod]
        public void Update_UpdateFeedbackDate_FeedbackUpdated()
        {
            var feed = new FeedbackBuilder().Build();
            Assert.AreEqual(TimeProvider.Current.UtcNow, feed.Date);
        }

        [TestMethod]
        public void Close_InvalidIdFeedbackNotExist_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.Close(INVALID_ID);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, FEEDBACK_NOT_FOUND);
        }

        [TestMethod]
        public void Close_FeedbackWithValidIdAsParam_UpdatedFeedbackInfo()
        {
            // Arrange
            var feedback = new FeedbackBuilder().WithId(SPECIFIC_FEEDBACK_ID).Build();
            MockGetFeedbackByIdQuery(feedback);

            SetupCurrentUserInfo();

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Close(SPECIFIC_FEEDBACK_ID);

            // Assert
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Close_NoCloseRights_ExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsExeption(AuthOperations.Feedbacks.Close);

            var sut = _kernel.Get<FeedbackService>();

            // Act
             sut.Close(SPECIFIC_FEEDBACK_ID);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never());
            VerifyCheckAccess(AuthOperations.Feedbacks.Close, Times.Once());
        }

        [TestMethod]
        public void GetDetails_FeedbackWithValidIdAsParam_UpdateFeedbackInfo()
        {
            // Arrange
            var feedback = new FeedbackBuilder().WithId(SPECIFIC_FEEDBACK_ID).Build();
            MockGetFeedbackByIdQuery(feedback);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.GetDetails(SPECIFIC_FEEDBACK_ID);

            // Assert
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        public void GetDetails_InvalidIdFeedbackNotExist_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.GetDetails(INVALID_ID);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, FEEDBACK_NOT_FOUND);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void GetDetails_NoReadRights_ExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsExeption(AuthOperations.Feedbacks.Read);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.GetDetails(SPECIFIC_FEEDBACK_ID);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never());
            VerifyCheckAccess(AuthOperations.Feedbacks.Read, Times.Once());
        }

        [TestMethod]
        public void Reply_InvalidIdFeedbackNotExist_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();
            var answerToSend = new Mail();
            SetupMail(answerToSend);

            // Act
            try
            {
                sut.Reply(INVALID_ID, answerToSend);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, FEEDBACK_NOT_FOUND);
        }

        [TestMethod]
        public void Reply_FeedbackWithValidIdAsParam_UpdatedFeedback()
        {
            // Arrange
            var feedback = new FeedbackBuilder().WithId(SPECIFIC_FEEDBACK_ID).Build();
            MockGetFeedbackByIdQuery(feedback);
            var answerToSend = new Mail();
            SetupMail(answerToSend);
            SetupCurrentUserInfo();

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Reply(SPECIFIC_FEEDBACK_ID, answerToSend);

            // Assert
            VerifyEditFeedback(feedback, Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void Reply_NoReplyRights_ExceptionThrown()
        {
            // Arrange
            MockAuthServiceThrowsExeption(AuthOperations.Feedbacks.Answer);
            var answerToSend = new Mail();
            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Reply(SPECIFIC_FEEDBACK_ID, answerToSend);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Never());
            VerifyCheckAccess(AuthOperations.Feedbacks.Answer, Times.Once());
        }

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

        private void SetupCurrentUserInfo()
        {
            this._userServiceMock.Setup(
             us => us.GetCurrentUserInstance()).Returns(
             new User { PersonName = TEST_NAME });
        }

        private void SetupMail(Mail newMail)
        {
            this._mailServiceMock.Setup(
                us => us.Send(newMail));
        }

        private void VerifyExceptionThrown(Exception exception, string expectedMessage)
        {
            Assert.IsNotNull(exception, "Exception is null");
            Assert.IsTrue(exception.Message.Equals(expectedMessage), "Expected and actual exceptions aren't equal");
        }

        private bool FeedbacksAreEqual(Feedback x, Feedback y)
        {
            return new FeedbackComparer().Compare(x, y) == 0;
        }

        private void VerifyCreateFeedback(Feedback feedback, Times times)
        {
            _feedbackRepositoryMock.Verify(
                pr => pr.Add(
                It.Is<Feedback>(f =>
                FeedbacksAreEqual(f, feedback))),
                times,
                ERROR_FOR_FEEDBACK_REPOSITORY_VERIFY);
            _unitOfWorkMock.Verify(
                uow => uow.Commit(),
                times,
                ERROR_FOR_UNIT_OF_WORK_VERIFY);
        }

        private void VerifyEditFeedback(Feedback feedback, Times times)
        {
            _feedbackRepositoryMock.Verify(tr => tr.Update(It.Is<Feedback>(t => FeedbacksAreEqual(t, feedback))), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }

        private void MockAuthServiceThrowsExeption(AuthOperation operation)
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
    }
}