namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameResults;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Tournaments;
    using VolleyManagement.UnitTests.Services.GameService;

    /// <summary>
    /// Schedule view model builder
    /// </summary>
    [ExcludeFromCodeCoverage]
     internal class ScheduleViewModelBuilder
    {
        /// <summary>
        /// Holds test schedule view model instance
        /// </summary>
        private ScheduleViewModel _scheduleViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleViewModelBuilder"/> class
        /// </summary>
        public ScheduleViewModelBuilder()
        {
            _scheduleViewModel = new ScheduleViewModel()
            {
                TournamentId = 1,
                TournamentName = "Name",
                NumberOfRounds = 1,
                Rounds = new GameServiceTestFixture().TestGameResults()
                                     .Build().GroupBy(d => d.Round)
                                     .ToDictionary(d => d.Key, c => c.OrderBy(t => t.GameDate)
                                     .Select(x => GameResultViewModel.Map(x)).ToList())
            };
        }

        /// <summary>
        /// Sets the schedule view model TournamentId
        /// </summary>
        /// <param name="tournamentId">Tournament view model TournamentId</param>
        /// <returns>Schedule view model builder object</returns>
        public ScheduleViewModelBuilder WithTournamentId(int tournamentId)
        {
            _scheduleViewModel.TournamentId = tournamentId;
            return this;
        }

        /// <summary>
        /// Sets the schedule view model TournamentName
        /// </summary>
        /// <param name="tournamentName">Tournament view model TournamentName</param>
        /// <returns>Schedule view model builder object</returns>
        public ScheduleViewModelBuilder WithTournamentName(string tournamentName)
        {
            _scheduleViewModel.TournamentName = tournamentName;
            return this;
        }

        /// <summary>
        /// Sets the schedule view model NumberOfRounds
        /// </summary>
        /// <param name="numberOfRounds">Tournament view model NumberOfRounds</param>
        /// <returns>Schedule view model builder object</returns>
        public ScheduleViewModelBuilder WithNumberOfRounds(byte numberOfRounds)
        {
            _scheduleViewModel.NumberOfRounds = numberOfRounds;
            return this;
        }

        /// <summary>
        /// Builds a test schedule view model.
        /// </summary>
        /// <returns>Test schedule view model.</returns>
        public ScheduleViewModel Build()
        {
            return _scheduleViewModel;
        }
    }
}
