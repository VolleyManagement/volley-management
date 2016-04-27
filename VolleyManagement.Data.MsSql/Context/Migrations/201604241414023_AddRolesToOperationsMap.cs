namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Add support for authorization operations
    /// </summary>
    public partial class AddRolesToOperationsMap : DbMigration
    {
        /// <summary>
        /// The up migration.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.RolesToOperationsMap",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        OperationId = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }

        /// <summary>
        /// The down migration.
        /// </summary>
        public override void Down()
        {
            DropTable("dbo.RolesToOperationsMap");
        }
    }
}
