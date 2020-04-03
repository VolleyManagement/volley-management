using Microsoft.Azure.Cosmos.Table;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage
{
    public class UserEntity : TableEntity
    {
        public string RoleId { get; set; }

        public UserEntity()
        {
	        
        }

        public UserEntity(User user)
        : this(user.Tenant, user.Id, user.Role)
        {
        }

        public UserEntity(TenantId tenant, UserId id)
        {
            PartitionKey = tenant.ToString();
            RowKey = id.ToString();
        }

        public UserEntity(TenantId tenant, UserId id, RoleId role)
        {
            PartitionKey = tenant.ToString();
            RowKey = id.ToString();

            RoleId = role?.ToString();
        }
    }
}