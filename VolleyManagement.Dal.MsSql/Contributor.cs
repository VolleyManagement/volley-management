namespace VolleyManagement.Dal.MsSql
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// DAL user model
    /// </summary>
    [Table("Contributors")]
    public partial class Contributor
    {
        /// <summary>
        /// Gets or sets id of contributor
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of contributor
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of contributor
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets Contributor Team Id of contributor
        /// </summary>
        public int? ContributorTeamId { get; set; }
    }
}