namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlayersToTeam : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Teams", "CaptainId", "dbo.Players");
            DropForeignKey("dbo.Players", "TeamEntity_Id", "dbo.Teams");
            DropIndex("dbo.Teams", new[] { "CaptainId" });
            DropIndex("dbo.Players", new[] { "TeamEntity_Id" });
            DropColumn("dbo.Players", "TeamEntity_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "TeamEntity_Id", c => c.Int());
            CreateIndex("dbo.Players", "TeamEntity_Id");
            CreateIndex("dbo.Teams", "CaptainId");
            AddForeignKey("dbo.Players", "TeamEntity_Id", "dbo.Teams", "Id");
            AddForeignKey("dbo.Teams", "CaptainId", "dbo.Players", "Id", cascadeDelete: true);
        }
    }
}
