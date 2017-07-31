namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ChangeTournamentRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TournamentRequests", "GroupId", c => c.Int(nullable: false));
            DropColumn("dbo.TournamentRequests", "TournamentId");
        }

        public override void Down()
        {
            AddColumn("dbo.TournamentRequests", "TournamentId", c => c.Int(nullable: false));
            DropColumn("dbo.TournamentRequests", "GroupId");
        }
    }
}
