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
        /// Tournament field.
        /// </summary>
        private Tournament _tournament;

        public Tournament Tournament
        {
            get 
            { 
     
            _tournament = new Tournament();
            _tournament.Name = Name;
            _tournament.Description = Description;
            _tournament.Season = Season;
            _tournament.Scheme = Scheme;
            _tournament.RegulationsLink = RegulationsLink;
            return _tournament;
            }
            set
            {
                _tournament = value;
            }
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