﻿namespace VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Domain.TournamentsAggregate;
    using Resources.UI;
    using VolleyManagement.Crosscutting.Contracts.Extensions;
    using tournConst = Domain.Constants.Tournament;

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
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        [StringLength(60, ErrorMessageResourceName = "MaxLengthErrorMessage", ErrorMessageResourceType = typeof(ViewModelResources))]
        [RegularExpression(
            @"^[\S\x20]+$",
            ErrorMessageResourceName = "InvalidEntriesError",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        [StringLength(300, ErrorMessageResourceName = "MaxLengthErrorMessage", ErrorMessageResourceType = typeof(ViewModelResources))]
        [RegularExpression(
            @"^[\S\x20]+$", ErrorMessageResourceName = "InvalidEntriesError", ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        [Range(
            tournConst.MINIMAL_SEASON_YEAR,
            tournConst.MAXIMAL_SEASON_YEAR,
            ErrorMessageResourceName = "NotInRange",
            ErrorMessageResourceType = typeof(ViewModelResources))]
        public short Season { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(ViewModelResources))]
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        [StringLength(255, ErrorMessageResourceName = "MaxLengthErrorMessage", ErrorMessageResourceType = typeof(ViewModelResources))]
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
                Scheme = tournament.Scheme.ToDescription(),
                GamesStart = tournament.GamesStart,
                GamesEnd = tournament.GamesEnd,
                ApplyingPeriodStart = tournament.ApplyingPeriodStart,
                ApplyingPeriodEnd = tournament.ApplyingPeriodEnd,
                TransferStart = tournament.TransferStart,
                TransferEnd = tournament.TransferEnd
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
            tournament.Id = Id;
            tournament.Name = Name;
            tournament.Description = Description;
            tournament.Season = Season;
            tournament.RegulationsLink = RegulationsLink;
            tournament.Scheme = Enum.GetValues(typeof(TournamentSchemeEnum))
                .Cast<TournamentSchemeEnum>()
                .FirstOrDefault(v => v.ToDescription() == Scheme);
            tournament.GamesStart = GamesStart;
            tournament.GamesEnd = GamesEnd;
            tournament.ApplyingPeriodStart = ApplyingPeriodStart;
            tournament.ApplyingPeriodEnd = ApplyingPeriodEnd;
            tournament.TransferStart = TransferStart;
            tournament.TransferEnd = TransferEnd;

            return tournament;
        }
        #endregion
    }
}