namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Crosscutting.Contracts.Providers;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.GameResult;
    using VolleyManagement.Data.Queries.Tournament;
    using VolleyManagement.Domain.GameResultsAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate.Standings;
    using TournamentConstants = VolleyManagement.Domain.Constants.Tournament;
    using TournamentResources = VolleyManagement.Domain.Properties.Resources;

    /// <summary>
    /// Defines TournamentService
    /// </summary>
    public class TournamentService : ITournamentService
    {
        #region Const & Readonly

        private static readonly TournamentStateEnum[] _actualStates =
            {
                TournamentStateEnum.Current, TournamentStateEnum.Upcoming
            };

        private static readonly TournamentStateEnum[] _finishedStates =
            {
                TournamentStateEnum.Finished
            };

        #endregion

        #region Fields

        private readonly ITournamentRepository _tournamentRepository;

        #endregion

        #region Query Objects

        private readonly IQuery<Tournament, UniqueTournamentCriteria> _uniqueTournamentQuery;
        private readonly IQuery<List<Tournament>, GetAllCriteria> _getAllQuery;
        private readonly IQuery<Tournament, FindByIdCriteria> _getTournamentByIdQuery;
        private readonly IQuery<List<GameResult>, TournamentGameResultsCriteria> _getTournamentGameResultsQuery;
        private readonly IQuery<Team, FindByIdCriteria> _getTeamByIdQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="tournamentRepository"> The tournament repository  </param>
        /// <param name="uniqueTournamentQuery"> First By Name object query  </param>
        /// <param name="getAllQuery"> Get All object query. </param>
        /// <param name="getByIdQuery">Get tournament by id query.</param>
        /// <param name="getTournamentGameResultsQuery">Get tournament's game results query.</param>
        /// <param name="getTeamByIdQuery">Get team by its identifier query.</param>
        public TournamentService(
            ITournamentRepository tournamentRepository,
            IQuery<Tournament, UniqueTournamentCriteria> uniqueTournamentQuery,
            IQuery<List<Tournament>, GetAllCriteria> getAllQuery,
            IQuery<Tournament, FindByIdCriteria> getByIdQuery,
            IQuery<List<GameResult>, TournamentGameResultsCriteria> getTournamentGameResultsQuery,
            IQuery<Team, FindByIdCriteria> getTeamByIdQuery)
        {
            _tournamentRepository = tournamentRepository;
            _uniqueTournamentQuery = uniqueTournamentQuery;
            _getAllQuery = getAllQuery;
            _getTournamentByIdQuery = getByIdQuery;
            _getTournamentGameResultsQuery = getTournamentGameResultsQuery;
            _getTeamByIdQuery = getTeamByIdQuery;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Get all tournaments
        /// </summary>
        /// <returns>All tournaments</returns>
        public List<Tournament> Get()
        {
            return _getAllQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Get only actual tournaments
        /// </summary>
        /// <returns>actual tournaments</returns>
        public List<Tournament> GetActual()
        {
            return GetFilteredTournaments(_actualStates);
        }

        /// <summary>
        /// Get only finished tournaments
        /// </summary>
        /// <returns>Finished tournaments</returns>
        public List<Tournament> GetFinished()
        {
            return GetFilteredTournaments(_finishedStates);
        }

        /// <summary>
        /// Create a new tournament
        /// </summary>
        /// <param name="tournamentToCreate">A Tournament to create</param>
        public void Create(Tournament tournamentToCreate)
        {
            if (tournamentToCreate == null)
            {
                throw new ArgumentNullException("tournamentToCreate");
            }

            ValidateTournament(tournamentToCreate);
            ValidateDivisions(tournamentToCreate.Divisions);
            ValidateGroups(tournamentToCreate.Divisions);

            _tournamentRepository.Add(tournamentToCreate);
        }

        /// <summary>
        /// Finds a Tournament by id
        /// </summary>
        /// <param name="id">id for search</param>
        /// <returns>A found Tournament</returns>
        public Tournament Get(int id)
        {
            return _getTournamentByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        /// <summary>
        /// Edit tournament
        /// </summary>
        /// <param name="tournamentToEdit">Tournament to edit</param>
        public void Edit(Tournament tournamentToEdit)
        {
            ValidateTournament(tournamentToEdit, isUpdate: true);

            _tournamentRepository.Update(tournamentToEdit);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Delete tournament by id.
        /// </summary>
        /// <param name="id">The id of tournament to delete.</param>
        public void Delete(int id)
        {
            _tournamentRepository.Remove(id);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Gets standings of the tournament specified by identifier.
        /// </summary>
        /// <param name="id">Identifier of the tournament.</param>
        /// <returns>Standings of the tournament with specified identifier.</returns>
        public Standings GetStandings(int id)
        {
            var gameResults = _getTournamentGameResultsQuery.Execute(new TournamentGameResultsCriteria { TournamentId = id });

            if (gameResults == null)
            {
                throw new ArgumentException(TournamentResources.NonexistentTournamentStanding);
            }

            var standing = new Standings();

            foreach (var gameResult in gameResults)
            {
                bool addHomeTeamEntry = false;
                bool addAwayTeamEntry = false;
                var standingHomeTeamEntry = standing.Entries.SingleOrDefault(tse => tse.TeamId == gameResult.HomeTeamId);
                var standingAwayTeamEntry = standing.Entries.SingleOrDefault(tse => tse.TeamId == gameResult.AwayTeamId);

                if (standingHomeTeamEntry == null)
                {
                    if (CreateStandingEntryForTeam(gameResult.HomeTeamId) == null)
                    {
                        continue;
                    }

                    addHomeTeamEntry = true;
                }

                if (standingAwayTeamEntry == null)
                {
                    if (CreateStandingEntryForTeam(gameResult.AwayTeamId) == null)
                    {
                        continue;
                    }

                    addAwayTeamEntry = true;
                }

                GetGamesStats(standingHomeTeamEntry, standingAwayTeamEntry, gameResult.SetsScore);
                GetSetsStats(standingHomeTeamEntry, standingAwayTeamEntry, gameResult.SetScores);

                if (addHomeTeamEntry)
                {
                    standing.AddEntry(standingHomeTeamEntry);
                }

                if (addAwayTeamEntry)
                {
                    standing.AddEntry(standingAwayTeamEntry);
                }
            }

            return standing.Rebuild();
        }

        #endregion

        #region Private

        private static UniqueTournamentCriteria BuildUniqueTournamentCriteria(Tournament newTournament, bool isUpdate)
        {
            var criteria = new UniqueTournamentCriteria { Name = newTournament.Name };

            if (isUpdate)
            {
                criteria.EntityId = newTournament.Id;
            }

            return criteria;
        }

        private List<Tournament> GetFilteredTournaments(IEnumerable<TournamentStateEnum> statesFilter)
        {
            return Get().Where(t => statesFilter.Contains(t.State)).ToList();
        }

        private void ValidateTournament(Tournament tournament, bool isUpdate = false)
        {
            ValidateUniqueTournamentName(tournament, isUpdate);
            ValidateTournamentDates(tournament);
        }

        private void ValidateUniqueTournamentName(Tournament newTournament, bool isUpdate = false)
        {
            var criteria = BuildUniqueTournamentCriteria(newTournament, isUpdate);
            var tournament = _uniqueTournamentQuery.Execute(criteria);

            if (tournament != null)
            {
                throw new TournamentValidationException(
                    TournamentResources.TournamentNameMustBeUnique,
                    TournamentConstants.UNIQUE_NAME_KEY,
                    "Name");
            }
        }

        private void ValidateTournamentDates(Tournament tournament)
        {
            ValidateTournamentApplyingPeriod(tournament);
            ValidateTournamentGamesPeriod(tournament);
            ValidateTournamentTrasferPeriod(tournament);
        }

        private void ValidateTournamentApplyingPeriod(Tournament tournament)
        {
            // if registration dates comes before current date
            if (TimeProvider.Current.UtcNow >= tournament.ApplyingPeriodStart)
            {
                throw new TournamentValidationException(
                    TournamentResources.LateRegistrationDates,
                    TournamentConstants.APPLYING_START_BEFORE_NOW,
                    TournamentConstants.APPLYING_START_CAPTURE);
            }

            // if registration start date comes after end date
            if (tournament.ApplyingPeriodStart >= tournament.ApplyingPeriodEnd)
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongRegistrationDatesPeriod,
                    TournamentConstants.APPLYING_START_DATE_AFTER_END_DATE,
                    TournamentConstants.APPLYING_START_CAPTURE);
            }

            // if registration end date comes after games start date
            if (tournament.ApplyingPeriodEnd >= tournament.GamesStart)
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongRegistrationGames,
                    TournamentConstants.APPLYING_END_DATE_AFTER_START_GAMES,
                    TournamentConstants.GAMES_START_CAPTURE);
            }

            // ToDo: Revisit this requirement
            ////double totalApplyingPeriodDays = (tournament.ApplyingPeriodEnd - tournament.ApplyingPeriodStart).TotalDays;

            ////// if registration period is little
            ////if (totalApplyingPeriodDays < ExceptionParams.DAYS_BETWEEN_START_AND_END_APPLYING_DATE)
            ////{
            ////    throw new TournamentValidationException(
            ////        MessageList.WrongThreeMonthRule,
            ////        ExceptionParams.APPLYING_PERIOD_LESS_THREE_MONTH,
            ////        ExceptionParams.APPLYING_END_CAPTURE);
            ////}
        }

        private void ValidateTournamentGamesPeriod(Tournament tournament)
        {
            // if games start date comes after end date
            if (tournament.GamesStart >= tournament.GamesEnd)
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongStartTournamentDates,
                    TournamentConstants.START_GAMES_AFTER_END_GAMES,
                    TournamentConstants.GAMES_END_CAPTURE);
            }
        }

        private void ValidateTournamentTrasferPeriod(Tournament tournament)
        {
            // if there is no transfer period
            if (!tournament.TransferStart.HasValue && !tournament.TransferEnd.HasValue)
            {
                return;
            }

            // if transfer start is not specified
            if (!tournament.TransferStart.HasValue && tournament.TransferEnd.HasValue)
            {
                throw new TournamentValidationException(
                    TournamentResources.TransferStartMissing,
                    TournamentConstants.TRANSFER_START_MISSING,
                    TournamentConstants.TRANSFER_START_CAPTURE);
            }

            // if transfer end is not specified
            if (tournament.TransferStart.HasValue && !tournament.TransferEnd.HasValue)
            {
                throw new TournamentValidationException(
                    TournamentResources.TransferEndMissing,
                    TournamentConstants.TRANSFER_END_MISSING,
                    TournamentConstants.TRANSFER_END_CAPTURE);
            }

            // if games start date comes after transfer start date
            if (tournament.GamesStart >= tournament.TransferStart.GetValueOrDefault())
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongTransferStart,
                    TournamentConstants.TRANSFER_PERIOD_BEFORE_GAMES_START,
                    TournamentConstants.TRANSFER_START_CAPTURE);
            }

            // if transfer start date comes after end date
            if (tournament.TransferStart.GetValueOrDefault() >= tournament.TransferEnd.GetValueOrDefault())
            {
                throw new TournamentValidationException(
                    TournamentResources.WrongTransferPeriod,
                    TournamentConstants.TRANSFER_END_BEFORE_TRANSFER_START,
                    TournamentConstants.TRANSFER_END_CAPTURE);
            }

            // if transfer end date comes before games end date
            if (tournament.TransferEnd.GetValueOrDefault() >= tournament.GamesEnd)
            {
                throw new TournamentValidationException(
                    TournamentResources.InvalidTransferEndpoint,
                    TournamentConstants.TRANSFER_END_AFTER_GAMES_END,
                    TournamentConstants.GAMES_END_CAPTURE);
            }
        }

        private void ValidateDivisions(List<Division> divisions)
        {
            ValidateDivisionCount(divisions.Count);
            ValidateUniqueDivisionNames(divisions);
        }

        private void ValidateDivisionCount(int count)
        {
            if (!TournamentValidationSpecification.IsDivisionCountWithinRange(count))
            {
                throw new ArgumentException(
                    string.Format(
                        TournamentResources.DivisionCountOutOfRange,
                        TournamentConstants.MIN_DIVISIONS_COUNT,
                        TournamentConstants.MAX_DIVISIONS_COUNT));
            }
        }

        private void ValidateUniqueDivisionNames(List<Division> divisions)
        {
            if (divisions.Select(d => new { Name = d.Name.ToUpper() }).Distinct().Count() != divisions.Count)
            {
                throw new ArgumentException(TournamentResources.DivisionNamesNotUnique);
            }
        }

        private void ValidateGroups(List<Division> divisions)
        {
            foreach (var division in divisions)
            {
                ValidateGroupCount(division.Groups.Count);
                ValidateUniqueGroupNames(division.Groups);
            }
        }

        private void ValidateGroupCount(int count)
        {
            if (!DivisionValidation.IsGroupCountWithinRange(count))
            {
                throw new ArgumentException(
                    string.Format(
                    TournamentResources.GroupCountOutOfRange,
                    TournamentConstants.MIN_GROUPS_COUNT,
                    TournamentConstants.MAX_GROUPS_COUNT));
            }
        }

        private void ValidateUniqueGroupNames(List<Group> groups)
        {
            if (groups.Select(g => new { Name = g.Name.ToUpper() }).Distinct().Count() != groups.Count)
            {
                throw new ArgumentException(TournamentResources.GroupNamesNotUnique);
            }
        }

        private StandingsEntry CreateStandingEntryForTeam(int id)
        {
            var team = _getTeamByIdQuery.Execute(new FindByIdCriteria { Id = id });

            if (team == null)
            {
                return null;
            }

            return new StandingsEntry
            {
                TeamId = team.Id,
                TeamName = team.Name
            };
        }

        private void GetGamesStats(StandingsEntry homeTeamEntry, StandingsEntry awayTeamEntry, Score setsScore)
        {
            homeTeamEntry.GamesTotal++;
            awayTeamEntry.GamesTotal++;

            switch (setsScore.Home - setsScore.Away)
            {
                case 3: // sets score - 3:0
                    homeTeamEntry.Points += 3;
                    homeTeamEntry.GamesWon++;
                    homeTeamEntry.GamesWithScoreThreeNil++;

                    awayTeamEntry.GamesLost++;
                    awayTeamEntry.GamesWithScoreNilThree++;
                    break;
                case 2: // sets score - 3:1
                    homeTeamEntry.Points += 3;
                    homeTeamEntry.GamesWon++;
                    homeTeamEntry.GamesWithScoreThreeOne++;

                    awayTeamEntry.GamesLost++;
                    awayTeamEntry.GamesWithScoreOneThree++;
                    break;
                case 1: // sets score - 3:2
                    homeTeamEntry.Points += 2;
                    homeTeamEntry.GamesWon++;
                    homeTeamEntry.GamesWithScoreThreeTwo++;

                    awayTeamEntry.Points++;
                    awayTeamEntry.GamesLost++;
                    awayTeamEntry.GamesWithScoreTwoThree++;
                    break;
                case -1: // sets score - 2:3
                    homeTeamEntry.Points++;
                    homeTeamEntry.GamesLost++;
                    homeTeamEntry.GamesWithScoreTwoThree++;

                    awayTeamEntry.Points += 2;
                    awayTeamEntry.GamesWon++;
                    awayTeamEntry.GamesWithScoreThreeTwo++;
                    break;
                case -2: // sets score - 1:3
                    homeTeamEntry.GamesLost++;
                    homeTeamEntry.GamesWithScoreOneThree++;

                    awayTeamEntry.Points += 3;
                    awayTeamEntry.GamesWon++;
                    awayTeamEntry.GamesWithScoreThreeOne++;
                    break;
                case -3: // sets score - 0:3
                    homeTeamEntry.GamesLost++;
                    homeTeamEntry.GamesWithScoreNilThree++;

                    awayTeamEntry.Points += 3;
                    awayTeamEntry.GamesWon++;
                    awayTeamEntry.GamesWithScoreThreeNil++;
                    break;
            }
        }

        private void GetSetsStats(StandingsEntry homeTeamEntry, StandingsEntry awayTeamEntry, List<Score> setScores)
        {
            foreach (var setScore in setScores)
            {
                if (setScore.Home > setScore.Away)
                {
                    homeTeamEntry.SetsWon++;
                    awayTeamEntry.SetsLost++;
                }
                else if (setScore.Home < setScore.Away)
                {
                    homeTeamEntry.SetsLost++;
                    awayTeamEntry.SetsWon++;
                }

                homeTeamEntry.BallsWon += setScore.Home;
                homeTeamEntry.BallsLost += setScore.Away;
                awayTeamEntry.BallsWon += setScore.Away;
                awayTeamEntry.BallsLost += setScore.Home;
            }

            if (homeTeamEntry.SetsLost != 0)
            {
                homeTeamEntry.SetsRatio = (float)homeTeamEntry.SetsWon / homeTeamEntry.SetsLost;
            }

            if (awayTeamEntry.SetsLost != 0)
            {
                awayTeamEntry.SetsRatio = (float)awayTeamEntry.SetsWon / awayTeamEntry.SetsLost;
            }

            if (homeTeamEntry.BallsLost != 0)
            {
                homeTeamEntry.BallsRatio = (float)homeTeamEntry.BallsWon / homeTeamEntry.BallsLost;
            }

            if (awayTeamEntry.BallsLost != 0)
            {
                awayTeamEntry.BallsRatio = (float)awayTeamEntry.BallsWon / awayTeamEntry.BallsLost;
            }
        }

        #endregion
    }
}
