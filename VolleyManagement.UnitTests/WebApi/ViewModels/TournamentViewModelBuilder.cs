namespace VolleyManagement.UnitTests.WebApi.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Tournaments;

    /// <summary>
    /// Builder for test tournament view models
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentViewModelBuilder
    {
        /// <summary>
        /// Holds test tournament view model instance
        /// </summary>
        private TournamentViewModel _tournamentViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentViewModelBuilder"/> class
        /// </summary>
        public TournamentViewModelBuilder()
        {
            _tournamentViewModel = new TournamentViewModel()
            {
                Id = 1,
                Name = "Name",
                Description = "Description 1",
                Season = 2014,
                Scheme = "2",
                RegulationsLink = "http://default.com"
            };
        }

        /// <summary>
        /// Sets id of test tournament view model
        /// </summary>
        /// <param name="id">Id for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentViewModelBuilder WithId(int id)
        {
            _tournamentViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test tournament view model
        /// </summary>
        /// <param name="name">Name for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentViewModelBuilder WithName(string name)
        {
            _tournamentViewModel.Name = name;
            return this;
        }

        /// <summary>
        /// Sets description of test tournament view model
        /// </summary>
        /// <param name="description">Description for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentViewModelBuilder WithDescription(string description)
        {
            _tournamentViewModel.Description = description;
            return this;
        }

        /// <summary>
        /// Sets scheme of test tournament view model
        /// </summary>
        /// <param name="scheme">Scheme for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentViewModelBuilder WithScheme(string scheme)
        {
            _tournamentViewModel.Scheme = scheme;
            return this;
        }

        /// <summary>
        /// Sets season of test tournament view model
        /// </summary>
        /// <param name="season">Season for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentViewModelBuilder WithSeason(short season)
        {
            _tournamentViewModel.Season = season;
            return this;
        }

        /// <summary>
        /// Sets regulations link of test tournament view model
        /// </summary>
        /// <param name="regulationsLink">Regulations link for test tournament view model</param>
        /// <returns>Tournament view model builder object</returns>
        public TournamentViewModelBuilder WithRegulationsLink(string regulationsLink)
        {
            _tournamentViewModel.RegulationsLink = regulationsLink;
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
