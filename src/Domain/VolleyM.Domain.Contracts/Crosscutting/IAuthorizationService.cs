using System.Threading.Tasks;

namespace VolleyM.Domain.Contracts.Crosscutting
{
    public interface IAuthorizationService
    {
        Task<Result<Unit>> AuthorizeUser();
    }
}