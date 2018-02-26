namespace VolleyManagement.UnitTests.Services.TournamentService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Domain.TournamentsAggregate;

    /// <summary>
    /// Builder for test tournaments
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TournamentBuilder
    {
        private const int TOURNAMENT_DEFAULT_ID = 1;

        private const string TEST_START_DATE = "2016-04-02 10:00";

        private const string TEST_END_DATE = "2016-04-05 10:00";

        /// <summary>
        /// Holds test tournament instance
        /// </summary>
        private Tournament _tournament;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentBuilder"/> class
        /// </summary>
        public TournamentBuilder()
        {
            _tournament = new Tournament
            {
                Id = TOURNAMENT_DEFAULT_ID,
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
                TransferEnd = new DateTime(2015, 11, 01),
                Divisions = new List<Division>
                {
                    new Division()
                    {
                        Id = 1,
                        Name = "Division 1",
                        TournamentId = TOURNAMENT_DEFAULT_ID,
                        Groups = new List<Group>
                        {
                            new Group { Id = 1, Name = "Group 1", DivisionId = 1 },
                            new Group { Id = 2, Name = "Group 2", DivisionId = 1 }
                        }
                    },
                    new Division()
                    {
                        Id = 2,
                        Name = "Division 2",
                        TournamentId = TOURNAMENT_DEFAULT_ID,
                        Groups = new List<Group>
                        {
                            new Group { Id = 3, Name = "Group 1", DivisionId = 2 },
                            new Group { Id = 4, Name = "Group 2", DivisionId = 2 }
                        }
                    }
                }
            };
        }

        public TournamentBuilder WithArchiveParameter(bool isArchive)
        {
            _tournament.IsArchived = isArchive;
            return this;
        }

        /// <summary>
        /// Sets id of test tournament
        /// </summary>
        /// <param name="id">Id for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithId(int id)
        {
            _tournament.Id = id;
            return this;
        }

        /// <summary>
        /// Sets name of test tournament
        /// </summary>
        /// <param name="name">Name for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithName(string name)
        {
            _tournament.Name = name;
            return this;
        }

        /// <summary>
        /// Sets description of test tournament
        /// </summary>
        /// <param name="description">Description for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithDescription(string description)
        {
            _tournament.Description = description;
            return this;
        }

        /// <summary>
        /// Sets scheme of test tournament
        /// </summary>
        /// <param name="scheme">Scheme for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithScheme(TournamentSchemeEnum scheme)
        {
            _tournament.Scheme = scheme;
            return this;
        }

        /// <summary>
        /// Sets season of test tournament
        /// </summary>
        /// <param name="season">Season for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithSeason(short season)
        {
            _tournament.Season = season;
            return this;
        }

        /// <summary>
        /// Sets regulations link of test tournament
        /// </summary>
        /// <param name="regulationsLink">Regulations link for test tournament</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithRegulationsLink(string regulationsLink)
        {
            _tournament.RegulationsLink = regulationsLink;
            return this;
        }

        /// <summary>
        /// Sets tournament start
        /// </summary>
        /// <param name="gamesStart">Games start</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithGamesStart(DateTime gamesStart)
        {
            _tournament.GamesStart = gamesStart;
            return this;
        }

        /// <summary>
        /// Sets tournament end
        /// </summary>
        /// <param name="gamesEnd">Games end</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithGamesEnd(DateTime gamesEnd)
        {
            _tournament.GamesEnd = gamesEnd;
            return this;
        }

        /// <summary>
        /// Sets applying start date of a tournament
        /// </summary>
        /// <param name="applyingPeriodStart">Applying period start</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithApplyingPeriodStart(DateTime applyingPeriodStart)
        {
            _tournament.ApplyingPeriodStart = applyingPeriodStart;
            return this;
        }

        /// <summary>
        /// Sets applying end date of a tournament
        /// </summary>
        /// <param name="applyingPeriodEnd">Applying period end</param>
        /// <returns>Tournament builder object</returns>
        public TournamentBuilder WithApplyingPeriodEnd(DateTime applyingPeriodEnd)
        {
            _tournament.ApplyingPeriodEnd = applyingPeriodEnd;
            return this;
        }

        /// <summary>
        /// Sets tournament transfer start date to a specified date.
        /// </summary>
        /// <param name="transferStart">Date of transfer start.</param>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentBuilder WithTransferStart(DateTime? transferStart)
        {
            _tournament.TransferStart = transferStart;
            return this;
        }

        /// <summary>
        /// Sets tournament transfer end date to a specified date.
        /// </summary>
        /// <param name="transferEnd">Date of transfer end.</param>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentBuilder WithTransferEnd(DateTime? transferEnd)
        {
            _tournament.TransferEnd = transferEnd;
            return this;
        }

        /// <summary>
        /// Sets tournament transfer start date and end date to null.
        /// </summary>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentBuilder WithNoTransferPeriod()
        {
            _tournament.TransferStart = null;
            _tournament.TransferEnd = null;
            return this;
        }

        /// <summary>
        /// Set divisions list
        /// </summary>
        /// <param name="divisions">Divisions list</param>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentBuilder WithDivisions(List<Division> divisions)
        {
            _tournament.Divisions = divisions;
            return this;
        }

        /// <summary>
        /// Clears all tournament's divisions.
        /// </summary>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentBuilder WithNoDivisions()
        {
            _tournament.Divisions.Clear();
            return this;
        }

        /// <summary>
        /// Sets divisions list to divisions with non-unique names.
        /// </summary>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentBuilder WithNonUniqueNameDivisions()
        {
            _tournament.Divisions = new List<Division>
            {
                new Division { Id = 1, Name = "Division 1", TournamentId = TOURNAMENT_DEFAULT_ID },
                new Division { Id = 2, Name = "Division 2", TournamentId = TOURNAMENT_DEFAULT_ID },
                new Division { Id = 3, Name = "Division 1", TournamentId = TOURNAMENT_DEFAULT_ID }
            };

            return this;
        }

        /// <summary>
        /// Set group list for division specified by index.
        /// </summary>
        /// <param name="divisionIdx">Index of the division in tournament divisions.</param>
        /// <param name="groups">Groups list.</param>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentBuilder WithDivisionGroups(int divisionIdx, List<Group> groups)
        {
            _tournament.Divisions[divisionIdx].Groups = groups;
            return this;
        }

        /// <summary>
        /// Clears all groups in tournament's divisions.
        /// </summary>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentBuilder WithNoDivisionsGroups()
        {
            foreach (var division in _tournament.Divisions)
            {
                division.Groups.Clear();
            }

            return this;
        }

        /// <summary>
        /// Sets group lists for all divisions to groups with non-unique names.
        /// </summary>
        /// <returns>Instance of Tournament builder.</returns>
        public TournamentBuilder WithDivisionsNonUniqueNameGroups()
        {
            int startId = 1;

            for (int i = 1; i <= _tournament.Divisions.Count; i++)
            {
                _tournament.Divisions[i - 1].Groups = new List<Group>
                {
                    new Group { Id = startId++, Name = "Group 1", DivisionId = i },
                    new Group { Id = startId++, Name = "Group 2", DivisionId = i },
                    new Group { Id = startId++, Name = "Group 1", DivisionId = i }
                };
            }

            return this;
        }

        public TournamentBuilder TestTournament()
        {
            _tournament.GamesStart = DateTime.Parse(TEST_START_DATE);
            _tournament.GamesEnd = DateTime.Parse(TEST_END_DATE);
            return this;
        }

        /// <summary>
        /// Builds test tournament.
        /// </summary>
        /// <returns>Instance of test tournament.</returns>
        public Tournament Build()
        {
            return _tournament;
        }
    }
}
