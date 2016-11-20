namespace VolleyManagement.Services
{
    using System.Collections.Generic;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Domain.RolesAggregate;
    using Domain.TournamentRequestAggregate;
    using Domain.TournamentsAggregate;

    /// <summary>
    /// Defines an implementation of <see cref="ITournamentRequestService"/> contract.
    /// </summary>
    public class TournamentRequestService : ITournamentRequestService
    {
        private readonly ITournamentRequestRepository _tournamentRequestRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IAuthorizationService _authService;
        private readonly IQuery<List<TournamentRequest>, GetAllCriteria> _getAllTournamentRequestsQuery;
        private readonly IQuery<TournamentRequest, FindByIdCriteria> _getTournamentRequestByIdQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentRequestService"/> class.
        /// </summary>
        /// <param name="tournamentRequestRepository"> Read the ITournamentRequestRepository instance</param>
        /// <param name="authService">Instance of class which implements <see cref="IAuthorizationService"/></param>
        /// <param name="getAllTournamentRequestsQuery">Get list of all requests</param>
        /// <param name="getTournamentRequestById">Get request by it's id</param>
        /// <param name="tournamentRepository">Read the ITournamentRepository instance</param>
        public TournamentRequestService(
            ITournamentRequestRepository tournamentRequestRepository,
            IAuthorizationService authService,
            IQuery<List<TournamentRequest>, GetAllCriteria> getAllTournamentRequestsQuery,
            IQuery<TournamentRequest, FindByIdCriteria> getTournamentRequestById,
            ITournamentRepository tournamentRepository)
        {
            _tournamentRequestRepository = tournamentRequestRepository;
            _authService = authService;
            _getAllTournamentRequestsQuery = getAllTournamentRequestsQuery;
            _getTournamentRequestByIdQuery = getTournamentRequestById;
            _tournamentRepository = tournamentRepository;
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

            _tournamentRepository.AddTeamToTournament(tournamentRequest.TeamId, tournamentRequest.TournamentId);
            _tournamentRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Create a new request
        /// </summary>
        /// <param name="userId">Id of user that ask for link</param>
        /// <param name="tournamentId"> Tournament's id</param>
        /// <param name="teamId"> Team's id</param>
        public void Create(int userId, int tournamentId, int teamId)
        {
            TournamentRequest tournamentRequest = new TournamentRequest()
            {
                TeamId = teamId,
                UserId = userId,
                TournamentId = tournamentId
            };
            _tournamentRequestRepository.Add(tournamentRequest);
            _tournamentRequestRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Decline the request
        /// </summary>
        /// <param name="requestId">Request's id</param>
        /// <param name="message">Message about reason for decline</param>
        public void Decline(int requestId, string message)
        {
            _authService.CheckAccess(AuthOperations.TournamentRequests.Decline);
            try
            {
                DeleteTournamentRequest(requestId);
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
    }
}
