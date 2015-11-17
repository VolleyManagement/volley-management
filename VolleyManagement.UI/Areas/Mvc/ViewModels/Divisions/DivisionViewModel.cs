namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Division
{
    using System.ComponentModel.DataAnnotations;

    using VolleyManagement.Domain;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.UI.App_GlobalResources;

    /// <summary>
    /// Represents division's view model.
    /// </summary>
    public class DivisionViewModel
    {
        /// <summary>
        /// Gets or sets the division's identifier.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the division's name.
        /// </summary>
        [Display(Name = "DivisionName", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "DivisionNameRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [MaxLength(Constants.Division.MAX_NAME_LENGTH, ErrorMessageResourceName = "DivisionNameMaxLengthErrorMessage",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the tournament that contains the division.
        /// </summary>
        public int TournamentId { get; set; }

        #region Factory methods

        /// <summary>
        /// Maps domain model of <see cref="Division"/> to view model of <see cref="Division"/>.
        /// </summary>
        /// <param name="division">Domain model of <see cref="Division"/>.</param>
        /// <returns>View model of <see cref="Division"/>.</returns>
        public static DivisionViewModel Map(Division division)
        {
            return new DivisionViewModel()
            {
                Id = division.Id,
                Name = division.Name
            };
        }

        /// <summary>
        /// Maps view model of <see cref="Division"/> to domain model of <see cref="Division"/>.
        /// </summary>
        /// <returns>Domain model of <see cref="Division"/>.</returns>
        public Division ToDomain()
        {
            return new Division()
            {
                Id = this.Id.HasValue ? this.Id.Value : 0,
                Name = this.Name,
                TournamentId = this.TournamentId
            };
        }

        #endregion
    }
}
