namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.GamesAggregate;
    using UI.Areas.Mvc.ViewModels.GameResults;

    /// <summary>
    /// Builder for test MVC tournament results view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentResultsViewModelBuilder
    {
        private const string DATE_A = "2016-04-03 10:00";

        private const string DATE_B = "2016-04-07 10:00";

        private const string DATE_C = "2016-04-10 10:00";

        private const string URL_A = "http://test-url-a.com";

        private const string URL_B = "http://test-url-b.com";

        private const string URL_C = "http://test-url-c.com";

        private const string URL_D = "http://test-url-d.com";

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
                        GameScore = new ScoreViewModel(3, 2),
                        SetScores = new List<ScoreViewModel>
                        {
                            new ScoreViewModel(25, 20),
                            new ScoreViewModel(24, 26),
                            new ScoreViewModel(28, 30),
                            new ScoreViewModel(25, 22),
                            new ScoreViewModel(27, 25),
                        },
                        GameDate = DateTime.Parse(DATE_A),
                        Round = 1,
                        TournamentId = 1,
                        UrlToGameVideo = URL_A,
                    },
                    new GameResultViewModel
                    {
                        Id = 2,
                        HomeTeamId = 1,
                        AwayTeamId = 3,
                        HomeTeamName = "TeamNameA",
                        AwayTeamName = "TeamNameC",
                        IsTechnicalDefeat = false,
                        GameScore = new ScoreViewModel(3, 1),
                        SetScores = new List<ScoreViewModel>
                        {
                            new ScoreViewModel(26, 28),
                            new ScoreViewModel(25, 15),
                            new ScoreViewModel(25, 21),
                            new ScoreViewModel(29, 27),
                            new ScoreViewModel(),
                        },
                        GameDate = DateTime.Parse(DATE_B),
                        Round = 2,
                        TournamentId = 1,
                        UrlToGameVideo = URL_B,
                    },
                    new GameResultViewModel
                    {
                        Id = 3,
                        HomeTeamId = 2,
                        AwayTeamId = 3,
                        HomeTeamName = "TeamNameB",
                        AwayTeamName = "TeamNameC",
                        IsTechnicalDefeat = true,
                        GameScore = new ScoreViewModel(0, 3),
                        SetScores = new List<ScoreViewModel>
                        {
                            new ScoreViewModel(0, 25),
                            new ScoreViewModel(0, 25),
                            new ScoreViewModel(0, 25),
                            new ScoreViewModel(),
                            new ScoreViewModel(),
                        },
                        GameDate = DateTime.Parse(DATE_C),
                        Round = 3,
                        TournamentId = 1,
                        UrlToGameVideo = URL_C,
                    },
                },
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
