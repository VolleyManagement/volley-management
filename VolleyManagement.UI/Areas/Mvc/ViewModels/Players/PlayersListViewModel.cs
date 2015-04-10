﻿namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
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
        private const int FIRST_PAGE = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersListViewModel"/> class
        /// </summary>
        /// <param name="source">All players</param>
        /// <param name="index">Index of page</param>
        /// <param name="size">Number of players on page</param>
        public PlayersListViewModel(IQueryable <Player> source, int? index, int size)
        {

            this.Size = size;
            this.PageNumber = index ?? FIRST_PAGE;
            this.NumberOfPages = (int) Math.Ceiling(source.Count() / (double)Size);

            if ((index > this.NumberOfPages) || (index < FIRST_PAGE))
                throw new ArgumentOutOfRangeException();

            List<PlayerViewModel> listOfPlayers = new List<PlayerViewModel>(source.Skip((this.PageNumber - 1) * Size)
                .Take(Size)
                .ToList()
                .Select(p => PlayerViewModel.Map(p)));

            this.List = new List<PlayerInListViewModel>();
            foreach(PlayerViewModel player in listOfPlayers)
            {
                this.List.Add(new PlayerInListViewModel(){FullName = player.LastName + " " + player.FirstName, Id = player.Id});
            }
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
        /// Number of pages
        /// </summary>
        public int NumberOfPages { get; private set; }

        /// <summary>
        /// List Of Players
        /// </summary>
        public List<PlayerInListViewModel> List { get; private set; }
    }
}