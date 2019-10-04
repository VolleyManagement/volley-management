using Microsoft.Azure.Cosmos.Table;
using VolleyM.Domain.IdentityAndAccess;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage
{
    public class UserEntity : TableEntity
    {
        public UserEntity()
        {

        }

        public UserEntity(User user)
        {
            PartitionKey = user.Tenant.ToString();
            RowKey = user.Id.ToString();
        }
    }
}