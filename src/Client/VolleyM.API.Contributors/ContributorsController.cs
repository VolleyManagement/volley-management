using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolleyM.API.Contracts;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contributors;

namespace VolleyM.API.Contributors
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContributorsController : ControllerBase
    {
        private readonly IRequestHandler<GetAll.Request, List<ContributorDto>> _handler;

        public ContributorsController(IRequestHandler<GetAll.Request, List<ContributorDto>> handler)
        {
            _handler = handler;
        }

        [HttpGet]
        [Route("")]
        public Task<IActionResult> GetAll()
        {
            Log.Information("Controller {Action} action called.", nameof(GetAll));

            return _handler.ExecuteHandler(new GetAll.Request());
        }
    }
}