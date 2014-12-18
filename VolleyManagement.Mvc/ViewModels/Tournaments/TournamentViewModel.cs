namespace VolleyManagement.Mvc.ViewModels.Tournaments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.Mvc.App_GlobalResources;

    /// <summary>
    /// TournamentViewModel for Create and Edit actions
    /// </summary>
    public class TournamentViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentViewModel"/> class.
        /// </summary>
        public TournamentViewModel()
        {
            Scheme = TournamentSchemeEnum.One;
            InitializeSeasonsList();
        }

        /// <summary>
        /// Gets or sets the list of seasons.
        /// </summary>
        /// <value>The list of seasons.</value>
        public List<string> SeasonsList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of tournament.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Name.
        /// </summary>
        /// <value>Name of tournament.</value>
        [Display(Name = "TournamentName", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(60)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        [Display(Name = "TournamentDescription", ResourceType = typeof(ViewModelResources))]
        [StringLength(300)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        [Display(Name = "TournamentSeason", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(9)]
        public string Season { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        [Display(Name = "TournamentScheme", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public TournamentSchemeEnum Scheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        [Display(Name = "TournamentRegulationsLink", ResourceType = typeof(ViewModelResources))]
        [StringLength(255)]
        public string RegulationsLink { get; set; }

        /// <summary>
        /// Initializes list of seasons.
        /// </summary>
        private void InitializeSeasonsList()
        {
            SeasonsList = new List<string>();
            int currentYear = DateTime.Now.Year;
            const int yearsRange = 16;
            const int yearsBeforeToday = 5;
            for (int i = 0; i < yearsRange; i++)
            {
                int year = currentYear - yearsBeforeToday + i;
                SeasonsList.Add(string.Format("{0}/{1}", year, year + 1));
            }
        }
    }
}