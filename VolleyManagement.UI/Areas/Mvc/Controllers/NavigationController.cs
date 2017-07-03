namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using ViewModels.Menu;

    /// <summary>
    /// Represents navigation menu controller
    /// </summary>
    public class NavigationController : Controller
    {
        /// <summary>
        /// Create list of menu items
        /// </summary>
        /// <returns> Partial view of the menu. </returns>
        public PartialViewResult Menu()
        {
            string controllerName = (string)Request.RequestContext.RouteData.Values["controller"];
            IQueryable<MenuItemViewModel> items = new List<MenuItemViewModel>
                {
                    new MenuItemViewModel() { Name = "Tournaments", Controller = "Tournaments", Action = "Index" },
                    new MenuItemViewModel() { Name = "Players", Controller = "Players", Action = "Index" },
                    new MenuItemViewModel() { Name = "Teams", Controller = "Teams", Action = "Index" },
                    new MenuItemViewModel() { Name = "Contributors", Controller = "ContributorsTeam", Action = "Index" }
                }.AsQueryable();

            var currentItem = items.Where(item =>
                string.Equals(item.Controller, controllerName, StringComparison.OrdinalIgnoreCase));

            if (currentItem.Count() > 0)
            {
                currentItem.First().IsCurrent = true;
            }

            return PartialView(items);
        }
    }
}