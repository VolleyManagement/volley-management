namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Domain.RequestsAggregate;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Domain.UsersAggregate;

    /// <summary>
    /// Defines an implementation of <see cref="IRequestService"/> contract.
    /// </summary>
    public class RequestService : IRequestService
    {
        #region Fields

        private readonly IRequestRepository _requestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authService;
        private readonly IQuery<Request, FindByIdCriteria> _getRequestByIdQuery;
        private readonly IQuery<List<Request>, GetAllCriteria> _getAllRequestsQuery;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestService"/> class.
        /// </summary>
        /// <param name="requestRepository"> Read the IRequestRepository instance</param>
        /// <param name="userRepository">Read the IUserRepository instance</param>
        /// <param name="userService">Instance of the class which implements <see cref="IUserService"/> </param>
        /// <param name="authService">Instance of class which implements <see cref="IAuthorizationService"/></param>
        /// <param name="getRequestByIdQuery">Get request by it's id </param>
        /// <param name="getAllRequestsQuery">Get list of all requests</param>
        public RequestService(
            IRequestRepository requestRepository,
            IUserRepository userRepository,
            IUserService userService,
            IAuthorizationService authService,
            IQuery<Request, FindByIdCriteria> getRequestByIdQuery,
            IQuery<List<Request>, GetAllCriteria> getAllRequestsQuery)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
            _authService = authService;
            _userService = userService;
            _getRequestByIdQuery = getRequestByIdQuery;
            _getAllRequestsQuery = getAllRequestsQuery;
        }

        #endregion

        /// <summary>
        /// Approve request by id
        /// </summary>
        /// <param name="requestId">The id of request to approve.</param>
        public void Approve(int requestId)
        {
            _authService.CheckAccess(AuthOperations.Requests.Approve);
            var request = Get(requestId);

            if (request == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.RequestNotFound, requestId);
            }

            var user = _userService.GetUser(request.UserId);

            if (user == null)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.UserNotFound);
            }

            user.PlayerId = request.PlayerId;

            try
            {
                _userRepository.Update(user);
            }
            catch (ConcurrencyException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.UserNotFound, ex);
            }

            _userRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Create a new request.
        /// </summary>
        /// <param name="userId"> User's id to link.</param>
        /// <param name="playerId"> Player's id to link</param>
        public void Create(int userId, int playerId)
        {
            var requestToCreate = new Request
            {
                PlayerId = playerId,
                UserId = userId
            };

            _requestRepository.Add(requestToCreate);
            _requestRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Decline request by id.
        /// </summary>
        /// <param name="requestId">The id of request to decline.</param>
        public void Decline(int requestId)
        {
            _authService.CheckAccess(AuthOperations.Requests.Decline);

            try
            {
                DeleteRequest(requestId);
            }
            catch (InvalidKeyValueException ex)
            {
                throw new MissingEntityException(ServiceResources.ExceptionMessages.RequestNotFound, requestId, ex);
            }
        }

        /// <summary>
        /// Method for getting all requests.
        /// </summary>
        /// <returns>All requests.</returns>
        public List<Request> Get()
        {
            _authService.CheckAccess(AuthOperations.Requests.ViewList);
            return _getAllRequestsQuery.Execute(new GetAllCriteria());
        }

        /// <summary>
        /// Finds a Request by id.
        /// </summary>
        /// <param name="id">id for search.</param>
        /// <returns>A found Request.</returns>
        public Request Get(int id)
        {
            return _getRequestByIdQuery.Execute(new FindByIdCriteria { Id = id });
        }

        private void DeleteRequest(int requestId)
        {
                _requestRepository.Remove(requestId);
                _requestRepository.UnitOfWork.Commit();
        }
    }
}
