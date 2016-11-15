namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Added Requests table
    /// </summary>
    public partial class AddRequestsTable : DbMigration
    {
        /// <summary>
        /// The up migration.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                    })
                   .PrimaryKey(t => t.Id);
        }

        /// <summary>
        /// The down migration.
        /// </summary>
        public override void Down()
        {
            DropTable("dbo.Requests");
        }
    }
}
