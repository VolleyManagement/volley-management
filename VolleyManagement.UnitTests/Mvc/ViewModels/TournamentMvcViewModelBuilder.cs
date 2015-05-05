namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.Tournaments;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Builder for test MVC tournament view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentMvcViewModelBuilder
    {
        /// <summary>
        /// Holds test tournament view model instance
        /// </summary>
        private TournamentViewModel _tournamentViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentMvcViewModelBuilder"/> class
        /// </summary>
        public TournamentMvcViewModelBuilder()
        {
            _tournamentViewModel = new TournamentViewModel()
            {
                Id = 1,
                Name = "Name",
                Description = "Description 1",
                Season = 2014,
                Scheme = TournamentSchemeEnum.Two,
                RegulationsLink = "http://default.com",
                ApplyingPeriodStart = new DateTime(2015, 06, 02),
                ApplyingPeriodEnd = new DateTime(2015, 09, 02),
                GamesStart = new DateTime(2015, 09, 03),
                GamesEnd = new DateTime(2015, 12, 03),
                TransferStart = new DateTime(2015, 10, 01),
                TransferEnd = new DateTime(2015, 11, 01)
            };
        }

        /// <summary>
        /// Sets id of test tournament view model
        /// </summary>
        /// <param name="id">Id for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithId(int id)
        {
            _tournamentViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test tournament view model
        /// </summary>
        /// <param name="name">Name for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithName(string name)
        {
            _tournamentViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Sets description of test tournament view model
        /// </summary>
        /// <param name="description">Description for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithDescription(string description)
        {
            _tournamentViewModel.Description = description;
            return this;
        }

        /// <summary>
        /// Sets scheme of test tournament view model
        /// </summary>
        /// <param name="scheme">Scheme for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithScheme(TournamentSchemeEnum scheme)
        {
            _tournamentViewModel.Scheme = scheme;
            return this;
        }

        /// <summary>
        /// Sets season of test tournament view model
        /// </summary>
        /// <param name="season">Season for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithSeason(short season)
        {
            _tournamentViewModel.Season = season;
            return this;
        }

        /// <summary>
        /// Sets regulations link of test tournament view model
        /// </summary>
        /// <param name="regulationsLink">Regulations link for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithRegulationsLink(string regulationsLink)
        {
            _tournamentViewModel.RegulationsLink = regulationsLink;
            return this;
        }

        /// <summary>
        /// Sets tournament start
        /// </summary>
        /// <param name="gamesStart">Games start</param>
        /// <returns>Tournament builder object</returns>
        public TournamentMvcViewModelBuilder WithGamesStart(DateTime gamesStart)
        {
            _tournamentViewModel.GamesStart = gamesStart;
            return this;
        }

        /// <summary>
        /// Sets tournament end
        /// </summary>
        /// <param name="gamesEnd">Games end</param>
        /// <returns>Tournament builder object</returns>
        public TournamentMvcViewModelBuilder WithGamesEnd(DateTime gamesEnd)
        {
            _tournamentViewModel.GamesEnd = gamesEnd;
            return this;
        }

        /// <summary>
        /// Sets applying start date of a tournament
        /// </summary>
        /// <param name="applyingPeriodStart">Applying period start</param>
        /// <returns>Tournament builder object</returns>
        public TournamentMvcViewModelBuilder WithApplyingPeriodStart(DateTime applyingPeriodStart)
        {
            _tournamentViewModel.ApplyingPeriodStart = applyingPeriodStart;
            return this;
        }

        /// <summary>
        /// Sets applying end date of a tournament
        /// </summary>
        /// <param name="applyingPeriodEnd">Applying period end</param>
        /// <returns>Tournament builder object</returns>
        public TournamentMvcViewModelBuilder WithApplyingPeriodEnd(DateTime applyingPeriodEnd)
        {
            _tournamentViewModel.ApplyingPeriodEnd = applyingPeriodEnd;
            return this;
        }

        /// <summary>
        /// Sets tournament transfer start date
        /// </summary>
        /// <param name="transferStart">Start transfer period</param>
        /// <returns>Tournament builder object</returns>
        public TournamentMvcViewModelBuilder WithTransferStart(DateTime transferStart)
        {
            _tournamentViewModel.TransferStart = transferStart;
            return this;
        }

        /// <summary>
        /// Sets tournament transfer end date
        /// </summary>
        /// <param name="transferEnd">End transfer period</param>
        /// <returns>Tournament builder object</returns>
        public TournamentMvcViewModelBuilder WithTransferEnd(DateTime transferEnd)
        {
            _tournamentViewModel.TransferEnd = transferEnd;
            return this;
        }

        /// <summary>
        /// Builds test tournament view model
        /// </summary>
        /// <returns>test tournament view model</returns>
        public TournamentViewModel Build()
        {
            return _tournamentViewModel;
        }
    }
}
