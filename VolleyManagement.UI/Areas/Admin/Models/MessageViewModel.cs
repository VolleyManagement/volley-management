namespace VolleyManagement.UI.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;
    using VolleyManagement.UI.App_GlobalResources;

    /// <summary>
    /// Represents a view model for <see cref="MessageViewModel"/>.
    /// </summary>
    public class MessageViewModel
    {
        /// <summary>
        /// Gets or sets request Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets message.
        /// </summary>
        [Display(Name = "DeclineMessage", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
           ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(5000)]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}