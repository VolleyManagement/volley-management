using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage
{
    public class AzureStorageUserRepository : IUserRepository
    {
        public Task<Unit> Add(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<User>> Get(UserId id)
        {
            throw new System.NotImplementedException();
        }
    }
}