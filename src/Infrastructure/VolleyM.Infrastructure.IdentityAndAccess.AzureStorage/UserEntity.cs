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
        : this(user.Tenant, user.Id)
        {
        }

        public UserEntity(TenantId tenant, UserId id)
        {
            PartitionKey = tenant.ToString();
            RowKey = id.ToString();
        }
    }
}