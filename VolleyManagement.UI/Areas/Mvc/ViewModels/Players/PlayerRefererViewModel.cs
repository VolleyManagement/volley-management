namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using Contracts.Authorization;
    using Domain.PlayersAggregate;
    using Generic;

    /// <summary>
    /// Represents PlayerViewModel and referrer link.
    /// </summary>
    public class PlayerRefererViewModel : RefererViewModel<PlayerViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRefererViewModel" /> class.
        /// </summary>
        /// <param name="player">Domain Player model.</param>
        /// <param name="referer">Referrer controller name.</param>
        public PlayerRefererViewModel(Player player, string referer)
            : base(PlayerViewModel.Map(player), referer)
        {
        }

        /// <summary>
        /// Gets or sets instance of <see cref="AllowedOperations"/> create object
        /// </summary>
        public AllowedOperations AllowedOperations { get; set; }
    }
}