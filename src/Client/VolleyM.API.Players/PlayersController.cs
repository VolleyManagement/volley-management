using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VolleyM.API.Contracts;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players.Handlers;

using ApiPlayer = VolleyM.API.Players.Player;
using DomainPlayer = VolleyM.Domain.Players.PlayerAggregate.Player;

namespace VolleyM.API.Players
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlayersController : ControllerBase
	{
		private readonly IRequestHandler<Create.Request, DomainPlayer> _handler;
		private readonly IMapper _mapper;

		public PlayersController(IRequestHandler<Create.Request, DomainPlayer> handler, IMapper mapper)
		{
			_handler = handler;
			_mapper = mapper;
		}

		[HttpPost]
		[Route("")]
		public Task<IActionResult> Create(Create.Request request)
		{
			return _handler.ExecuteHandler(request, _mapper.Map<ApiPlayer>);
		}
	}
}