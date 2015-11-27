namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddGameResults : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TournamentId = c.Int(nullable: false),
                        HomeTeamId = c.Int(nullable: false),
                        AwayTeamId = c.Int(nullable: false),
                        HomeSetsScore = c.Byte(nullable: false),
                        AwaySetsScore = c.Byte(nullable: false),
                        IsTechnicalDefeat = c.Boolean(nullable: false),
                        HomeSet1Score = c.Byte(nullable: false),
                        AwaySet1Score = c.Byte(nullable: false),
                        HomeSet2Score = c.Byte(nullable: false),
                        AwaySet2Score = c.Byte(nullable: false),
                        HomeSet3Score = c.Byte(nullable: false),
                        AwaySet3Score = c.Byte(nullable: false),
                        HomeSet4Score = c.Byte(nullable: false),
                        AwaySet4Score = c.Byte(nullable: false),
                        HomeSet5Score = c.Byte(nullable: false),
                        AwaySet5Score = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.AwayTeamId)
                .ForeignKey("dbo.Teams", t => t.HomeTeamId)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.TournamentId)
                .Index(t => t.HomeTeamId)
                .Index(t => t.AwayTeamId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.GameResults", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.GameResults", "HomeTeamId", "dbo.Teams");
            DropForeignKey("dbo.GameResults", "AwayTeamId", "dbo.Teams");
            DropIndex("dbo.GameResults", new[] { "AwayTeamId" });
            DropIndex("dbo.GameResults", new[] { "HomeTeamId" });
            DropIndex("dbo.GameResults", new[] { "TournamentId" });
            DropTable("dbo.GameResults");
        }
    }
}
