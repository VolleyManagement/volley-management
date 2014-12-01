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
        /// tournament id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// tournament name
        /// </summary>
        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        /// <summary>
        /// tournament scheme
        /// </summary>
        [Required]
        [StringLength(40)]
        public string Scheme { get; set; }

        /// <summary>
        /// tournament season
        /// </summary>
        [Required]
        [StringLength(40)]
        public string Season { get; set; }

        /// <summary>
        /// tournament description
        /// </summary>
        [Required]
        [StringLength(1024)]
        public string Description { get; set; }

        /// <summary>
        /// regulations of the tournament
        /// </summary>
        [StringLength(1024)]
        public string LinkToReglament { get; set; }
    }
}
