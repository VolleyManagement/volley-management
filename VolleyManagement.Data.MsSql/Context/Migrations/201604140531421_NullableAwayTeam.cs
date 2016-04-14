namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableAwayTeam : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GameResults", new[] { "AwayTeamId" });
            AlterColumn("dbo.GameResults", "AwayTeamId", c => c.Int());
            CreateIndex("dbo.GameResults", "AwayTeamId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GameResults", new[] { "AwayTeamId" });
            AlterColumn("dbo.GameResults", "AwayTeamId", c => c.Int(nullable: false));
            CreateIndex("dbo.GameResults", "AwayTeamId");
        }
    }
}
