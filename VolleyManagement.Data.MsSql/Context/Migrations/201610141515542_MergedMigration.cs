namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Added merged version for Add_playoff_scheme
    /// </summary>
    public partial class MergedMigration : DbMigration
    {
        /// <summary>
        /// The up migration.
        /// </summary>
        public override void Up()
        {
            DropIndex("dbo.GameResults", new[] { "HomeTeamId" });
            AlterColumn("dbo.GameResults", "HomeTeamId", c => c.Int());
            AlterColumn("dbo.GameResults", "StartTime", c => c.DateTime(precision: 0, storeType: "datetime2"));
            CreateIndex("dbo.GameResults", "HomeTeamId");
        }

        /// <summary>
        /// The down migration.
        /// </summary>
        public override void Down()
        {
            DropIndex("dbo.GameResults", new[] { "HomeTeamId" });
            AlterColumn("dbo.GameResults", "StartTime", c => c.DateTime(nullable: false, precision: 0, storeType: "datetime2"));
            AlterColumn("dbo.GameResults", "HomeTeamId", c => c.Int(nullable: false));
            CreateIndex("dbo.GameResults", "HomeTeamId");
        }
    }
}
