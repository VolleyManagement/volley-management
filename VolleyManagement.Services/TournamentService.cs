namespace VolleyManagement.Services
{
    using System;
    using System.Linq;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Domain.Tournaments;

    using ExceptionParams = VolleyManagement.Services.Constants;
    using MessageList = VolleyManagement.Domain.Properties.Resources;

    /// <summary>
    /// Defines TournamentService
    /// </summary>
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentService"/> class
        /// </summary>
        /// <param name="tournamentRepository">The tournament repository</param>
        public TournamentService(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        /// <summary>
        /// Method to get all tournaments
        /// </summary>
        /// <returns>All tournaments</returns>
        public IQueryable<Tournament> Get()
        {
            return _tournamentRepository.Find();
        }

        /// <summary>
        /// Create a new tournament
        /// </summary>
        /// <param name="tournamentToCreate">A Tournament to create</param>
        public void Create(Tournament tournamentToCreate)
        {
            if (tournamentToCreate != null)
            {
                IsTournamentNameUnique(tournamentToCreate);
                AreDatesValid(tournamentToCreate);
            }

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
            var tournament = _tournamentRepository.FindWhere(t => t.Id == id).Single();
            return tournament;
        }

        /// <summary>
        /// Edit tournament
        /// </summary>
        /// <param name="tournamentToEdit">Tournament to edit</param>
        public void Edit(Tournament tournamentToEdit)
        {
            IsTournamentNameUnique(tournamentToEdit);
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
        /// Checks whether tournament name is unique or not.
        /// </summary>
        /// <param name="newTournament">tournament to edit or create</param>
        private void IsTournamentNameUnique(Tournament newTournament)
        {
            var tournament = _tournamentRepository.FindWhere(t => t.Name == newTournament.Name
                && t.Id != newTournament.Id).FirstOrDefault();

            if (tournament != null)
            {
                throw new TournamentValidationException(
                   MessageList.TournamentNameMustBeUnique, ExceptionParams.UNIQUE_NAME_KEY, "Name");
            }
        }

        /// <summary>
        /// Checks the tournament dates
        /// </summary>
        /// <param name="tournament">Tournament to check</param>
        private void AreDatesValid(Tournament tournament)
        {
            // if registration dates before now
            if (DateTime.UtcNow >= tournament.ApplyingPeriodStart)
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

            double totalApplyingPeriodDays = (tournament.ApplyingPeriodEnd - tournament.ApplyingPeriodStart).TotalDays;

            // if registration period is little
            if (totalApplyingPeriodDays < Constants.DAYS_BETWEEN_START_AND_END_APPLYING_DATE)
            {
                throw new TournamentValidationException(
                    MessageList.WrongThreeMonthRule,
                    ExceptionParams.APPLYING_PERIOD_LESS_THREE_MONTH,
                    ExceptionParams.APPLYING_END_CAPTURE);
            }

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

            // if games date go after transfer start
            if (tournament.GamesStart >= tournament.TransferStart)
            {
                throw new TournamentValidationException(
                    MessageList.WrongTransferStart,
                    ExceptionParams.TRANSFER_PERIOD_BEFORE_GAMES_START,
                    ExceptionParams.TRANSFER_START_CAPTURE);
            }

            // if transfer start goes after transfer end
            if (tournament.TransferStart >= tournament.TransferEnd)
            {
                throw new TournamentValidationException(
                    MessageList.WrongTransferPeriod,
                    ExceptionParams.TRANSFER_END_BEFORE_TRANSFER_START,
                    ExceptionParams.TRANSFER_END_CAPTURE);
            }

            // fransfer end before tournament end date
            if (tournament.TransferEnd >= tournament.GamesEnd)
            {
                throw new TournamentValidationException(
                    MessageList.InvalidTransferEndpoint,
                    ExceptionParams.TRANSFER_END_AFTER_GAMES_END,
                    ExceptionParams.GAMES_END_CAPTURE);
            }
        }
    }
}