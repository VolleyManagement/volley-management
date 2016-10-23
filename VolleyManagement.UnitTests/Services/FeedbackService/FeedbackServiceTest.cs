using VolleyManagement.Contracts.Exceptions;
using VolleyManagement.Data.Exceptions;
using VolleyManagement.Domain.UsersAggregate;

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
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.FeedbackAggregate;
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

        private const int ANONYM_ID = -1;

        private const int SPECIFIC_FEEDBACK_ID = 1;

        private const int NOT_VALID_ID = -1;

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
            _kernel.Bind<IUserService>().ToConstant(_userServiceMock.Object);
            _timeMock.Setup(tp => tp.UtcNow).Returns(_date);
        }

        /// <summary>
        /// Cleanup test data
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            TimeProvider.ResetToDefault();
        }

        /// <summary>
        /// Test for Get() method. The method should return existing feedbacks
        /// </summary>
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

            // Act
            var sut = _kernel.Get<FeedbackService>();
            var actual = sut.Get().ToList();

            // Assert
            CollectionAssert.AreEqual(expected, actual, new FeedbackComparer());
        }

        /// <summary>
        /// Test for Get() method. The method should return existing feedback
        /// </summary>
        [TestMethod]
        public void GetById_FeedbackExist_FeedbackReturned()
        {
            // Arrange
            var testData = new FeedbackBuilder().WithId(SPECIFIC_FEEDBACK_ID).Build();
            MockGetFeedbackByIdQuery(testData);
            var expected = new FeedbackBuilder().WithId(SPECIFIC_FEEDBACK_ID).Build();
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

        /// <summary>
        /// Test for Close() method with invalid id as input parameter. The method should throw MissingEntityException
        /// and shouldn't invoke Update and Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        public void Close_NotValidIdAsParam_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.Close(NOT_VALID_ID);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.FEEDBACK_NOT_FOUND); 
        }

        /// <summary>
        /// Test for Close() method with valid id as input parameter. The method should throw MissingEntityException
        /// and shouldn't invoke Update and Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        public void Close_ValidIdAsParam_UpdatedFeedbackInfo()
        {
            // Arrange
            var feedback = new FeedbackBuilder().WithId(SPECIFIC_FEEDBACK_ID).Build();
            MockGetFeedbackByIdQuery(feedback);

            this._userServiceMock.Setup(
                us => us.GetCurrentUserInstance()).Returns(
                new User { PersonName = TEST_NAME });

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.Close(SPECIFIC_FEEDBACK_ID);

            // Assert
            VerifyEditFeedback(feedback, Times.Once());
        }

        /// <summary>
        /// Test for Close() method with valid id as input parameter and non-authorized user. 
        /// The method should throw AuthorizationException
        /// and shouldn't invoke Update and Commit() method of IUnitOfWork.
        /// </summary>
        [TestMethod]
        public void Close_ValidIdAsParamUserNotAuthorized_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            var feedback = new FeedbackBuilder().WithId(SPECIFIC_FEEDBACK_ID).Build();
            MockGetFeedbackByIdQuery(feedback);

            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.Close(SPECIFIC_FEEDBACK_ID);
            }
            catch (AuthorizationException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, "Requested operation is not allowed");
        }

        /// <summary>
        /// Test for GetDetails() method with valid id as input parameter. The method should return feedback 
        /// and update it's status
        /// </summary>
        [TestMethod]
        public void GetDetails_ValidIdAsParam_UpdateFeedbackInfo()
        {
            // Arrange
            var feedback = new FeedbackBuilder().WithId(SPECIFIC_FEEDBACK_ID).Build();
            MockGetFeedbackByIdQuery(feedback);

            this._userServiceMock.Setup(
                us => us.GetCurrentUserInstance()).Returns(
                new User { PersonName = TEST_NAME });

            var sut = _kernel.Get<FeedbackService>();

            // Act
            sut.GetDetails(SPECIFIC_FEEDBACK_ID);

            // Assert
            VerifyEditFeedback(feedback, Times.Once());
        }

        /// <summary>
        /// Test for GetDetails() method with non-valid id as input parameter. The method should thrown exception
        /// </summary>
        [TestMethod]
        public void GetDetails_NotValidIdAsParam_ExceptionThrown()
        {
            // Arrange
            Exception exception = null;
            var sut = _kernel.Get<FeedbackService>();

            // Act
            try
            {
                sut.GetDetails(NOT_VALID_ID);
            }
            catch (MissingEntityException ex)
            {
                exception = ex;
            }

            // Assert
            VerifyExceptionThrown(exception, ExpectedExceptionMessages.FEEDBACK_NOT_FOUND);
        }

        private void SetupEditMissingEntityException(Feedback feedback)
        {
            _feedbackRepositoryMock.Setup(m =>
                m.Update(It.Is<Feedback>(grs => FeedbacksAreEqual(grs, feedback))))
                .Throws(new ConcurrencyException());
        }

        /// <summary>
        /// Checks if exception was thrown and has appropriate message
        /// </summary>
        /// <param name="exception">Exception that has been thrown</param>
        /// <param name="expectedMessage">Message to compare with</param>
        private void VerifyExceptionThrown(Exception exception, string expectedMessage)
        {
            Assert.IsNotNull(exception);
            Assert.IsTrue(exception.Message.Equals(expectedMessage));
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