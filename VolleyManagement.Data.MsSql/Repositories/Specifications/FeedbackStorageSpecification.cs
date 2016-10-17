namespace VolleyManagement.Data.MsSql.Repositories.Specifications
{
    using VolleyManagement.Crosscutting.Contracts.Specifications;
    using VolleyManagement.Crosscutting.Specifications;
    using VolleyManagement.Data.MsSql.Entities;

    /// <summary>
    /// Provides Specification for DB storage
    /// </summary>
    public class FeedbackStorageSpecification : ISpecification<FeedbackEntity>
    {
        /// <summary>
        /// Verifies that entity matches specification
        /// </summary>
        /// <param name="entity"> The entity to test </param>
        /// <returns> Results of the match </returns>
        public bool IsSatisfiedBy(FeedbackEntity entity)
        {
            var usersEmail = new ExpressionSpecification<FeedbackEntity>(p =>
                                !string.IsNullOrEmpty(p.UsersEmail)
                                && p.UsersEmail.Length <= ValidationConstants.Feedback.MAX_EMAIL_LENGTH);
            var content = new ExpressionSpecification<FeedbackEntity>(p =>
                                !string.IsNullOrEmpty(p.Content)
                                && p.Content.Length <= ValidationConstants.Feedback.MAX_CONTENT_LENGTH);

            return usersEmail.And(content).IsSatisfiedBy(entity);
        }
    }
}