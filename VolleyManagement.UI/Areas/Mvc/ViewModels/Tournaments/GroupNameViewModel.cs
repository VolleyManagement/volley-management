namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Division
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;

    /// <summary>
    /// Represents team view model just with name and id
    /// </summary>
    public class GroupNameViewModel
    {
        /// <summary>
        /// Gets or sets the team Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the team name
        /// </summary>
        [Display(Name = "Group Name")]
        public string Name { get; set; }

        /// <summary>
        /// Maps Team to TeamNameViewModel
        /// </summary>
        /// <param name="group">Domain team</param>
        /// <returns>View model object</returns>
        public static GroupNameViewModel Map(Group group)
        {
            return new GroupNameViewModel
            {
                Id = group.Id,
                Name = group.Name,
            };
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns>Domain object</returns>
        public Group ToDomain()
        {
            return new Group
            {
                Id = Id,
                Name = Name
            };
        }
    }
}