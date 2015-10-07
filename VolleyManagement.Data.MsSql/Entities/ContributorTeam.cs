namespace VolleyManagement.Data.MsSql.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// DAL user model
    /// </summary>
    [Table("ContributorTeam")]
    public partial class ContributorTeam
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
        public virtual ICollection<Contributor> Contributors { get; set; }
    }
}