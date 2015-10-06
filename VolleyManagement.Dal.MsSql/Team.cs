namespace VolleyManagement.Dal.MsSql
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// DAL team model
    /// </summary>
    [Table("Teams")]
    public partial class Team
    {
        /// <summary>
        /// Gets or sets id of team
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name of team
        /// </summary>
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets coach of team
        /// </summary>
        [StringLength(60)]
        public string Coach { get; set; }

        /// <summary>
        /// Gets or sets achievements of team
        /// </summary>
        [StringLength(4000)]
        public string Achievements { get; set; }

        /// <summary>
        /// Gets or sets captain id of team
        /// </summary>
        public int CaptainId { get; set; }
    }
}