using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VolleyM.API.Contracts;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Players;
using VolleyM.Domain.Players.Handlers;

using ApiPlayer = VolleyM.API.Players.Player;
using DomainPlayer = VolleyM.Domain.Players.PlayerAggregate.Player;

namespace VolleyM.API.Players
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlayersController : ControllerBase
	{
		private readonly IRequestHandlerOld<Create.Request, DomainPlayer> _createHandler;
		private readonly IRequestHandlerOld<GetAll.Request, List<PlayerDto>> _getAllHandler;
		private readonly IMapper _mapper;

		public PlayersController(
			IRequestHandlerOld<Create.Request, DomainPlayer> createHandler, 
			IRequestHandlerOld<GetAll.Request, List<PlayerDto>> getAllHandler, 
			IMapper mapper)
		{
			_createHandler = createHandler;
			_mapper = mapper;
			_getAllHandler = getAllHandler;
		}

		[HttpPost]
		[Route("")]
		public Task<IActionResult> Create(Create.Request request)
		{
			return _createHandler.ExecuteHandler(request, _mapper.Map<ApiPlayer>);
		}

		[HttpGet]
		[Route("")]
		public Task<IActionResult> GetAll()
		{
			return _getAllHandler.ExecuteHandler(new GetAll.Request(), _mapper.Map<List<ApiPlayer>>);
		}
	}
}