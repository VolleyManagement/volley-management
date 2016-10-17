namespace VolleyManagement.UI.Areas.Mvc.ViewModels.FeedbackViewModel
{
    /// <summary>
    /// Represents a view model for <see cref="FeedbackViewModel"/>.
    /// </summary>
    public class FeedbackViewModel
    {
        /// <summary>
        /// Gets or sets feedback Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets users Email.
        /// </summary>
        public string UsersEmail { get; set; }

        /// <summary>
        /// Gets or sets feedback content.
        /// </summary>
        public string Content { get; set; }
    }
}