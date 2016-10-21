namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Mvc.ViewModels.FeedbackViewModel;

    [ExcludeFromCodeCoverage]
    internal class FeedbackViewModelComparer : IComparer<FeedbackViewModel>, IComparer
    {
        /// <summary>
        /// Compares two feedback object.
        /// </summary>
        /// <param name="x">First object to compare.</param>
        /// <param name="y">Second object to compare.</param>
        /// <returns>A signed integer that indicates
        /// the relative values of feedback.</returns>
        public int Compare(FeedbackViewModel x, FeedbackViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two feedback object (non-generic implementation).
        /// </summary>
        /// <param name="x">First object to compare.</param>
        /// <param name="y">Second object to compare.</param>
        /// <returns>A signed integer that indicates the relative
        /// values of feedback.</returns>
        public int Compare(object x, object y)
        {
            FeedbackViewModel firstFeedback = x as FeedbackViewModel;
            FeedbackViewModel secondFeedback = y as FeedbackViewModel;

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

        /// <summary>
        /// Finds out whether two feedback objects have the same properties.
        /// </summary>
        /// <param name="x">First object to compare.</param>
        /// <param name="y">Second object to compare.</param>
        /// <returns>True if given feedbacks have the same properties.</returns>
        private bool AreEqual(FeedbackViewModel x, FeedbackViewModel y)
        {
            return x.Id == y.Id
                && x.UsersEmail == y.UsersEmail
                && x.Content == y.Content;
        }
    }
}
