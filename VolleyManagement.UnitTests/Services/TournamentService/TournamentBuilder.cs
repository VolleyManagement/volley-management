namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Moq;
    using VolleyManagement.Domain.Providers;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Builder for test tournaments
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentBuilder
    {
        public const int TRANSFER_PERIOD_DAYS = 10;

        public const int TRANSFER_PERIOD_MONTH = 6;

        /// <summary>
        /// Holds test tournament instance
        /// </summary>
        private Tournament _tournament;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentBuilder"/> class
        /// </summary>
        public TournamentBuilder()
        {
            this._tournament = new Tournament
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



                //ApplyingPeriodStart = _mockNowDate.AddDays(1),
                //ApplyingPeriodEnd = _mockNowDate.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH)
                //        .AddDays(1),
                //GamesStart = _mockNowDate.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + 1),
                //GamesEnd = _mockNowDate.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + TRANSFER_PERIOD_MONTH),
                //TransferStart = _mockNowDate.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + 1).AddDays(1),
                //TransferEnd = _mockNowDate.AddMonths(Domain.Constants.Tournament.MINIMUN_REGISTRATION_PERIOD_MONTH + 1)
                //        .AddDays(TRANSFER_PERIOD_DAYS)
            };
        }

        /// <summary>
        /// Sets id of test tournament
        /// </summary>
        /// <param name="id">Id for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithId(int id)
        {
            this._tournament.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test tournament
        /// </summary>
        /// <param name="name">Name for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithName(string name)
        {
            this._tournament.Name = name;
            return this;
        }

        /// <summary>
        /// Sets description of test tournament
        /// </summary>
        /// <param name="description">Description for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithDescription(string description)
        {
            this._tournament.Description = description;
            return this;
        }

        /// <summary>
        /// Sets scheme of test tournament
        /// </summary>
        /// <param name="scheme">Scheme for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithScheme(TournamentSchemeEnum scheme)
        {
            this._tournament.Scheme = scheme;
            return this;
        }

        /// <summary>
        /// Sets season of test tournament
        /// </summary>
        /// <param name="season">Season for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithSeason(short season)
        {
            this._tournament.Season = season;
            return this;
        }

        /// <summary>
        /// Sets regulations link of test tournament
        /// </summary>
        /// <param name="regulationsLink">Regulations link for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithRegulationsLink(string regulationsLink)
        {
            this._tournament.RegulationsLink = regulationsLink;
            return this;
        }

        /// <summary>
        /// Sets tournament start
        /// </summary>
        /// <param name="gamesStart">Games start</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithGamesStart(DateTime gamesStart)
        {
            this._tournament.GamesStart = gamesStart;
            return this;
        }

        /// <summary>
        /// Sets tournament end
        /// </summary>
        /// <param name="gamesEnd">Games end</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithGamesEnd(DateTime gamesEnd)
        {
            this._tournament.GamesEnd = gamesEnd;
            return this;
        }

        /// <summary>
        /// Sets applying start date of a tournament
        /// </summary>
        /// <param name="applyingPeriodStart">Applying period start</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithApplyingPeriodStart(DateTime applyingPeriodStart)
        {
            this._tournament.ApplyingPeriodStart = applyingPeriodStart;
            return this;
        }

        /// <summary>
        /// Sets applying end date of a tournament
        /// </summary>
        /// <param name="applyingPeriodEnd">Applying period end</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithApplyingPeriodEnd(DateTime applyingPeriodEnd)
        {
            this._tournament.ApplyingPeriodEnd = applyingPeriodEnd;
            return this;
        }

        /// <summary>
        /// Sets tournament transfer start date
        /// </summary>
        /// <param name="transferStart">Start transfer period</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithTransferStart(DateTime transferStart)
        {
            this._tournament.TransferStart = transferStart;
            return this;
        }

        /// <summary>
        /// Sets tournament transfer end date
        /// </summary>
        /// <param name="transferEnd">End transfer period</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithTransferEnd(DateTime transferEnd)
        {
            this._tournament.TransferEnd = transferEnd;
            return this;
        }

        /// <summary>
        /// Builds test tournament
        /// </summary>
        /// <returns>Test tournament</returns>
        public Tournament Build()
        {
            return this._tournament;
        }

        /// <summary>
        /// Mocks the dateB
        /// </summary>
        /// <returns>Mocked date</returns>
        //private DateTime MockDate()
        //{
        //    var gamesStart = new Mock<TimeProvider>();
        //    gamesStart.SetupGet(t => t.UtcNow).Returns(new DateTime(2015, 06, 01));
        //    TimeProvider.Current = gamesStart.Object;
        //    return TimeProvider.Current.UtcNow;
        //}
    }
}
