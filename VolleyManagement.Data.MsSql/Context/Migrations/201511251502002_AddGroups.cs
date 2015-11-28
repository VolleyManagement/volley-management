namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Add groups for divisions
    /// </summary>
    public partial class AddGroups : DbMigration
    {
        /// <summary>
        /// Migrates up
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60),
                        DivisionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .Index(t => t.DivisionId);
        }

        /// <summary>
        /// Migrates down
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "DivisionId", "dbo.Divisions");
            DropIndex("dbo.Groups", new[] { "DivisionId" });
            DropTable("dbo.Groups");
        }
    }
}
