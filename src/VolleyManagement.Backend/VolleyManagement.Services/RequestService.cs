namespace VolleyManagement.Services
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Exceptions;
    using Data.Queries.Common;
    using Data.Queries.Player;
    using Domain.RequestsAggregate;
    using Domain.RolesAggregate;
    using Domain.UsersAggregate;

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
        private readonly IQuery<Request, UserToPlayerCriteria> _getRequestUserPlayerQuery;

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
        /// <param name="getRequestUserPlayerQuery">Get request by user's and player's id </param>
        public RequestService(
            IRequestRepository requestRepository,
            IUserRepository userRepository,
            IUserService userService,
            IAuthorizationService authService,
            IQuery<Request, FindByIdCriteria> getRequestByIdQuery,
            IQuery<List<Request>, GetAllCriteria> getAllRequestsQuery,
            IQuery<Request, UserToPlayerCriteria> getRequestUserPlayerQuery)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
            _authService = authService;
            _userService = userService;
            _getRequestByIdQuery = getRequestByIdQuery;
            _getAllRequestsQuery = getAllRequestsQuery;
            _getRequestUserPlayerQuery = getRequestUserPlayerQuery;
        }

        #endregion

        /// <summary>
        /// Confirm request by id
        /// </summary>
        /// <param name="requestId">The id of request to Confirm.</param>
        public void Confirm(int requestId)
        {
            _authService.CheckAccess(AuthOperations.Requests.Confirm);
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

            _userRepository.Update(user);
            _requestRepository.Remove(requestId);
            _userRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Create a new request.
        /// </summary>
        /// <param name="userId"> User's id to link.</param>
        /// <param name="playerId"> Player's id to link</param>
        public void Create(int userId, int playerId)
        {
            var requestExists = _getRequestUserPlayerQuery.Execute(
                new UserToPlayerCriteria { UserId = userId, PlayerId = playerId });
            if (requestExists == null)
            {
                var requestToCreate = new Request
                {
                    PlayerId = playerId,
                    UserId = userId
                };

                _requestRepository.Add(requestToCreate);
                _requestRepository.UnitOfWork.Commit();
            }
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
        public ICollection<Request> Get()
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
