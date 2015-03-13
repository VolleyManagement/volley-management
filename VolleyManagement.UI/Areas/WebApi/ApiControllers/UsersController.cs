namespace VolleyManagement.UI.Areas.WebApi.ApiControllers
{
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.OData;

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
        [Queryable]
        public IQueryable<UserViewModel> GetUsers()
        {
            return _userService.Get()
                               .Select(u => UserViewModel.Map(u));
        }

        /// <summary>
        /// Creates Tournament
        /// </summary>
        /// <param name="tournament"> The tournament. </param>
        /// <returns> The <see cref="IHttpActionResult"/>. </returns>
        public IHttpActionResult Post(UserViewModel tournament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tournamentToCreate = tournament.ToDomain();
            _userService.Create(tournamentToCreate);
            tournament.Id = tournamentToCreate.Id;

            return Created(tournament);
        }
    }
}
