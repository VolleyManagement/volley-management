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
    [Table("Players")]
    public partial class Player
    {
        /// <summary>
        /// Gets or sets id of player
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of player
        /// </summary>
        [Required]
        [StringLength(60)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of player
        /// </summary>
        [Required]
        [StringLength(60)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets birth year of player
        /// </summary>
        public int? BirthYear { get; set; }

        /// <summary>
        /// Gets or sets height of player
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets weight of player
        /// </summary>
        public int? Weight { get; set; }
    }
}