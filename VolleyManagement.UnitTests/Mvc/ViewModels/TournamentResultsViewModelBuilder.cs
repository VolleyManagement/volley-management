namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Builder for test MVC tournament results view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentResultsViewModelBuilder
    {
        private const string DATE_A = "2016-04-03 10:00";

        private const string DATE_B = "2016-04-07 10:00";

        private const string DATE_C = "2016-04-10 10:00";

        /// <summary>
        /// Holds test Tournament results view model instance
        /// </summary>
        private TournamentResultsViewModel _tournamentResultsViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentResultsViewModelBuilder"/> class
        /// </summary>
        public TournamentResultsViewModelBuilder()
        {
            _tournamentResultsViewModel = new TournamentResultsViewModel()
            {
                Id = 1,
                Name = "Name",
                GameResults = new List<GameResultViewModel>
                {
                    new GameResultViewModel
                    {
                         Id = 1,
                         HomeTeamId = 1,
                         AwayTeamId = 2,
                         HomeTeamName = "TeamNameA",
                         AwayTeamName = "TeamNameB",
                         IsTechnicalDefeat = false,
                         SetsScore = new Score(3, 2),
                         SetScores = new List<Score>
                         {
                             new Score(25, 20),
                             new Score(24, 26),
                             new Score(28, 30),
                             new Score(25, 22),
                             new Score(27, 25)
                         },
                         GameDate = DateTime.Parse(DATE_A),
                         Round = 1,
                         TournamentId = 1
                    },
                    new GameResultViewModel
                    {
                         Id = 2,
                         HomeTeamId = 1,
                         AwayTeamId = 3,
                         HomeTeamName = "TeamNameA",
                         AwayTeamName = "TeamNameC",
                         IsTechnicalDefeat = false,
                         SetsScore = new Score(3, 1),
                         SetScores = new List<Score>
                         {
                             new Score(26, 28),
                             new Score(25, 15),
                             new Score(25, 21),
                             new Score(29, 27),
                             new Score()
                         },
                         GameDate = DateTime.Parse(DATE_B),
                         Round = 2,
                         TournamentId = 1
                    },
                    new GameResultViewModel
                    {
                        Id = 3,
                        HomeTeamId = 2,
                         AwayTeamId = 3,
                         HomeTeamName = "TeamNameB",
                         AwayTeamName = "TeamNameC",
                         IsTechnicalDefeat = true,
                         SetsScore = new Score(0, 3),
                         SetScores = new List<Score>
                         {
                             new Score(0, 25),
                             new Score(0, 25),
                             new Score(0, 25),
                             new Score(),
                             new Score()
                         },
                         GameDate = DateTime.Parse(DATE_C),
                         Round = 3,
                         TournamentId = 1
                    }
                }
            };
        }

        /// <summary>
        /// Sets id of test tournament
        /// </summary>
        /// <param name="id">Id of tournament</param>
        /// <returns>Tournament results view model builder object</returns>
        public TournamentResultsViewModelBuilder WithId(int id)
        {
            _tournamentResultsViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test tournament
        /// </summary>
        /// <param name="name">Tournament name</param>
        /// <returns>Tournament results view model builder object</returns>
        public TournamentResultsViewModelBuilder WithTournamentName(string name)
        {
            _tournamentResultsViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Builds test Tournament results view model
        /// </summary>
        /// <returns>test Tournament results view model</returns>
        public TournamentResultsViewModel Build()
        {
            return _tournamentResultsViewModel;
        }
    }
}
