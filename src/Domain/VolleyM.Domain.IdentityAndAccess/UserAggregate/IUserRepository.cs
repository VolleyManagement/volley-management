using System.Threading.Tasks;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.IdentityAndAccess
{
    public interface IUserRepository
    {
        Task<Unit> Add(User user);
    }
}