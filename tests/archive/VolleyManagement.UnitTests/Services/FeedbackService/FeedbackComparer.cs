namespace VolleyManagement.UnitTests.Services.FeedbackService
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.FeedbackAggregate;

    /// <summary>
    /// Comparer for feedback objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class FeedbackComparer : IComparer<Feedback>, IComparer, IEqualityComparer<Feedback>
    {
        /// <summary>
        /// Compare two feedback objects.
        /// </summary>
        /// <param name="x">First feedback to compare.</param>
        /// <param name="y">Second feedback to compare.</param>
        /// <returns>A signed integer that indicates
        /// the relative values of feedbacks.</returns>
        public int Compare(Feedback x, Feedback y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compare two feedback objects (non-generic implementation).
        /// </summary>
        /// <param name="x">First feedback to compare.</param>
        /// <param name="y">Second feedback to compare.</param>
        /// <returns>A signed integer that indicates
        /// the relative values of feedbacks.</returns>
        public int Compare(object x, object y)
        {
            var firstFeedback = x as Feedback;
            var secondFeedback = y as Feedback;

            if (firstFeedback == null)
            {
                return -1;
            }

            if (secondFeedback == null)
            {
                return 1;
            }

            return Compare(firstFeedback, secondFeedback);
        }

        public bool Equals(Feedback x, Feedback y)
        {
            return AreEqual(x, y);
        }

        public int GetHashCode(Feedback obj)
        {
            return obj.Id.GetHashCode();
        }

        /// <summary>
        /// Finds out whether two feedback objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given feedbacks have the same properties.</returns>
        private bool AreEqual(Feedback x, Feedback y)
        {
            return x.Id == y.Id
                && x.UsersEmail == y.UsersEmail
                && x.Content == y.Content
                && x.Date == y.Date
                && x.Status == y.Status
                && x.UserEnvironment == y.UserEnvironment;
        }
    }
}
