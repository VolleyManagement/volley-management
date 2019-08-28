using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace VolleyM.API.Contributors
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContributorsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            return Ok(new List<Contributor> {
                new Contributor {FullName = "API1", CourseDirection = "All", Team = "Special"},
                new Contributor {FullName = "API2", CourseDirection = "All", Team = "Special"},
            });
        }
    }
}