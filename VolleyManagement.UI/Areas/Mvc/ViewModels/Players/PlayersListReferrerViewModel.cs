namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using Generic;

    /// <summary>
    /// Represents PlayersListViewModel and referrer link.
    /// </summary>
    public class PlayersListReferrerViewModel : RefererViewModel<PlayersListViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersListReferrerViewModel" /> class.
        /// </summary>
        /// <param name="player">Players list view model.</param>
        /// <param name="referer">Referrer controller name.</param>
        public PlayersListReferrerViewModel(PlayersListViewModel player, string referer)
            : base(player, referer)
        {
        }
    }
}