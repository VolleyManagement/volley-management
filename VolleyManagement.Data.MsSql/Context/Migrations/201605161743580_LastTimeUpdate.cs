namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Added possibility to check tournament last update time
    /// </summary>
    public partial class LastTimeUpdate : DbMigration
    {
        /// <summary>
        /// The up migration.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "LastTimeUpdated", c => c.DateTime());
        }

        /// <summary>
        /// The down migration.
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.Tournaments", "LastTimeUpdated");
        }
    }
}
