namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;

    [ExcludeFromCodeCoverage]
    public class TournamentApplyViewModelBuilder
    {
        private const int DEFAULT_ID = 1;

        private TournamentApplyViewModel _tournamentApplyViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentApplyViewModelBuilder"/> class
        /// </summary>
        public TournamentApplyViewModelBuilder()
        {
            _tournamentApplyViewModel = new TournamentApplyViewModel()
            {
                Id = DEFAULT_ID,
                TournamentTitle = "Name",
                Teams = new TeamNameViewModelBuilder().GetList()
            };
        }

        /// <summary>
        /// Sets id of test team view model
        /// </summary>
        /// <param name="id">Id for tournament team view model</param>
        /// <returns>Team view model builder object</returns>
        public TournamentApplyViewModelBuilder WithId(int id)
        {
            _tournamentApplyViewModel.Id = id;
            return this;
        }

        /// <summary>
        /// Sets id of test team view model
        /// </summary>
        /// <param name="id">Id for test team view model</param>
        /// <returns>Team view model builder object</returns>
        public TournamentApplyViewModelBuilder WithTeamId(int id)
        {
            _tournamentApplyViewModel.TeamId = id;
            return this;
        }

        /// <summary>
        /// Sets title of test tournament in view model
        /// </summary>
        /// <param name="title">Title for test tournament in view model</param>
        /// <returns>Team view model builder object</returns>
        public TournamentApplyViewModelBuilder WithTitle(string title)
        {
            _tournamentApplyViewModel.TournamentTitle = title;
            return this;
        }

        /// <summary>
        /// Builds test tournament view model
        /// </summary>
        /// <returns>test tournament view model</returns>
        public TournamentApplyViewModel Build()
        {
            return _tournamentApplyViewModel;
        }
    }
}