namespace VolleyManagement.Dal.MsSql
{
    using System.Data.Entity;

    internal partial class VolleyManagementContext : DbContext
    {
        public VolleyManagementContext()
            : base("name=VolleyManagementContext")
        {
        }

        public virtual DbSet<Tournament> Tournament { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
