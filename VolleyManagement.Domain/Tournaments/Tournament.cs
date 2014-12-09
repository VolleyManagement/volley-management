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
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of tournament.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Name.
        /// </summary>
        /// <value>Name of tournament.</value>
        [Display(Name = "TournamentName", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(Resources))]
        [StringLength(80)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        [Display(Name = "TournamentDescription", ResourceType = typeof(Resources))]
        [StringLength(1024)]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "This field is not set.")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        [Display(Name = "TournamentSeason", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(Resources))]
        [StringLength(40)]
        public string Season { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        [Display(Name = "TournamentScheme", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(Resources))]
        public TournamentSchemeEnum Scheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        [Display(Name = "TournamentRegulationsLink", ResourceType = typeof(Resources))]
        [StringLength(1024)]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "This field is not set.")]
        public string RegulationsLink { get; set; }
    }
}
