using System.Web.Mvc;

namespace VolleyManagement.UI.Areas.Mvc
{
    public class MvcAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Mvc";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Mvc_default",
                "Mvc/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "VolleyManagement.UI.Areas.Mvc.Controllers" });
        }
    }
}