namespace VolleyManagement.UI.Areas.Mvc.ViewModels.FeedbackViewModel
{
    /// <summary>
    /// Represents the information which will use for the feedback completing dialog.
    /// </summary>
    public class FeedbackMessageViewModel
    {
        /// <summary>
        /// Gets or sets message to user
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the result of Feedback send.
        /// </summary>
        public bool OperationSuccessful { get; set; }
    }
}