namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using VolleyManagement.UI.Areas.Mvc.ViewModels.Menu;

    /// <summary>
    /// Represents navigation menu controller
    /// </summary>
    public class NavigationController : Controller
    {
        public PartialViewResult Menu()
        {
            string controllerName = this.Request.RequestContext.RouteData.Values["controller"].ToString();
            IQueryable<MenuItemViewModel> items 
                = new List<MenuItemViewModel>
                {
                    new MenuItemViewModel() { Name = "Tournaments", Controller = "Tournaments", Action = "Index" },
                    new MenuItemViewModel() { Name = "Players", Controller = "Players", Action = "Index" }
                }.AsQueryable();

            var currentItem = items.Where(item => item.Controller.ToLower() == controllerName.ToLower());
            if (currentItem.Count() > 0)
            {
                currentItem.First().IsCurent = true;
            }

            return PartialView(items);
        }
    }
}