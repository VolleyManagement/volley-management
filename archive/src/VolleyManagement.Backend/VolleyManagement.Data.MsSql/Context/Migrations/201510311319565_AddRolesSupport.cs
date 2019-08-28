namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Add support for user roles
    /// </summary>
    public partial class AddRolesSupport : DbMigration
    {
        /// <summary>
        /// The up migration.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.UserToRoleMap",
                c => new
                {
                    UserId = c.Int(nullable: false),
                    RoleId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
        }

        /// <summary>
        /// The down migration.
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.UserToRoleMap", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserToRoleMap", "UserId", "dbo.Users");
            DropIndex("dbo.UserToRoleMap", new[] { "RoleId" });
            DropIndex("dbo.UserToRoleMap", new[] { "UserId" });
            DropTable("dbo.UserToRoleMap");
            DropTable("dbo.Roles");
        }
    }
}
