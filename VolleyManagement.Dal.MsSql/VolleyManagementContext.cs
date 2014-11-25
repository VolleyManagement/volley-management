namespace SoftServe.VolleyManagement.Dal.MsSql
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

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
