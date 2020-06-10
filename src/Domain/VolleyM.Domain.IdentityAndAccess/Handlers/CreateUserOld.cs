﻿using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.IdentityAndAccess.Handlers
{
    public class CreateUserOld
    {
        public class Request : IRequest<User>
        {
            public UserId UserId { get; set; }

            public TenantId Tenant { get; set; }

            public RoleId Role { get; set; }

            public override string ToString()
            {
                return $"UserId:{UserId};Tenant:{Tenant}";
            }
        }

        public class Handler : IRequestHandlerOld<Request, User>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;
            }

            public async Task<Either<Error, User>> Handle(Request request)
            {
                var user = new User(request.UserId, request.Tenant);
                if (request.Role != null)
                {
                    user.AssignRole(request.Role);
                }

                var addResult = await _repository.Add(user);

                return addResult;
            }
        }
    }
}
