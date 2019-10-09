using System.Threading.Tasks;

namespace VolleyM.Domain.Contracts.Crosscutting
{
    public interface IAuthorizationHandler
    {
        Task<Result<Unit>> AuthorizeUser();
    }
}