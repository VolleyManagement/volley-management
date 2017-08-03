namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Division
{
    using System.ComponentModel.DataAnnotations;
    using Domain;
    using Domain.TournamentsAggregate;
    using Resources.UI;

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
            Name = string.Format(
                "{0} {1}",
                ViewModelResources.GroupDefaultName,
                Constants.Group.MIN_GROUPS_COUNT);

            IsEmpty = true;
        }

        /// <summary>
        /// Gets or sets the group's identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the group's name.
        /// </summary>
        [Display(Name = "GroupName", ResourceType = typeof(ViewModelResources))]
        [Required(
            ErrorMessageResourceName = "GroupNameRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [MaxLength(
            Constants.Group.MAX_NAME_LENGTH,
            ErrorMessageResourceName = "GroupNameMaxLengthErrorMessage",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Name { get; set; }

        public int DivisionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether group is empty.
        /// </summary>
        public bool IsEmpty { get; set; }

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
                DivisionId = group.DivisionId,
                IsEmpty = group.IsEmpty
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
                Id = Id,
                Name = Name,
                DivisionId = DivisionId
            };
        }

        #endregion
    }
}
