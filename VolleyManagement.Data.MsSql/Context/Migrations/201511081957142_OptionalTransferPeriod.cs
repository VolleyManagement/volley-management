namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Adjust Tournament entity table to support optional transfer period
    /// </summary>
    public partial class OptionalTransferPeriod : DbMigration
    {
        /// <summary>
        /// Migrates up
        /// </summary>
        public override void Up()
        {
            AlterColumn("dbo.Tournaments", "TransferStart", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Tournaments", "TransferEnd", c => c.DateTime(storeType: "date"));
        }

        /// <summary>
        /// Migrates down
        /// </summary>
        public override void Down()
        {
            AlterColumn("dbo.Tournaments", "TransferEnd", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Tournaments", "TransferStart", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
