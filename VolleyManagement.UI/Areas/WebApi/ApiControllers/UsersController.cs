namespace VolleyManagement.UI.Areas.WebApi.ApiControllers
{
    using System.Linq;
    using System.Web.Http;
    using System.Web.OData;

    using VolleyManagement.Contracts;
    using VolleyManagement.UI.Areas.WebApi.ViewModels.Users;

    /// <summary>
    /// The tournaments controller.
    /// </summary>
    public class UsersController : ODataController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService"> The user service. </param>
        public UsersController(IUserService userService)
        {
            this._userService = userService;
        }

        /// <summary>
        /// Gets tournaments
        /// </summary>
        /// <returns> Tournament list. </returns>
        [EnableQuery]
        public IQueryable<UserViewModel> GetUsers()
        {
            return _userService.Get()
                               .Select(u => UserViewModel.Map(u));
        }

        /// <summary>
        /// Creates User
        /// </summary>
        /// <param name="user"> The user. </param>
        /// <returns> The action result <see cref="IHttpActionResult"/>. </returns>
        public IHttpActionResult Post(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = user.ToDomain();
            _userService.Create(userToCreate);
            user.Id = userToCreate.Id;

            return Created(user);
        }
    }
}
