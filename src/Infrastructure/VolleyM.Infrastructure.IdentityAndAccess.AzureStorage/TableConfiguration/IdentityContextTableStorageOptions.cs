using VolleyM.Infrastructure.AzureStorage;

namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration
{
    public class IdentityContextTableStorageOptions : AzureTableStorageOptions
    {
        public string UsersTable { get; set; } = "users";
    }
}