namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Division
{
    using System.ComponentModel.DataAnnotations;
    using VolleyManagement.Domain;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.App_GlobalResources;

    /// <summary>
    /// Represents division's view model.
    /// </summary>
    public class GroupViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupViewModel"/> class.
        /// </summary>
        public GroupViewModel()
        {
            this.Name = string.Format(
                "{0} {1}",
                ViewModelResources.GroupDefaultName,
                Constants.Group.MIN_GROUPS_COUNT);
        }

        /// <summary>
        /// Gets or sets the group's identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the group's name.
        /// </summary>
        [Display(Name = "GroupName", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "GroupNameRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [MaxLength(Constants.Group.MAX_NAME_LENGTH, ErrorMessageResourceName = "GroupNameMaxLengthErrorMessage",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the division that contains the group.
        /// </summary>
        public int DivisionId { get; set; }

        #region Factory methods

        /// <summary>
        /// Maps domain model of <see cref="Group"/> to view model of <see cref="GroupViewModel"/>.
        /// </summary>
        /// <param name="group">Domain model of <see cref="Group"/>.</param>
        /// <returns>View model of <see cref="GroupViewModel"/>.</returns>
        public static GroupViewModel Map(Group group)
        {
            return new GroupViewModel()
            {
                Id = group.Id,
                Name = group.Name,
                DivisionId = group.DivisionId
            };
        }

        /// <summary>
        /// Maps view model of <see cref="GroupViewModel"/> to domain model of <see cref="Group"/>.
        /// </summary>
        /// <returns>Domain model of <see cref="Group"/>.</returns>
        public Group ToDomain()
        {
            return new Group()
            {
                Id = this.Id,
                Name = this.Name,
                DivisionId = this.DivisionId
            };
        }

        #endregion
    }
}
