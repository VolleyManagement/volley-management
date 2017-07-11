namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddTeamGroupRelations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupTeam",
                c => new
                    {
                        GroupId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroupId, t.TeamId })
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.GroupId)
                .Index(t => t.TeamId);

            DropTable("dbo.TournamentTeam");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.TournamentTeam",
                c => new
                    {
                        TournamentId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TournamentId, t.TeamId });

            DropTable("dbo.GroupTeam");
            CreateIndex("dbo.TournamentTeam", "TeamId");
            CreateIndex("dbo.TournamentTeam", "TournamentId");
            AddForeignKey("dbo.TournamentTeam", "TeamId", "dbo.Teams", "Id");
            AddForeignKey("dbo.TournamentTeam", "TournamentId", "dbo.Tournaments", "Id");
        }
    }
}
