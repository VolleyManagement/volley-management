﻿using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess.Handlers
{
    public class CreateUser
    {
        public class Request : IRequest<Unit>
        {
            public UserId UserId { get; set; }

            public TenantId Tenant { get; set; }

            public override string ToString()
            {
                return $"UserId:{UserId};Tenant:{Tenant}";
            }
        }

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<Unit>> Handle(Request request)
            {
                var existing = await _repository.Get(request.Tenant, request.UserId);

                if (existing.IsSuccessful)
                {
                    return Error.Conflict();
                }

                var user = new User(request.UserId, request.Tenant);
                return await _repository.Add(user);
            }
        }
    }
}
