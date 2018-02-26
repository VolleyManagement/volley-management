namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ViewModels.Menu;
    using Contracts.Authorization;
    using Domain.RolesAggregate;

    /// <summary>
    /// Represents navigation menu controller
    /// </summary>
    public class NavigationController : Controller
    {
        /// <summary>
        /// Holds AuthorizationService instance
        /// </summary>
        private readonly IAuthorizationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationController"/> class
        /// </summary>
        /// <param name="authService">The authorization service</param>
        public NavigationController(IAuthorizationService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Create list of menu items
        /// </summary>
        /// <returns> Partial view of the menu. </returns>
        public PartialViewResult Menu()
        {
            var authorization = _authService.GetAllowedOperations(AuthOperations.Tournaments.ViewArchived);
            string controllerName = (string)Request.RequestContext.RouteData.Values["controller"];
            List<MenuItemViewModel> items = new List<MenuItemViewModel>
                {
                    new MenuItemViewModel() { Name = "Tournaments", Controller = "Tournaments", Action = "Index" },
                    new MenuItemViewModel() { Name = "Players", Controller = "Players", Action = "Index" },
                    new MenuItemViewModel() { Name = "Teams", Controller = "Teams", Action = "Index" },
                    new MenuItemViewModel() { Name = "Contributors", Controller = "ContributorsTeam", Action = "Index" }
                };

            if (authorization.IsAllowed(AuthOperations.Tournaments.ViewArchived))
            {
                items.Add(new MenuItemViewModel() { Name = "Archived tournaments", Controller = "Tournaments", Action = "Archived" });
            }

            var queryableItems = items.AsQueryable();

            var currentItem = queryableItems.Where(item =>
                string.Equals(item.Controller, controllerName, StringComparison.OrdinalIgnoreCase));

            if (currentItem.Count() > 0)
            {
                currentItem.First().IsCurrent = true;
            }

            return PartialView(queryableItems);
        }
    }
}