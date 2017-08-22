namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddCascadeDeleteForTournaments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.Divisions", "TournamentId", "dbo.Tournaments");
            AddForeignKey("dbo.Groups", "DivisionId", "dbo.Divisions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Divisions", "TournamentId", "dbo.Tournaments", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Divisions", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.Groups", "DivisionId", "dbo.Divisions");
            AddForeignKey("dbo.Divisions", "TournamentId", "dbo.Tournaments", "Id");
            AddForeignKey("dbo.Groups", "DivisionId", "dbo.Divisions", "Id");
        }
    }
}
