namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Added fields AdminName and UpdateDate to feedbacks table.
    /// </summary>
    public partial class AddNewFieldsToFeedback : DbMigration
    {
        /// <summary>
        /// The up migration.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.Feedbacks", "UpdateDate", c => c.DateTime(nullable: false, precision: 0, storeType: "datetime2"));
            AddColumn("dbo.Feedbacks", "AdminName", c => c.String(maxLength: 128));
        }

        /// <summary>
        /// The down migration.
        /// </summary>
        public override void Down()
        {
            DropColumn("dbo.Feedbacks", "AdminName");
            DropColumn("dbo.Feedbacks", "UpdateDate");
        }
    }
}
