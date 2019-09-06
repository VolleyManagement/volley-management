using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contributors;

namespace VolleyM.API.Contributors
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContributorsController : ControllerBase
    {
        private readonly IRequestHandler<GetAllContributors.Request, List<ContributorDto>> _handler;

        public ContributorsController(IRequestHandler<GetAllContributors.Request, List<ContributorDto>> handler)
        {
            _handler = handler;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _handler.Handle(new GetAllContributors.Request());

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("protected")]
        public async Task<IActionResult> GetProtected()
        {
            var result = await _handler.Handle(new GetAllContributors.Request());

            return Ok(result);
        }
    }
}