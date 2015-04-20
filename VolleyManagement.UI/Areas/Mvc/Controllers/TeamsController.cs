namespace VolleyManagement.UI.Areas.Mvc.Controllers
{
    using System.Web.Mvc;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Exceptions;
    using VolleyManagement.UI.Areas.Mvc.Mappers;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.Teams;
    using System.Collections.Generic;

    public class TeamsController : Controller
    {
        /// <summary>
        /// Holds TeamService instance
        /// </summary>
        private readonly ITeamService _teamService;

        public JsonResult Delete(int id, bool? confirm)
        {
            TeamDeleteResultViewModel jsondata;
            try
            {
                if (confirm == true)
                {
                    this._teamService.Delete(id);
                    jsondata = new TeamDeleteResultViewModel() { Message = "Ok", HasDeleted = true };
                }
                else
                {
                    var team = this._teamService.Get().Single(t => t.Id == id);
                    jsondata = new TeamDeleteResultViewModel() { Message = team.Name, HasDeleted = false };
                }
            }
            catch(MissingEntityException ex)
            {
                jsondata = new TeamDeleteResultViewModel() { Message = ex.Message, HasDeleted = false };
            }

            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
    }
}