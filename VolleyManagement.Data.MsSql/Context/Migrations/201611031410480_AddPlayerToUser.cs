namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Added field PlayerId to User table
    /// </summary>
    public partial class AddPlayerToUser : DbMigration
    {
        /// <summary>
        /// The up migration.
        /// </summary>
        public override void Up()
        {
            AddColumn("dbo.Users", "PlayerId", c => c.Int());
            CreateIndex("dbo.Users", "PlayerId");
            AddForeignKey("dbo.Users", "PlayerId", "dbo.Players", "Id");
        }

        /// <summary>
        /// The down migration.
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.Users", "PlayerId", "dbo.Players");
            DropIndex("dbo.Users", new[] { "PlayerId" });
            DropColumn("dbo.Users", "PlayerId");
        }
    }
}
