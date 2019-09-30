﻿using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess.Handlers
{
    public class CreateUser
    {
        public class Request : IRequest<Unit>
        {
            public UserId Id { get; set; }

            public TenantId Tenant { get; set; }
        }

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;
            }

            public Task<Unit> Handle(Request request)
            {
                var user = new User(request.Id, request.Tenant);
                return _repository.Add(user);
            }
        }
    }
}