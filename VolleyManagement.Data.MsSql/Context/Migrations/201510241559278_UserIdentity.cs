namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Adjust User entity table to support ASP.NET Identity
    /// </summary>
    public partial class UserIdentity : DbMigration
    {
        /// <summary>
        /// Migrates up
        /// </summary>
        public override void Up()
        {
            RenameTable(name: "dbo.UserEntities", newName: "Users");
            CreateTable(
                "dbo.LoginProviders",
                c => new
                    {
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProviderKey, t.LoginProvider })
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);

            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Users", "FullName", c => c.String(maxLength: 128));
            AlterColumn("dbo.Users", "CellPhone", c => c.String(maxLength: 15));
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 128));
            DropColumn("dbo.Users", "Password");
        }

        /// <summary>
        /// Migrates down
        /// </summary>
        public override void Down()
        {
            AddColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 68));
            DropForeignKey("dbo.LoginProviders", "User_Id", "dbo.Users");
            DropIndex("dbo.LoginProviders", new[] { "User_Id" });
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 80));
            AlterColumn("dbo.Users", "CellPhone", c => c.String(maxLength: 20));
            AlterColumn("dbo.Users", "FullName", c => c.String(maxLength: 60));
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 20));
            DropTable("dbo.LoginProviders");
            RenameTable(name: "dbo.Users", newName: "UserEntities");
        }
    }
}
