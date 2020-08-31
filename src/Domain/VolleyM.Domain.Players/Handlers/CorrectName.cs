﻿using System.Collections.Generic;
using FluentValidation;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Contracts.EventBroker;
using VolleyM.Domain.Players.Events;
using VolleyM.Domain.Players.PlayerAggregate;

namespace VolleyM.Domain.Players.Handlers
{
	public class CorrectName
	{
		public class Request : IRequest<Unit>
		{
			public PlayerId PlayerId { get; set; }

			public string FirstName { get; set; }

			public string LastName { get; set; }

			public override string ToString()
			{
				return $"Name={FirstName} {LastName}";
			}
		}

		public class Validator : AbstractValidator<CorrectName.Request>
		{
			public Validator()
			{
				RuleFor(r => r.FirstName)
					.NotEmpty()
					.MaximumLength(60);

				RuleFor(r => r.LastName)
					.NotEmpty()
					.MaximumLength(60);
			}
		}

		public class Handler : IRequestHandler<Request, Unit>, ICanProduceEvent
		{
			private readonly ICurrentUserProvider _currentUser;
			private readonly IPlayersRepository _repo;

			public Handler(IPlayersRepository repo, ICurrentUserProvider currentUser)
			{
				_repo = repo;
				_currentUser = currentUser;
			}

			public EitherAsync<Error, Unit> Handle(Request request)
			{
				var player = _repo.Get(_currentUser.Tenant, request.PlayerId);

				var result = player
					.Map(p =>
					{
						p.ChangeName(request.FirstName, request.LastName);
						return _repo.Update(p);
					})
					.MatchAsync(
						RightAsync: async right => await right.ToEither(),
						Left: l => l)
					.ToAsync()
					.Do(_ => DomainEvents.Add(new PlayerNameCorrected
					{
						TenantId = _currentUser.Tenant,
						PlayerId = request.PlayerId,
						FirstName = request.FirstName,
						LastName = request.LastName
					}));

				return result;
			}

			public List<IEvent> DomainEvents { get; } = new List<IEvent>();
		}
	}
}