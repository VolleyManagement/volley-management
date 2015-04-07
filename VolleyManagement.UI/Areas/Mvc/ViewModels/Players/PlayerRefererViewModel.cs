namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using Domain = VolleyManagement.Domain.Players;

    /// <summary>
    /// Represents PlayerViewModel and referer link.
    /// </summary>
    public class PlayerRefererViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRefererViewModel" /> class.
        /// </summary>
        /// <param name="player">Domain player model.</param>
        /// <param name="referer">Referer controller name.</param>
        public PlayerRefererViewModel(Domain.Player player, string referer)
        {
            Player = PlayerViewModel.Map(player);
            Referer = referer;
        }

        /// <summary>
        /// Gets Player ViewModel.
        /// </summary>
        public PlayerViewModel Player { get; private set; }

        /// <summary>
        /// Gets referer controller name.
        /// </summary>
        public string Referer { get; private set; }
    }
}