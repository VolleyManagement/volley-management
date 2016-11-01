namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Makes it possible to set feedback to null
    /// </summary>
    public partial class AddFeedbacks : DbMigration
    {
        /// <summary>
        /// Migrates feedback up
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.Feedbacks",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UsersEmail = c.String(nullable: false, maxLength: 80),
                    Content = c.String(nullable: false, maxLength: 5000),
                    Date = c.DateTime(nullable: false, precision: 0, storeType: "datetime2"),
                    StatusCode = c.Byte(nullable: false),
                })
                .PrimaryKey(t => t.Id);
        }

        /// <summary>
        /// Migrates feedback down
        /// </summary>
        public override void Down()
        {
            DropTable("dbo.Feedbacks");
        }
    }
}
