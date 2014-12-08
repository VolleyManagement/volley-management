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
        public TournamentViewModel()
        {
            this.Tournament = new Tournament();
            this.Tournament.Name = Name;
            this.Tournament.Description = Description;
            this.Tournament.Season = Season;
            this.Tournament.Scheme = Scheme;
            this.Tournament.RegulationsLink = RegulationsLink;
        }
        public Tournament Tournament { get; set; }
        
        /// <summary>
        /// Enumeration for tournament scheme
        /// </summary>
        public enum TournamentSchemeEnum
        {
            /// <summary>
            /// Scheme 1
            /// </summary>
            [Description("1")]
            One = 1,

            /// <summary>
            /// Scheme 2
            /// </summary>
            [Description("2")]
            Two,

            /// <summary>
            /// Scheme 2.5
            /// </summary>
            [Description("2.5")]
            TwoAndHalf
        }

        /// <summary>
        /// Gets or sets a value indicating where Name.
        /// </summary>
        /// <value>Name of tournament.</value>
        [Required]
        [StringLength(80)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        [StringLength(1024)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        [Required]
        [StringLength(40)]
        public string Season { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        [Required]
        public Tournament.TournamentSchemeEnum Scheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        [StringLength(1024)]
        public string RegulationsLink { get; set; }
    }
}