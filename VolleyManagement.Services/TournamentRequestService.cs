namespace VolleyManagement.Services
{
    using System.Collections.Generic;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Crosscutting.Contracts.MailService;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Data.Queries.TournamentRequest;
    using Domain.RolesAggregate;
    using Domain.TournamentRequestAggregate;
    using Domain.TournamentsAggregate;
    using Domain.UsersAggregate;

    /// <summary>
    /// Defines an implementation of <see cref="ITournamentRequestService"/> contract.
    /// </summary>
    public class TournamentRequestService : ITournamentRequestService
    {
        private readonly IMailService _mailService;
        private readonly ITournamentRequestRepository _tournamentRequestRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IAuthorizationService _authService;
        private readonly IQuery<List<TournamentRequest>, GetAllCriteria> _getAllTournamentRequestsQuery;
        private readonly IQuery<TournamentRequest, FindByIdCriteria> _getTournamentRequestByIdQuery;
        private readonly IQuery<TournamentRequest, FindByTeamTournamentCriteria> _getTournamentRequestByAllQuery;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentRequestService"/> class.
        /// </summary>
        /// <param name="tournamentRequestRepository"> Read the ITournamentRequestRepository instance</param>
        /// <param name="authService">Instance of class which implements <see cref="IAuthorizationService"/></param>
        /// <param name="getAllTournamentRequestsQuery">Get list of all requests</param>
        /// <param name="getTournamentRequestById">Get request by it's id</param>
        /// <param name="getTournamentRequestByAll">Get list of all requests by team id and tournament id</param>
        /// <param name="tournamentRepository">Read the ITournamentRepository instance</param>
        /// <param name="mailService">Instance of class which implements <see cref="IMailService"/></param>
        /// <param name="userService">Instance of class which implements <see cref="IUserService"/></param>
        public TournamentRequestService(
            ITournamentRequestRepository tournamentRequestRepository,
            IAuthorizationService authService,
            IQuery<List<TournamentRequest>, GetAllCriteria> getAllTournamentRequestsQuery,
            IQuery<TournamentRequest, FindByIdCriteria> getTournamentRequestById,
            IQuery<TournamentRequest, FindByTeamTournamentCriteria> getTournamentRequestByAll,
            ITournamentRepository tournamentRepository,
            IMailService mailService,
            IUserService userService)
        {
            _tournamentRequestRepository = tournamentRequestRepository;
            _authService = authService;
            _getAllTournamentRequestsQuery = getAllTournamentRequestsQuery;
            _getTournamentRequestByIdQuery = getTournamentRequestById;
            _getTournamentRequestByAllQuery = getTournamentRequestByAll;
            _tournamentRepository = tournamentRepository;
            _mailService = mailService;
            _userService = userService;
        }

        /// <summary>
        /// Confirm the request
        /// </summary>
        /// <param name="requestId">Request's id</param>
        public void Confirm(int requestId)
        {
            _authService.CheckAccess(AuthOperations.TournamentRequests.Confirm);
            var tournamentRequest = Get(requestId);

            if (tournamentRequest == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TournamentRequestNotFound, requestId);
            }

            _tournamentRepository.AddTeamToTournament(tournamentRequest.TeamId, tournamentRequest.TournamentId, tournamentRequest.GroupId, tournamentRequest.DivisionId);
            _tournamentRepository.UnitOfWork.Commit();
             NotifyUser(_userService.GetUser(Get(requestId).UserId).Email);
            _tournamentRequestRepository.Remove(requestId);
            _tournamentRequestRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Create a new request
        /// </summary>
        /// <param name="userId">Id of user that ask for link</param>
        /// <param name="tournamentId"> Tournament's id</param>
        /// <param name="teamId"> Team's id</param>
        /// <param name="groupId"> Groups id</param>
        public void Create(int userId, int tournamentId, int teamId, int groupId)
        {
            var existTournament = _getTournamentRequestByAllQuery.Execute(
                new FindByTeamTournamentCriteria { GroupId = groupId, TeamId = teamId });
            if (existTournament == null)
            {
                TournamentRequest tournamentRequest = new TournamentRequest()
                {
                    TeamId = teamId,
                    UserId = userId,
                    TournamentId = tournamentId,
                    GroupId = groupId
                };
                _tournamentRequestRepository.Add(tournamentRequest);
                _tournamentRequestRepository.UnitOfWork.Commit();
                NotifyAdmins(tournamentRequest);
            }
        }

        /// <summary>
        /// Decline the request
        /// </summary>
        /// <param name="requestId">Request's id</param>
        /// <param name="message">Message about reason for decline</param>
        public void Decline(int requestId, string message)
        {
            _authService.CheckAccess(AuthOperations.TournamentRequests.Decline);
            var email = _userService.GetUser(Get(requestId).UserId).Email;
            try
            {
                DeleteTournamentRequest(requestId);
                NotifyUser(email, message);
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.TournamentRequestNotFound, requestId, ex);
            }
        }

        /// <summary>
        /// Gets list of all requests.
        /// </summary>
        /// <returns>Return list of all requests.</returns>
        public List<TournamentRequest> Get()
        {
            _authService.CheckAccess(AuthOperations.TournamentRequests.ViewList);
            return _getAllTournamentRequestsQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Find request by id.
        /// </summary>
        /// <param name="id">Request id.</param>
        /// <returns>Found request.</returns>
        public TournamentRequest Get(int id)
        {
            return _getTournamentRequestByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        private void DeleteTournamentRequest(int requestId)
        {
            _tournamentRequestRepository.Remove(requestId);
            _tournamentRequestRepository.UnitOfWork.Commit();
        }

        private void NotifyUser(string emailTo)
        {
            string body = Properties.Resources.TournamentRequestConfirmitionLetterBody;
            string subject = Properties.Resources.TournamentRequestLetterSubject;
            EmailMessage emailMessage = new EmailMessage(emailTo, subject, body);
            _mailService.Send(emailMessage);
        }

        private void NotifyUser(string emailTo, string message)
        {
            string subject = Properties.Resources.TournamentRequestLetterSubject;
            EmailMessage emailMessage = new EmailMessage(emailTo, subject, message);
            _mailService.Send(emailMessage);
        }

        private void NotifyAdmins(TournamentRequest request)
        {
            string subject = string.Format(
                Properties.Resources.TournamentRequestEmailSubjectToAdmins,
                request.Id);

            string body = string.Format(
                Properties.Resources.TournamentRequestEmailBodyToAdmins,
                request.Id,
                request.UserId,
                request.TournamentId,
                request.TeamId);
            IList<User> adminList = _userService.GetAdminsList();
            foreach (var admin in adminList)
            {
                EmailMessage emailMessage = new EmailMessage(admin.Email, subject, body);
                _mailService.Send(emailMessage);
            }
        }
    }
}
