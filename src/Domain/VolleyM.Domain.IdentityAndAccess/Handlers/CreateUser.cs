using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Domain.IdentityAndAccess.Handlers
{
    public class CreateUser
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

        public class Handler : IRequestHandler<Request, User>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<User>> Handle(Request request)
            {
                var existing = await _repository.Get(request.Tenant, request.UserId);

                if (existing.IsSuccessful)
                {
                    return Error.Conflict();
                }

                var user = new User(request.UserId, request.Tenant);

                //FIXME: Assign ROle
                return await _repository.Add(user);
            }
        }
    }
}
