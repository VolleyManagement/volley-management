namespace VolleyManagement.Domain.Tournaments
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Tournament domain class.
    /// </summary>
    public class Tournament
    {
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
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of tournament.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Name.
        /// </summary>
        /// <value>Name of tournament.</value>
        [Required(ErrorMessageResourceName = "TournamentNameRequired",
            ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TournamentName", ResourceType = typeof(Resources))]
        [StringLength(80)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        [Display(Name = "TournamentDescription", ResourceType = typeof(Resources))]
        [StringLength(1024)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        [Required(ErrorMessageResourceName = "TournamentSeasonRequired",
            ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TournamentSeason", ResourceType = typeof(Resources))]
        [StringLength(40)]
        public string Season { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        [Required(ErrorMessageResourceName = "TournamentSchemeRequired",
            ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "TournamentScheme", ResourceType = typeof(Resources))]
        public TournamentSchemeEnum Scheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        [Display(Name = "TournamentRegulationsLink", ResourceType = typeof(Resources))]
        [StringLength(1024)]
        public string RegulationsLink { get; set; }
    }
}
