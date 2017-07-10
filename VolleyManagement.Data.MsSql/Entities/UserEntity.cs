namespace VolleyManagement.Data.MsSql.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// DAL user model
    /// </summary>
    public class UserEntity
    {
        private ICollection<LoginInfoEntity> _loginProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEntity"/> class.
        /// </summary>
        public UserEntity()
        {
            _loginProviders = new List<LoginInfoEntity>();
        }

        /// <summary>
        /// Gets or sets id of user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets login of user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets email of user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets full name of user
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets player id
        /// </summary>
        public int? PlayerId { get; set; }

        /// <summary>
        /// Gets or sets player belongs to user
        /// </summary>
        public virtual PlayerEntity Player { get; set; }

        /// <summary>
        /// Gets or sets telephone of user
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user account is blocked .
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Gets or sets login providers
        /// </summary>
        public virtual ICollection<LoginInfoEntity> LoginProviders
        {
            get { return _loginProviders; }
            set { _loginProviders = value; }
        }

        /// <summary>
        /// Gets or sets roles assigned to current user
        /// </summary>
        public virtual ICollection<RoleEntity> Roles { get; set; }
    }
}