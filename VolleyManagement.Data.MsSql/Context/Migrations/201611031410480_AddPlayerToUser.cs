namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlayerToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PlayerId", c => c.Int());
            CreateIndex("dbo.Users", "PlayerId");
            AddForeignKey("dbo.Users", "PlayerId", "dbo.Players", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "PlayerId", "dbo.Players");
            DropIndex("dbo.Users", new[] { "PlayerId" });
            DropColumn("dbo.Users", "PlayerId");
        }
    }
}
