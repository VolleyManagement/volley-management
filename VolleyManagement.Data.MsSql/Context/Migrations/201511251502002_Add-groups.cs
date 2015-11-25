namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addgroups : DbMigration
    {
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
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "DivisionId", "dbo.Divisions");
            DropIndex("dbo.Groups", new[] { "DivisionId" });
            DropTable("dbo.Groups");
        }
    }
}
