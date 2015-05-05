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
    [Table("Tournaments")]
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
        /// Gets or sets the tournament season as a byte offset from the 1900
        /// </summary>
        [Required]
        public byte Season { get; set; }

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

        /// <summary>
        /// Gets or sets start of a tournament
        /// </summary>
        [Required]
        public DateTime GamesStart { get; set; }

        /// <summary>
        /// Gets or sets end of a tournament
        /// </summary>
        [Required]
        public DateTime GamesEnd { get; set; }

        /// <summary>
        /// Gets or sets start of a transfer period
        /// </summary>
        [Required]
        public DateTime TransferStart { get; set; }

        /// <summary>
        /// Gets or sets end of a transfer period
        /// </summary>
        [Required]
        public DateTime TransferEnd { get; set; }

        /// <summary>
        /// Gets or sets start of a tournament
        /// </summary>
        [Required]
        public DateTime ApplyingPeriodStart { get; set; }

        /// <summary>
        /// Gets or sets end of a tournament registration
        /// </summary>
        [Required]
        public DateTime ApplyingPeriodEnd { get; set; }
    }
}