using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess.Handlers
{
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
                return null;
            }
        }
    }
}