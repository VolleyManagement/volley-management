namespace VolleyManagement.API.ViewModels.Tournaments
{
    using System;
    using Domain.TournamentsAggregate;
    using Crosscutting.Contracts.Extensions;

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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        public short Season { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        public string RegulationsLink { get; set; }

        /// <summary>
        /// Gets or sets start of a tournament registration
        /// </summary>

        public DateTime ApplyingPeriodStart { get; set; }

        /// <summary>
        /// Gets or sets end of a tournament registration
        /// </summary>
        public DateTime ApplyingPeriodEnd { get; set; }

        /// <summary>
        /// Gets or sets start of a tournament
        /// </summary>
        public DateTime GamesStart { get; set; }

        /// <summary>
        /// Gets or sets end of a tournament
        /// </summary>
        public DateTime GamesEnd { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether transfer enabled state
        /// </summary>
        public bool IsTransferEnabled { get; set; }

        /// <summary>
        /// Gets or sets start of a transfer period
        /// </summary>
        public DateTime? TransferStart { get; set; }

        /// <summary>
        /// Gets or sets end of a transfer period
        /// </summary>
        public DateTime? TransferEnd { get; set; }

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

        #endregion
    }
}