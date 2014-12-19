namespace VolleyManagement.Dal.MsSql
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// DAL tournament model
    /// </summary>
    [Table("Tournament")]
    public partial class Tournament
    {
        /// <summary>
        /// Gets or sets the tournament id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the tournament name
        /// </summary>
        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tournament scheme
        /// </summary>
        public byte Scheme { get; set; }

        /// <summary>
        /// Gets or sets the tournament season
        /// </summary>
        [Required]
        [StringLength(9)]
        public string Season { get; set; }

        /// <summary>
        /// Gets or sets the tournament description
        /// </summary>
        [StringLength(300)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets regulations of the tournament
        /// </summary>
        [StringLength(255)]
        public string RegulationsLink { get; set; }
    }
}
