﻿namespace VolleyManagement.UI.Areas.Mvc.ViewModels.ContributorsTeam
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Domain;
    using Domain.ContributorsAggregate;
    using Resources;

    /// <summary>
    /// Represents contributor team view model
    /// </summary>
    public class ContributorsTeamViewModel
    {
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
        public IEnumerable<Contributor> Contributors { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="contributorTeam"> Domain object </param>
        /// <returns> View model object </returns>
        public static ContributorsTeamViewModel Map(ContributorTeam contributorTeam)
        {
            var contributorTeamViewModel = new ContributorsTeamViewModel {
                Id = contributorTeam.Id,
                Name = contributorTeam.Name,
                CourseDirection = contributorTeam.CourseDirection,
                Contributors = contributorTeam.Contributors
            };

            return contributorTeamViewModel;
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        public ContributorTeam ToDomain()
        {
            return new ContributorTeam {
                Id = Id,
                Name = Name,
                CourseDirection = CourseDirection,
                Contributors = Contributors
            };
        }
        #endregion
    }
}