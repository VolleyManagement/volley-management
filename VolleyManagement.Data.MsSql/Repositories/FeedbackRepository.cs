namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Contracts;
    using Domain.FeedbackAggregate;
    using Entities;
    using Exceptions;
    using Mappers;
    using Specifications;

    /// <summary>
    /// Defines implementation of the IFeedbackRepository contract.
    /// </summary>
    internal class FeedbackRepository : IFeedbackRepository
    {
        private readonly FeedbackStorageSpecification _dbStorageSpecification
            = new FeedbackStorageSpecification();

        private readonly DbSet<FeedbackEntity> _dalFeedbacks;

        private readonly VolleyUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public FeedbackRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalFeedbacks = _unitOfWork.Context.Feedbacks;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Adds new feedback.
        /// </summary>
        /// <param name="newEntity">The feedback for adding.</param>
        public void Add(Feedback newEntity)
        {
            var newFeedback = new FeedbackEntity();
            DomainToDal.Map(newFeedback, newEntity);

            if (!_dbStorageSpecification.IsSatisfiedBy(newFeedback))
            {
                throw new InvalidEntityException();
            }

            _dalFeedbacks.Add(newFeedback);
            _unitOfWork.Commit();
            newEntity.Id = newFeedback.Id;
        }

        /// <summary>
        /// Updates specified feedback.
        /// </summary>
        /// <param name="updatedEntity">Updated feedback.</param>
        public void Update(Feedback updatedEntity)
        {
            var feedbackToUpdate = _dalFeedbacks.SingleOrDefault(t => t.Id == updatedEntity.Id);

            if (feedbackToUpdate == null)
            {
                throw new ConcurrencyException();
            }

            DomainToDal.Map(feedbackToUpdate, updatedEntity);
        }

        /// <summary>
        /// Removes Feedback by id.
        /// </summary>
        /// <param name="id">The id of Feedback to remove.</param>
        public void Remove(int id)
        {
            var dalToRemove = new FeedbackEntity { Id = id };
            _dalFeedbacks.Attach(dalToRemove);
            _dalFeedbacks.Remove(dalToRemove);
        }
    }
}
