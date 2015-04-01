namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using VolleyManagement.Domain.Players;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Players;

    public class PagedPlayersViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedPlayersViewModel"/> class
        /// </summary>
        /// <param name="source">All players</param>
        /// <param name="index">Index of page</param>
        /// <param name="size">Number of players on page</param>
        public PagedPlayersViewModel(IQueryable<Player> source, int index, int size)
        {
            this.Size = size;
            this.Index = index + 1;
            this.NumberOfPages = (int) Math.Ceiling(source.Count() / (double)Size);
            this.List = new List<Player>(source.Skip(index * Size).Take(Size));
        }

        /// <summary>
        /// Index of page
        /// </summary>
        public int Index { get; private set; }

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
        public List<Player> List { get; private set; }
    }
}