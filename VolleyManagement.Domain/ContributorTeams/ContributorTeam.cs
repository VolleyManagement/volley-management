namespace VolleyManagement.Domain.ContributorTeams
{
    using System;
    using System.Collections.Generic;
    using VolleyManagement.Domain.Contributors;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Contributor domain class.
    /// </summary>
    public class ContributorTeam
    {
        /// <summary>
        /// Gets or sets id of ContributorTeam
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of ContributorTeam
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets first name of ContributorTeam
        /// </summary>
        public string CourseDirection { get; set; }

        /// <summary>
        /// Gets or sets contributors in ContributorTeam
        /// </summary>
        public IList<Contributor> Contributors { get; set; }
    }
}
