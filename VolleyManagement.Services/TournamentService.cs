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
    using VolleyManagement.Data.Queries.Tournaments;
    using VolleyManagement.Domain.TournamentsAggregate;
    using ExceptionParams = VolleyManagement.Domain.Constants.Tournament;
    using MessageList = VolleyManagement.Domain.Properties.Resources;

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

        private readonly IQuery<Tournament, FindByIdCriteria> _getByIdQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="tournamentRepository"> The tournament repository  </param>
        /// <param name="uniqueTournamentQuery"> First By Name object query  </param>
        /// <param name="getAllQuery"> Get All object query. </param>
        /// <param name="getByIdQuery"> Get by ID object query.</param>
        public TournamentService(
            ITournamentRepository tournamentRepository,
            IQuery<Tournament, UniqueTournamentCriteria> uniqueTournamentQuery,
            IQuery<List<Tournament>, GetAllCriteria> getAllQuery,
            IQuery<Tournament, FindByIdCriteria> getByIdQuery)
        {
            _tournamentRepository = tournamentRepository;
            this._uniqueTournamentQuery = uniqueTournamentQuery;
            this._getAllQuery = getAllQuery;
            this._getByIdQuery = getByIdQuery;
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
            return this.GetFilteredTournaments(_actualStates);
        }

        /// <summary>
        /// Get only finished tournaments
        /// </summary>
        /// <returns>Finished tournaments</returns>
        public List<Tournament> GetFinished()
        {
            return this.GetFilteredTournaments(_finishedStates);
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

            IsTournamentNameUnique(tournamentToCreate);
            AreDatesValid(tournamentToCreate);

            _tournamentRepository.Add(tournamentToCreate);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Finds a Tournament by id
        /// </summary>
        /// <param name="id">id for search</param>
        /// <returns>A found Tournament</returns>
        public Tournament Get(int id)
        {
            var criteria = new FindByIdCriteria { Id = id };
            return _getByIdQuery.Execute(criteria);
        }

        /// <summary>
        /// Edit tournament
        /// </summary>
        /// <param name="tournamentToEdit">Tournament to edit</param>
        public void Edit(Tournament tournamentToEdit)
        {
            IsTournamentNameUnique(tournamentToEdit, isUpdate: true);
            AreDatesValid(tournamentToEdit);
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

        /// <summary>
        /// Checks whether tournament name is unique or not.
        /// </summary>
        /// <param name="newTournament">tournament to edit or create</param>
        /// <param name="isUpdate">Specifies operation</param>
        private void IsTournamentNameUnique(Tournament newTournament, bool isUpdate = false)
        {
            var criteria = BuildUniqueTournamentCriteria(newTournament, isUpdate);

            var tournament = this._uniqueTournamentQuery.Execute(criteria);

            if (tournament != null)
            {
                throw new TournamentValidationException(
                    MessageList.TournamentNameMustBeUnique,
                    ExceptionParams.UNIQUE_NAME_KEY,
                    "Name");
            }
        }

        /// <summary>
        /// Checks the tournament dates
        /// </summary>
        /// <param name="tournament">Tournament to check</param>
        private void AreDatesValid(Tournament tournament)
        {
            // ToDo: Re-factor it - code is hard to read

            // if registration dates before now
            if (TimeProvider.Current.UtcNow >= tournament.ApplyingPeriodStart)
            {
                throw new TournamentValidationException(
                    MessageList.LateRegistrationDates,
                    ExceptionParams.APPLYING_START_BEFORE_NOW,
                    ExceptionParams.APPLYING_START_CAPTURE);
            }

            // if registration start date after end date
            if (tournament.ApplyingPeriodStart >= tournament.ApplyingPeriodEnd)
            {
                throw new TournamentValidationException(
                    MessageList.WrongRegistrationDatesPeriod,
                    ExceptionParams.APPLYING_START_DATE_AFTER_END_DATE,
                    ExceptionParams.APPLYING_START_CAPTURE);
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

            // if registration period is after games start
            if (tournament.ApplyingPeriodEnd >= tournament.GamesStart)
            {
                throw new TournamentValidationException(
                    MessageList.WrongRegistrationGames,
                    ExceptionParams.APPLYING_END_DATE_AFTER_START_GAMES,
                    ExceptionParams.GAMES_START_CAPTURE);
            }

            // if tournament start dates goes after tournament end
            if (tournament.GamesStart >= tournament.GamesEnd)
            {
                throw new TournamentValidationException(
                    MessageList.WrongStartTournamentDates,
                    ExceptionParams.START_GAMES_AFTER_END_GAMES,
                    ExceptionParams.GAMES_END_CAPTURE);
            }

            // if there is transfer period
            if (tournament.TransferStart.HasValue || tournament.TransferEnd.HasValue)
            {
                IsTrasferPeriodValid(tournament);
            }
        }

        /// <summary>
        /// Check whether transfer period is valid
        /// </summary>
        /// <param name="tournament">Tournament to validate</param>
        /// <exception cref="TournamentValidationException">Tournament Validation Exception</exception>
        private void IsTrasferPeriodValid(Tournament tournament)
        {
            // if transfer end missing
            if (tournament.TransferStart.HasValue && !tournament.TransferEnd.HasValue)
            {
                throw new TournamentValidationException(
                    MessageList.TransferEndMissing,
                    ExceptionParams.TRANSFER_END_MISSING,
                    ExceptionParams.TRANSFER_END_CAPTURE);
            }

            // if transfer start missing
            if (!tournament.TransferStart.HasValue && tournament.TransferEnd.HasValue)
            {
                throw new TournamentValidationException(
                    MessageList.TransferStartMissing,
                    ExceptionParams.TRANSFER_START_MISSING,
                    ExceptionParams.TRANSFER_START_CAPTURE);
            }

            // if games date go after transfer start
            if (tournament.GamesStart >= tournament.TransferStart.Value)
            {
                throw new TournamentValidationException(
                    MessageList.WrongTransferStart,
                    ExceptionParams.TRANSFER_PERIOD_BEFORE_GAMES_START,
                    ExceptionParams.TRANSFER_START_CAPTURE);
            }

            // if transfer start goes after transfer end
            if (tournament.TransferStart.Value >= tournament.TransferEnd.Value)
            {
                throw new TournamentValidationException(
                    MessageList.WrongTransferPeriod,
                    ExceptionParams.TRANSFER_END_BEFORE_TRANSFER_START,
                    ExceptionParams.TRANSFER_END_CAPTURE);
            }

            // if transfer end is before tournament end date
            if (tournament.TransferEnd.Value >= tournament.GamesEnd)
            {
                throw new TournamentValidationException(
                    MessageList.InvalidTransferEndpoint,
                    ExceptionParams.TRANSFER_END_AFTER_GAMES_END,
                    ExceptionParams.GAMES_END_CAPTURE);
            }
        }

        private List<Tournament> GetFilteredTournaments(IEnumerable<TournamentStateEnum> statesFilter)
        {
            return this.Get().Where(t => statesFilter.Contains(t.State)).ToList();
        }

        #endregion
    }
}