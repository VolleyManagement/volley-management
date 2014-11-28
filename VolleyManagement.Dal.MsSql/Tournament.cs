namespace SoftServe.VolleyManagement.Dal.MsSql
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tournament")]
    public partial class Tournament
    {
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        [Required]
        [StringLength(40)]
        public string Scheme { get; set; }

        [Required]
        [StringLength(40)]
        public string Season { get; set; }

        [Required]
        [StringLength(1024)]
        public string Description { get; set; }

        [StringLength(1024)]
        public string LinkToReglament { get; set; }
    }
}
