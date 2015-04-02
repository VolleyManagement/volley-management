namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;
    using VolleyManagement.UI.Areas.Mvc.Mappers;

    public class PlayersListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersListViewModel"/> class
        /// </summary>
        /// <param name="source">All players</param>
        /// <param name="index">Index of page</param>
        /// <param name="size">Number of players on page</param>
        public PlayersListViewModel(IQueryable<Player> source, int? index, int size)
        {
            this.Size = size;
            this.PageNumber = index ?? 1;
            this.NumberOfPages = (int) Math.Ceiling(source.Count() / (double)Size);

            if (index > this.NumberOfPages)
                throw new ArgumentOutOfRangeException();

            this.List = new List<PlayerViewModel>(source.Skip((this.PageNumber - 1) * Size).Take(Size)
                .ToList()
                .Select(p => PlayerViewModel.Map(p)));
        }

        /// <summary>
        /// Index of page
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Number of players on page
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Numder of pages
        /// </summary>
        public int NumberOfPages { get; private set; }

        /// <summary>
        /// List Of Players
        /// </summary>
        public List<PlayerViewModel> List { get; private set; }
    }
}