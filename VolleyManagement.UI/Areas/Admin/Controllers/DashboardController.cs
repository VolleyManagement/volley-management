namespace VolleyManagement.UI.Areas.Admin.Controllers
{
    using System.Web.Mvc;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.RolesAggregate;

    /// <summary>
    /// Main view in Admin part of Volley Management web-site
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        /// <param name="authService">Authorization service</param>
        public DashboardController(IAuthorizationService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Returns default view for Admin dashboard
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult Index()
        {
            _authService.CheckAccess(AuthOperations.ViewDashboard.Index);

            return View();
        }
    }
}