namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentsAggregate;
    using UI.Areas.Mvc.ViewModels.Division;
    using UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Builder for test MVC tournament view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentMvcViewModelBuilder
    {
        private const int TOURNAMENT_DEFAULT_ID = 1;

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
                Id = TOURNAMENT_DEFAULT_ID,
                Name = "Name",
                Description = "Description 1",
                Location = "Location 1",
                Season = 2014,
                Scheme = TournamentSchemeEnum.Two,
                RegulationsLink = "http://default.com",
                ApplyingPeriodStart = new DateTime(2015, 06, 02),
                ApplyingPeriodEnd = new DateTime(2015, 09, 02),
                GamesStart = new DateTime(2015, 09, 03),
                GamesEnd = new DateTime(2015, 12, 03),
                TransferStart = new DateTime(2015, 10, 01),
                TransferEnd = new DateTime(2015, 11, 01),
                IsTransferEnabled = true,
                Divisions = new List<DivisionViewModel>
                {
                    new DivisionViewModel()
                    {
                        Id = 1,
                        Name = "Division 1",
                        TournamentId = TOURNAMENT_DEFAULT_ID,
                        Groups = new List<GroupViewModel>
                        {
                            new GroupViewModel { Id = 1, Name = "Group 1", DivisionId = 1 },
                            new GroupViewModel { Id = 2, Name = "Group 2", DivisionId = 1 }
                        }
                    },
                    new DivisionViewModel()
                    {
                        Id = 2,
                        Name = "Division 2",
                        TournamentId = TOURNAMENT_DEFAULT_ID,
                        Groups = new List<GroupViewModel>
                        {
                            new GroupViewModel { Id = 3, Name = "Group 1", DivisionId = 2 },
                            new GroupViewModel { Id = 4, Name = "Group 2", DivisionId = 2 }
                        }
                    }
                }
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
        /// Sets location of test tournament view model
        /// </summary>
        /// <param name="location">Location for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithLocation(string location)
        {
            _tournamentViewModel.Location = location;
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
        /// Sets tournament transfer end date to a specified date.
        /// </summary>
        /// <param name="transferStart">Date of transfer start.</param>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentMvcViewModelBuilder WithTransferStart(DateTime? transferStart)
        {
            _tournamentViewModel.TransferStart = transferStart;
            return this;
        }

        /// <summary>
        /// Sets tournament transfer end date to a specified date.
        /// </summary>
        /// <param name="transferEnd">Date of transfer end.</param>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentMvcViewModelBuilder WithTransferEnd(DateTime? transferEnd)
        {
            _tournamentViewModel.TransferEnd = transferEnd;
            return this;
        }

        /// <summary>
        /// Sets tournament transfer start date and end date to null.
        /// </summary>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentMvcViewModelBuilder WithNoTransferPeriod()
        {
            _tournamentViewModel.TransferStart = null;
            _tournamentViewModel.TransferEnd = null;
            return this;
        }

        /// <summary>
        /// Sets divisions of test tournament view model
        /// </summary>
        /// <param name="divisions">Divisions for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentMvcViewModelBuilder WithDivisions(List<DivisionViewModel> divisions)
        {
            _tournamentViewModel.Divisions = divisions;
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
