using System;
using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess.Handlers
{
	[Obsolete]
    public class GetUserOld
    {
        public class Request : IRequest<User>
        {
            public UserId UserId { get; set; }

            public TenantId Tenant { get; set; }

            public override string ToString()
            {
                return $"UserId:{UserId};Tenant:{Tenant}";
            }
        }

        public class Handler : IRequestHandlerOld<Request, User>
        {
            private readonly IUserRepositoryOld _repository;

            public Handler(IUserRepositoryOld repository)
            {
                _repository = repository;
            }

            public Task<Either<Error, User>> Handle(Request request)
            {
                return _repository.Get(request.Tenant, request.UserId);
            }
        }
    }
}