namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HomeTeamOptional : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GameResults", new[] { "HomeTeamId" });
            AlterColumn("dbo.GameResults", "HomeTeamId", c => c.Int());
            CreateIndex("dbo.GameResults", "HomeTeamId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GameResults", new[] { "HomeTeamId" });
            AlterColumn("dbo.GameResults", "HomeTeamId", c => c.Int(nullable: false));
            CreateIndex("dbo.GameResults", "HomeTeamId");
        }
    }
}
