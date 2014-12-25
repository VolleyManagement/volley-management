namespace VolleyManagement.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Routing;
    using Contracts;
    using VolleyManagement.WebApi.Mappers;
    using VolleyManagement.WebApi.ViewModels.Tournaments;
    using VolleyManagement.WebApi.ViewModels.Users;

    /// <summary>
    /// Defines UserController
    /// </summary>
    public class UsersController : ODataController
    {
        /// <summary>
        /// Holds IUserService instance
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class
        /// </summary>
        /// <param name="userService">The user service</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates new User.
        /// </summary>
        /// <param name="userViewModel">User to create.</param>
        /// <returns>HttpResponse with created user.</returns>
        [HttpPost]
        public HttpResponseMessage Post(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var response = new HttpResponseMessage();
            try
            {
                var userToCreate = ViewModelToDomain.Map(userViewModel);
                _userService.Create(userToCreate);
                userViewModel.Id = userToCreate.Id;
                response = Request.CreateResponse<UserViewModel>(HttpStatusCode.Created, userViewModel);
                return response;
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        /// <summary>
        /// Gets all users from UserService.
        /// </summary>
        /// <returns>All users.</returns>
        public IQueryable<UserViewModel> Get()
        {
            try
            {
                var users = _userService.GetAll().ToList();
                var userViewModels = new List<UserViewModel>();
                foreach (var u in users)
                {
                    userViewModels.Add(DomainToViewModel.Map(u));
                }

                return userViewModels.AsQueryable();
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        /// <summary>
        /// Gets specific user from UserService
        /// </summary>
        /// <param name="key">User id</param>
        /// <returns>Response with specific user</returns>
        public HttpResponseMessage Get([FromODataUri]int key)
        {
            try
            {
                var user = _userService.FindById(key);
                return Request.CreateResponse(HttpStatusCode.OK, DomainToViewModel.Map(user));
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
