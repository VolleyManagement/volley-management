namespace VolleyManagement.Dal.MsSql
{
    using System.Data.Entity;

    /// <summary>
    /// volley management database context
    /// </summary>
    internal partial class VolleyManagementContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyManagementContext" /> class.
        /// </summary>
        public VolleyManagementContext()
            : base("VolleyManagementContext")
        {
        }

        /// <summary>
        /// Gets or sets the tournament table.
        /// </summary>
        public virtual DbSet<Tournament> Tournament { get; set; }

        /// <summary>
        /// configure models if needed
        /// </summary>
        /// <param name="modelBuilder">model builder</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
