namespace VolleyManagement.Data.MsSql.Entities
{
    /// <summary>
    /// Represents information about Login provider
    /// </summary>
    public class LoginInfoEntity
    {
        /// <summary>
        /// User which owns this entity
        /// </summary>
        public virtual UserEntity User { get; set; }

        /// <summary>
        /// Unique user identifier in realm of provider
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Name of the provider
        /// </summary>
        public string LoginProvider { get; set; }
    }
}