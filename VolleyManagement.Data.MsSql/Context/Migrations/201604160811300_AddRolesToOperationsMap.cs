namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRolesToOperationsMap : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RolesToOperationsMap",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        OperationId = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RolesToOperationsMap");
        }
    }
}
