namespace VolleyManagement.Data.MsSql.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// DAL user model
    /// </summary>
    public class ContributorTeamEntity
    {
        /// <summary>
        /// Gets or sets id of Team
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of Team
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets first name of Team
        /// </summary>
        public string CourseDirection { get; set; }

        /// <summary>
        /// Gets or sets contributors in Team
        /// </summary>
        public virtual ICollection<ContributorEntity> Contributors { get; set; }
    }
}