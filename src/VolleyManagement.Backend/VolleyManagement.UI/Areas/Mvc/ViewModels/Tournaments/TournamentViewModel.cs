namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Contracts.Authorization;
    using Division;
    using Domain;
    using Domain.TournamentsAggregate;
    using Resources.UI;

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
            InitializeTournamentSchemeList();
            IsTransferEnabled = true;
            Divisions = new List<DivisionViewModel>();
        }

        /// <summary>
        /// Gets or sets the list of seasons.
        /// </summary>
        /// <value>The list of seasons.</value>
        [System.Web.Script.Serialization.ScriptIgnore]
        public Dictionary<short, string> SeasonsList { get; set; }

        /// <summary>
        /// Gets or sets the list of possible tournament schemes.
        /// </summary>
        /// <value>The list of tournament schemes.</value>
        public IList<string> TournamentSchemeList { get; set; }

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
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(
            Constants.Tournament.MAX_NAME_LENGTH,
            ErrorMessageResourceName = "MaxLengthErrorMessage",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [RegularExpression(
            @"^[\S\x20]+$",
            ErrorMessageResourceName = "InvalidEntriesError",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        [Display(Name = "TournamentDescription", ResourceType = typeof(ViewModelResources))]
        [StringLength(
            Constants.Tournament.MAX_DESCRIPTION_LENGTH,
            ErrorMessageResourceName = "MaxLengthErrorMessage",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [RegularExpression(
            @"^[\S\x20]+$",
            ErrorMessageResourceName = "InvalidEntriesError",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        [Display(Name = "TournamentSeason", ResourceType = typeof(ViewModelResources))]
        [Required(
            ErrorMessageResourceName = "FieldRequired",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        [Range(
            Constants.Tournament.MINIMAL_SEASON_YEAR,
            Constants.Tournament.MAXIMAL_SEASON_YEAR,
            ErrorMessageResourceName = "NotInRange",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public short Season { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        [Display(Name = "TournamentScheme", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        public TournamentSchemeEnum Scheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        [Display(Name = "TournamentRegulationsLink", ResourceType = typeof(ViewModelResources))]
        [StringLength(
            Constants.Tournament.MAX_REGULATION_LENGTH,
            ErrorMessageResourceName = "MaxLengthErrorMessage",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string RegulationsLink { get; set; }

        /// <summary>
        /// Gets or sets start of a tournament registration
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "ApplyingPeriodStart", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        public DateTime ApplyingPeriodStart { get; set; }

        /// <summary>
        /// Gets or sets end of a tournament registration
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "ApplyingPeriodEnd", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        public DateTime ApplyingPeriodEnd { get; set; }

        /// <summary>
        /// Gets or sets start of a tournament
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "GamesStart", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        public DateTime GamesStart { get; set; }

        /// <summary>
        /// Gets or sets end of a tournament
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "GamesEnd", ResourceType = typeof(ViewModelResources))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        public DateTime GamesEnd { get; set; }

        /// <summary>
        /// Gets a value indicating whether tournament is new
        /// </summary>
        public bool IsNew
        {
            get { return Id == 0; }
        }

        /// <summary>
        /// Gets a value indicating whether count of divisions is min
        /// </summary>
        public bool IsDivisionsCountMin
        {
            get { return Divisions.Count == Constants.Division.MIN_DIVISIONS_COUNT; }
        }

        /// <summary>
        /// Gets a value indicating whether count of divisions is max
        /// </summary>
        public bool IsDivisionsCountMax
        {
            get { return Divisions.Count == Constants.Division.MAX_DIVISIONS_COUNT; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether transfer enabled state
        /// </summary>
        public bool IsTransferEnabled { get; set; }

        /// <summary>
        /// Gets or sets start of a transfer period
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "TransferStart", ResourceType = typeof(ViewModelResources))]
        public DateTime? TransferStart { get; set; }

        /// <summary>
        /// Gets or sets end of a transfer period
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "TransferEnd", ResourceType = typeof(ViewModelResources))]
        public DateTime? TransferEnd { get; set; }

        /// <summary>
        /// Gets or sets list of divisions
        /// </summary>
        [Display(Name = "Divisions", ResourceType = typeof(ViewModelResources))]
        public IList<DivisionViewModel> Divisions { get; set; }

        /// <summary>
        /// Gets or sets instance of <see cref="AllowedOperations"/> object
        /// </summary>
        // Bug:missing [ScriptIgnore]
        public AllowedOperations Authorization { get; set; }

        #region Factory Methods

        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="tournament"> Domain object </param>
        /// <returns> View model object </returns>
        public static TournamentViewModel Map(Tournament tournament)
        {
            var tournamentViewModel = new TournamentViewModel {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                Season = tournament.Season,
                RegulationsLink = tournament.RegulationsLink,
                Scheme = tournament.Scheme,
                GamesStart = tournament.GamesStart,
                GamesEnd = tournament.GamesEnd,
                ApplyingPeriodStart = tournament.ApplyingPeriodStart,
                ApplyingPeriodEnd = tournament.ApplyingPeriodEnd,
                TransferStart = tournament.TransferStart,
                TransferEnd = tournament.TransferEnd,
                IsTransferEnabled = tournament.TransferStart != null && tournament.TransferEnd != null
            };

            tournamentViewModel.Divisions = tournament.Divisions.Select(d => DivisionViewModel.Map(d)).ToList();

            return tournamentViewModel;
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        public Tournament ToDomain()
        {
            var tournament = new Tournament {
                Id = Id,
                Name = Name,
                Description = Description,
                Season = Season,
                Scheme = Scheme,
                RegulationsLink = RegulationsLink,
                GamesStart = GamesStart,
                GamesEnd = GamesEnd,
                ApplyingPeriodStart = ApplyingPeriodStart,
                ApplyingPeriodEnd = ApplyingPeriodEnd,
                TransferStart = TransferStart,
                TransferEnd = TransferEnd
            };

            tournament.Divisions = Divisions.Select(d => d.ToDomain()).ToList();

            return tournament;
        }
        #endregion

        /// <summary>
        /// Initializes list of seasons.
        /// </summary>
        private void InitializeSeasonsList()
        {
            SeasonsList = new Dictionary<short, string>();
            const short YEARS_RANGE = 16;
            const short YEARS_BEFORE_TODAY = 5;
            var year = (short)(DateTime.Now.Year - YEARS_BEFORE_TODAY);
            for (var i = 0; i < YEARS_RANGE; i++, year++)
            {
                var str = string.Format("{0}/{1}", year, year + 1);

                SeasonsList.Add(year, str);
            }
        }

        /// <summary>
        /// Initializes schemes of tournament. Right now we don't want to have 2,5 variant
        /// </summary>
        private void InitializeTournamentSchemeList()
        {
            TournamentSchemeList = new List<string>();

            foreach (TournamentSchemeEnum scheme in Enum.GetValues(typeof(TournamentSchemeEnum)))
            {
                switch (scheme)
                {
                    case TournamentSchemeEnum.TwoAndHalf:
                        break;
                    case TournamentSchemeEnum.PlayOff:
                        TournamentSchemeList.Add(scheme.ToString());
                        break;
                    default:
                        TournamentSchemeList.Add(((int)scheme).ToString());
                        break;
                }
            }
        }
    }
}