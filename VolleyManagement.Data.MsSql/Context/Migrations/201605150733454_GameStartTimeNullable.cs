namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Makes it possible to set game start time to null
    /// </summary>
    public partial class GameStartTimeNullable : DbMigration
    {
        /// <summary>
        /// Migrates schema up
        /// </summary>
        public override void Up()
        {
            AlterColumn("dbo.GameResults", "StartTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }

        /// <summary>
        /// Migrates schema down
        /// </summary>
        public override void Down()
        {
            AlterColumn("dbo.GameResults", "StartTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
