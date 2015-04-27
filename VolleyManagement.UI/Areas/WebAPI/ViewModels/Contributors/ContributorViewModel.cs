
namespace VolleyManagement.UI.Areas.WebApi.ViewModels.Contributors
{
    using System.ComponentModel.DataAnnotations;
    using VolleyManagement.Domain;
    using VolleyManagement.Domain.ContributorsAggregate;
    using VolleyManagement.UI.App_GlobalResources;

    /// <summary>
    /// Represents contributor view model
    /// </summary>
    public class ContributorViewModel
    {
        /// <summary>
        /// Gets or sets the contributor Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the contributor first name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets contributor Team Id of player
        /// </summary>
        public int? ContributorTeamId { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="contributor"> Domain object </param>
        /// <returns> View model object </returns>
        public static ContributorViewModel Map(Contributor contributor)
        {
            var contributorViewModel = new ContributorViewModel
            {
                Id = contributor.Id,
                Name = contributor.Name,
                ContributorTeamId = contributor.ContributorTeamId
            };

            return contributorViewModel;
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        public Contributor ToDomain()
        {
            return new Contributor
            {
                Id = this.Id,
                Name = this.Name,
                ContributorTeamId = this.ContributorTeamId
            };
        }
        #endregion

    }
}