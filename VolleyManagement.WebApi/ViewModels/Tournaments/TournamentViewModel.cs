namespace VolleyManagement.WebApi.ViewModels.Tournaments
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// TournamentViewModel class.
    /// </summary>
    public class TournamentViewModel
    {
        /// <summary>
        /// Gets or sets value indicating where Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Name.
        /// </summary>
        /// <value>Name of tournament.</value>
        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        [StringLength(300)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        [Required]
        [StringLength(9)]
        public string Season { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        [Required]
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        [StringLength(255)]
        public string RegulationsLink { get; set; }
    }
}