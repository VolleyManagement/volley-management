namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Drops unneeded Cascade Delete constraints
    /// </summary>
    public partial class DropExtraIndexes : DbMigration
    {
        /// <summary>
        /// Migrates up
        /// </summary>
        public override void Up()
        {
            DropForeignKey("dbo.Contributors", "ContributorTeamId", "dbo.ContributorTeams");
            DropForeignKey("dbo.Teams", "CaptainId", "dbo.Players");
            AddForeignKey("dbo.Contributors", "ContributorTeamId", "dbo.ContributorTeams", "Id");
            AddForeignKey("dbo.Teams", "CaptainId", "dbo.Players", "Id");
        }

        /// <summary>
        /// Migrates down
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.Teams", "CaptainId", "dbo.Players");
            DropForeignKey("dbo.Contributors", "ContributorTeamId", "dbo.ContributorTeams");
            AddForeignKey("dbo.Teams", "CaptainId", "dbo.Players", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Contributors", "ContributorTeamId", "dbo.ContributorTeams", "Id", cascadeDelete: true);
        }
    }
}
