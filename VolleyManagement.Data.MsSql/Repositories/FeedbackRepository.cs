namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.MsSql.Mappers;
    using VolleyManagement.Data.MsSql.Repositories.Specifications;
    using VolleyManagement.Domain.FeedbackAggregate;

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
            this._unitOfWork = (VolleyUnitOfWork)unitOfWork;
            this._dalFeedbacks = _unitOfWork.Context.Feedbacks;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return this._unitOfWork; }
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

            this._dalFeedbacks.Add(newFeedback);
            this._unitOfWork.Commit();
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
            var dalToRemove = new Entities.FeedbackEntity { Id = id };
            this._dalFeedbacks.Attach(dalToRemove);
            this._dalFeedbacks.Remove(dalToRemove);
        }
    }
}
