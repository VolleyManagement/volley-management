namespace VolleyManagement.UnitTests.Services.FeedbackService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Data.Queries.Player;
    using Data.Queries.Team;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject;
    using VolleyManagement.Domain.FeedbackAggregate;
    using VolleyManagement.Services;
    using Data.Contracts;

    [TestClass]
    public class FeedbackServiceTest
    {         
        private readonly Mock<IFeedbackRepository> _feedbackRepositoryMock = new Mock<IFeedbackRepository>();
        private IKernel _kernel;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [TestInitialize]
        public void TestInit()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IFeedbackRepository>().ToConstant(_feedbackRepositoryMock.Object);
            _feedbackRepositoryMock.Setup(fr => fr.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        [TestMethod]
        public void Create_FeedbackPassed_FeedbackCreated()
        {
            var newFeedback = new FeedbackBuilder().WithId(2).Build();

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

        private bool FeedbacksAreEqual(Feedback x, Feedback y)
        {
            return new FeedbackComparer().Compare(x, y) == 0;
        }

        private void VerifyCreateFeedback(Feedback feedback, Times times)
        {
            _feedbackRepositoryMock.Verify(pr => pr.Add(It.Is<Feedback>(f => FeedbacksAreEqual(f, feedback))), times);
            _unitOfWorkMock.Verify(uow => uow.Commit(), times);
        }
    }
}