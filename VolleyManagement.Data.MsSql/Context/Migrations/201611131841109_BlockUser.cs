namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Add boolean field is user blocked
    /// </summary>
    public partial class BlockUser : DbMigration
    {
        /// <summary>
        /// The up migration.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.Users", "IsUserBlocked", c => c.Boolean(nullable: false));
        }

        /// <summary>
        /// The down migration.
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.Users", "IsUserBlocked");
        }
    }
}
