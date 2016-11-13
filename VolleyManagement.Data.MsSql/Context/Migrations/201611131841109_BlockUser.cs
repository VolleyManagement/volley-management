namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlockUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsUserBlocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsUserBlocked");
        }
    }
}
