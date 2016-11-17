namespace VolleyManagement.UI.Areas.Mvc.ViewModels.FeedbackViewModel
{
    using System.ComponentModel.DataAnnotations;
    using App_GlobalResources;
    using Domain.FeedbackAggregate;

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
        [Display(Name = "UserEmail", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(80)]
        public string UsersEmail { get; set; }

        /// <summary>
        /// Gets or sets feedback content.
        /// </summary>
        [Display(Name = "FeedbackContent", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(5000)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        /// <summary>
        /// Environment (browser  and operating system) of user's computer.
        /// </summary>
        [Display(Name = "User's computer Environment")]
        [StringLength(320)]
        [DataType(DataType.MultilineText)]
        public string UserEnvironment { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps feedback entity to domain.
        /// </summary>
        /// <returns>Domain object.</returns>
        public Feedback ToDomain()
        {
            return new Feedback
            {
                Id = this.Id,
                UsersEmail = this.UsersEmail,
                Content = this.Content,
                UserEnvironment = this.UserEnvironment ?? string.Empty
            };
        }

        #endregion
    }
}