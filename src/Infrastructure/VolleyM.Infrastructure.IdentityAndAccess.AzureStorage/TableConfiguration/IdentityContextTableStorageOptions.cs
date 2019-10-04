namespace VolleyM.Infrastructure.IdentityAndAccess.AzureStorage.TableConfiguration
{
    public class IdentityContextTableStorageOptions
    {
        public string ConnectionString { get; set; }

        public string UsersTable { get; set; } = "users";
    }
}