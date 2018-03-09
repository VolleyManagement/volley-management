namespace VolleyManagement.UI.Areas.Mvc.ViewModels.Players
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Contracts.Authorization;
    using Domain.PlayersAggregate;

    /// <summary>
    /// The players list view model.
    /// </summary>
    public class PlayersListViewModel
    {
        private const int FIRST_PAGE = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayersListViewModel"/> class
        /// </summary>
        /// <param name="source">All players</param>
        /// <param name="index">Index of page</param>
        /// <param name="size">Number of players on page</param>
        /// <param name="textToSearch">Substring to search</param>
        public PlayersListViewModel(IQueryable<Player> source, int? index, int size, string textToSearch)
        {
            TextToSearch = textToSearch;
            Size = size;
            PageNumber = index ?? FIRST_PAGE;
            NumberOfPages = (int)Math.Ceiling(source.Count() / (double)Size);

            if ((PageNumber > NumberOfPages) || (PageNumber < FIRST_PAGE))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            List<PlayerViewModel> listOfPlayers = new List<PlayerViewModel>(source.Skip((PageNumber - 1) * Size)
                            .Take(Size)
                            .Select(p => PlayerViewModel.Map(p)));

            List = new List<PlayerNameViewModel>();
            foreach (PlayerViewModel player in listOfPlayers)
            {
                List.Add(PlayerNameViewModel.Map(player));
            }
        }

        /// <summary>
        /// Gets or sets substring to search player
        /// </summary>
        public string TextToSearch { get; set; }

        /// <summary>
        /// Gets index of page
        /// </summary>
        public int PageNumber { get; private set; }

        /// <summary>
        /// Gets number of players on page
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Gets number of pages
        /// </summary>
        public int NumberOfPages { get; private set; }

        /// <summary>
        /// Gets list Of Players
        /// </summary>
        public List<PlayerNameViewModel> List { get; private set; }

        /// <summary>
        /// Gets or sets instance of <see cref="AllowedOperations"/> create object
        /// </summary>
        public AllowedOperations AllowedOperations { get; set; }
    }
}