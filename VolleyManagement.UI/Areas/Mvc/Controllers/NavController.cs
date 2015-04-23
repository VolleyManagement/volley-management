namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using VolleyManagement.UI.Areas.Mvc.ViewModels.Menu;

    public class NavController : Controller
    {
        public PartialViewResult Menu(string id)
        
        {
            IEnumerable<MenuItemViewModel> items 
                = new List<MenuItemViewModel>
                {
                    new MenuItemViewModel() { Name = "Tournaments", Route = "Tournaments" },
                    new MenuItemViewModel() { Name = "Players", Route = "Players" }
                };

            return PartialView(items);
        }
    }
}