namespace VolleyManagement.Data.MsSql.Entities
{
    /// <summary>
    /// Represents information about Login provider
    /// </summary>
    public class LoginInfoEntity
    {
        /// <summary>
        /// Gets or sets user which owns this entity
        /// </summary>
        public virtual UserEntity User { get; set; }

        /// <summary>
        /// Gets or sets unique user identifier in realm of provider
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets name of the provider
        /// </summary>
        public string LoginProvider { get; set; }
    }
}