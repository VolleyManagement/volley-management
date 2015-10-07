namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Generic;

    /// <summary>
    /// Represents PlayerViewModel and referer link.
    /// </summary>
    public class PlayerRefererViewModel : RefererViewModel<PlayerViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRefererViewModel" /> class.
        /// </summary>
        /// <param name="player">Domain Player model.</param>
        /// <param name="referer">Referer controller name.</param>
        public PlayerRefererViewModel(Player player, string referer)
            : base(PlayerViewModel.Map(player), referer)
        {
        }
    }
}