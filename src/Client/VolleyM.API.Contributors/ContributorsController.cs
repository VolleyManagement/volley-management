﻿using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using VolleyM.API.Contracts;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contributors;

namespace VolleyM.API.Contributors
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContributorsController : ControllerBase
	{
		private readonly IRequestHandlerOld<GetAllOld.Request, List<ContributorDto>> _handler;
		private readonly IMapper _mapper;

		public ContributorsController(IRequestHandlerOld<GetAllOld.Request, List<ContributorDto>> handler, IMapper mapper)
		{
			_handler = handler;
			_mapper = mapper;
		}

		[HttpGet]
		[Route("")]
		public Task<IActionResult> GetAll()
		{
			Log.Information("Controller {Action} action called.", nameof(GetAll));

			return _handler.ExecuteHandlerOld(new GetAllOld.Request(), _mapper.Map<List<Contributor>>);
		}
	}
}