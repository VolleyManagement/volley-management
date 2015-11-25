namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Division
{
    using System.Collections.Generic;
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
        /// Initializes a new instance of the <see cref="DivisionViewModel"/> class.
        /// </summary>
        public DivisionViewModel()
        {
            this.Groups = new List<GroupViewModel>() { new GroupViewModel() };
        }
        
        /// <summary>
        /// Gets or sets the division's identifier.
        /// </summary>
        public int Id { get; set; }

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

        /// <summary>
        /// List of groups
        /// </summary>
        [Display(Name = "Groups", ResourceType = typeof(ViewModelResources))]
        public List<GroupViewModel> Groups { get; set; }

        #region Factory methods

        /// <summary>
        /// Maps domain model of <see cref="Division"/> to view model of <see cref="Division"/>.
        /// Maps view model of <see cref="Division"/> to domain model of <see cref="Division"/>.
        /// </summary>
        /// <param name="division">Domain model of <see cref="Division"/>.</param>
        /// <returns>View model of <see cref="Division"/>.</returns>
        public static DivisionViewModel Map(Division division)
        {
            var divisionViewModel = new DivisionViewModel()
            {
                Id = division.Id,
                Name = division.Name
            };

            divisionViewModel.Groups = division.Groups.Select(g => GroupViewModel.Map(g)).ToList();
            return divisionViewModel;
        }

        /// <summary>
        /// Maps view model of <see cref="Division"/> to domain model of <see cref="Division"/>.
        /// </summary>
        /// <returns>Domain model of <see cref="Division"/>.</returns>
        public Division ToDomain()
        {
            var division = new Division()
            {
                Id = this.Id,
                Name = this.Name,
                TournamentId = this.TournamentId
            };

            division.Groups = this.Groups.Select(g => g.ToDomain()).ToList();
            return division;
        }

        #endregion
    }
}
