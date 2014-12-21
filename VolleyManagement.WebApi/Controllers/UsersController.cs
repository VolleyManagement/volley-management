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
    }
}
