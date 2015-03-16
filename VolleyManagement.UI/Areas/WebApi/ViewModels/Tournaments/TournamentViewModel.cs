namespace VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

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

        #region Factory Methods

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="tournament"> Domain object </param>
        /// <returns> View model object </returns>
        public static TournamentViewModel Map(Tournament tournament)
        {
            var tournamentViewModel = new TournamentViewModel
                                          {
                                              Id = tournament.Id,
                                              Name = tournament.Name,
                                              Description = tournament.Description,
                                              Season = tournament.Season,
                                              RegulationsLink = tournament.RegulationsLink,
                                              Scheme = tournament.Scheme.ToString() ////Bug: Use description
                                          };

            return tournamentViewModel;
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        public Tournament ToDomain()
        {
            var tournament = new Tournament();
            tournament.Id = this.Id;
            tournament.Name = this.Name;
            tournament.Description = this.Description;
            tournament.Season = this.Season;
            tournament.RegulationsLink = this.RegulationsLink;
            tournament.Scheme = (TournamentSchemeEnum)Enum.Parse(typeof(TournamentSchemeEnum), this.Scheme);

            return tournament;
        }
        
        #endregion
    }
}