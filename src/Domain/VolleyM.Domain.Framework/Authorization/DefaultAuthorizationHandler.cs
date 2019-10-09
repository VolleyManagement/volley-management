using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Framework.Authorization
{
    public class DefaultAuthorizationHandler : IAuthorizationHandler
    {
        public Task<Result<Unit>> AuthorizeUser()
        {
            return Task.FromResult((Result<Unit>)Unit.Value);
        }
    }
}