namespace VolleyManagement.UI.Areas.WebApi.ViewModels.ContributorsTeam
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Domain;
    using Domain.ContributorsAggregate;
    using Resources;

    /// <summary>
    /// Represents contributors team team view model
    /// </summary>
    public class ContributorsTeamViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContributorsTeamViewModel" /> class.
        /// </summary>
        public ContributorsTeamViewModel()
        {
            Contributors = new List<string>();
        }

        /// <summary>
        /// Gets or sets id of ContributorTeam
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name of ContributorTeam
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets course direction of ContributorTeam
        /// </summary>
        public string CourseDirection { get; set; }

        /// <summary>
        /// Gets or sets contributors in ContributorTeam
        /// </summary>
        public ICollection<string> Contributors { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="contributorTeam"> Domain object </param>
        /// <returns> View model object </returns>
        public static ContributorsTeamViewModel Map(ContributorTeam contributorTeam)
        {
            var contributorTeamViewModel = new ContributorsTeamViewModel
            {
                Id = contributorTeam.Id,
                Name = contributorTeam.Name,
                CourseDirection = contributorTeam.CourseDirection
            };

            foreach (var c in contributorTeam.Contributors)
            {
                contributorTeamViewModel.Contributors.Add(c.Name);
            }

            return contributorTeamViewModel;
        }
        #endregion
    }
}