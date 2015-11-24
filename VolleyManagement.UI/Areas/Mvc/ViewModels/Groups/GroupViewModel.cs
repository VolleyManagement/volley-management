namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Groups
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using VolleyManagement.Domain;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.App_GlobalResources;

    /// <summary>
    /// Represents group's view model.
    /// </summary>
    public class GroupViewModel
    {
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

        /// <summary>
        /// Maps <see cref="Group"/> to <see cref="GroupViewModel"/>.
        /// </summary>
        /// <param name="group">Group to map</param>
        /// <returns>Group view model</returns>
        public static GroupViewModel Map(Group group)
        {
            return new GroupViewModel { Id = group.Id, Name = group.Name, DivisionId = group.DivisionId };
        }

        /// <summary>
        /// Maps  <see cref="GroupViewModel"/> to <see cref="Group"/>.
        /// </summary>
        /// <returns>Group domain model</returns>
        public Group ToDomain()
        {
            return new Group()
            {
                Id = this.Id,
                Name = this.Name,
                DivisionId = this.DivisionId
            };
        }
    }
}