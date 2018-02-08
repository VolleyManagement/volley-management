namespace VolleyManagement.UI.Areas.Admin.Models
{
    using Domain.FeedbackAggregate;

    /// <summary>
    /// The feedback view model.
    /// </summary>
    public class RequestsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestsViewModel"/> class.
        /// </summary>
        public RequestsViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestsViewModel"/> class.
        /// </summary>
        /// <param name="request">
        /// The feedback.
        /// </param>
        public RequestsViewModel(Feedback request)
        {
            Id = request.Id;
            UsersEmail = request.UsersEmail;
            Content = request.Content;
            Date = request.Date;
            Status = request.Status;
            AdminName = request.AdminName;
            UpdateDate = request.UpdateDate;
            CanClose = request.Status != FeedbackStatusEnum.Closed;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string UsersEmail { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public System.DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public FeedbackStatusEnum Status { get; set; }

        /// <summary>
        /// Gets or sets the reply date.
        /// </summary>
        public System.DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the admin name.
        /// </summary>
        public string AdminName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets if user can close.
        /// </summary>
        public bool CanClose { get; set; }
    }
}