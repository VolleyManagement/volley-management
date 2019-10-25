using System.Threading.Tasks;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess.Handlers
{
    [Permission(Permissions.User.GetUser)]
    public class GetUser
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

        public class Handler : IRequestHandler<Request, User>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<User>> Handle(Request request)
            {
                return await _repository.Get(request.Tenant, request.UserId);
            }
        }

        public class Handler1 : IRequestHandler1<Request, User>
        {
            private readonly IUserRepository _repository;

            public Handler1(IUserRepository repository)
            {
                _repository = repository;
            }

            public Task<Either<Error, User>> Handle(Request request)
            {
                return _repository.Get1(request.Tenant, request.UserId);
            }
        }
    }
}