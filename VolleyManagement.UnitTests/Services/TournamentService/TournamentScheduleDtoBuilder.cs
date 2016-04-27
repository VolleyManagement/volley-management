namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.TournamentsAggregate;

    public class TournamentScheduleDtoBuilder
    {
        private const int DEFAULT_ID = 1;

        private const string TEST_START_DATE = "2016-04-02 10:00";

        private const string TEST_END_DATE = "2016-04-05 10:00";

        private TournamentScheduleDto _tournamentScheduleDto;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentScheduleDtoBuilder"/> class
        /// </summary>
        public TournamentScheduleDtoBuilder()
        {
            this._tournamentScheduleDto = new TournamentScheduleDto()
            {
                Id = DEFAULT_ID,
                Name = "Tour1",
                Scheme = TournamentSchemeEnum.One,
                StartDate = DateTime.Parse(TEST_START_DATE),
                EndDate = DateTime.Parse(TEST_END_DATE)
            };
        }

        /// <summary>
        ///  Sets the id of the tournament schedule data transfer object
        /// </summary>
        /// <param name="id">id to set</param>
        /// <returns>Instance of <see cref="TournamentScheduleDtoBuilder"/></returns>
        public TournamentScheduleDtoBuilder WithId(int id)
        {
            this._tournamentScheduleDto.Id = id;
            return this;
        }

        /// <summary>
        /// Sets start date for tournament schedule data transfer object
        /// </summary>
        /// <param name="date">Start date</param>
        /// <returns>Instance of <see cref="TournamentScheduleDtoBuilder"/></returns>
        public TournamentScheduleDtoBuilder WithStartDate(DateTime date)
        {
            this._tournamentScheduleDto.StartDate = date;
            return this;
        }

        /// <summary>
        /// Sets end date for tournament schedule data transfer object
        /// </summary>
        /// <param name="date">End date</param>
        /// <returns>Instance of <see cref="TournamentScheduleDtoBuilder"/></returns>
        public TournamentScheduleDtoBuilder WithEndDate(DateTime date)
        {
            this._tournamentScheduleDto.EndDate = date;
            return this;
        }

        /// <summary>
        /// Sets name for tournament schedule data transfer object
        /// </summary>
        /// <param name="name">Tournament name</param>
        /// <returns>Instance of <see cref="TournamentScheduleDtoBuilder"/></returns>
        public TournamentScheduleDtoBuilder WithName(string name)
        {
            this._tournamentScheduleDto.Name = name;
            return this;
        }

        /// <summary>
        /// Sets number of teams for tournament schedule data transfer object
        /// </summary>
        /// <param name="teamCount">Number of teams</param>
        /// <returns>Instance of <see cref="TournamentScheduleDtoBuilder"/></returns>
        public TournamentScheduleDtoBuilder WithTeamCount(byte teamCount)
        {
            this._tournamentScheduleDto.TeamCount = teamCount;
            return this;
        }

        /// <summary>
        /// Sets scheme for tournament schedule data transfer object
        /// </summary>
        /// <param name="scheme">Tournament's scheme</param>
        /// <returns>Instance of <see cref="TournamentScheduleDtoBuilder"/></returns>
        public TournamentScheduleDtoBuilder WithScheme(TournamentSchemeEnum scheme)
        {
            this._tournamentScheduleDto.Scheme = scheme;
            return this;
        }

        /// <summary>
        ///  Fills test tournament schedule data transfer object with default values
        /// </summary>
        /// <returns>Instance of <see cref="TournamentScheduleDtoBuilder"/></returns>
        public TournamentScheduleDtoBuilder TestTournamemtSchedultDto()
        {
            this._tournamentScheduleDto.Id = DEFAULT_ID;
            this._tournamentScheduleDto.StartDate = DateTime.Parse(TEST_START_DATE);
            this._tournamentScheduleDto.EndDate = DateTime.Parse(TEST_END_DATE);

            return this;
        }

        /// <summary>
        /// Builds tournament schedule data transfer object
        /// </summary>
        /// <returns>Instance of <see cref="TournamentScheduleDtoBuilder"/></returns>
        public TournamentScheduleDto Build()
        {
            return this._tournamentScheduleDto;
        }
    }
}
