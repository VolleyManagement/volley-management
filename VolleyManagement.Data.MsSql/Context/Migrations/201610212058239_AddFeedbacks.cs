namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFeedbacks : DbMigration
    {
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
                        UpdateDate = c.DateTime(nullable: false, precision: 0, storeType: "datetime2"),
                        AdminName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Feedbacks");
        }
    }
}
