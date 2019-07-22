namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System.Data.Entity.Migrations;

    /// <summary>
    /// Initial DB migration
    /// </summary>
    public partial class InitialCreate : DbMigration
    {
        /// <summary>
        /// Migrates schema up
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.Contributors",
                c =>
                new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 30),
                    ContributorTeamId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContributorTeams", t => t.ContributorTeamId);

            CreateTable(
                "dbo.ContributorTeams",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 20),
                    CourseDirection = c.String(nullable: false, maxLength: 20),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Players",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false, maxLength: 60),
                    LastName = c.String(nullable: false, maxLength: 60),
                    BirthYear = c.Short(),
                    Height = c.Short(),
                    Weight = c.Short(),
                    TeamId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.TeamId);

            CreateTable(
                "dbo.Teams",
                c =>
                new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 60),
                    Coach = c.String(maxLength: 60),
                    Achievements = c.String(maxLength: 4000),
                    CaptainId = c.Int(nullable: false),
                }).PrimaryKey(t => t.Id)
                    .ForeignKey("dbo.Players", t => t.CaptainId);

            CreateTable(
                "dbo.Tournaments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 60),
                    SchemeCode = c.Byte(nullable: false),
                    SeasonOffset = c.Byte(nullable: false),
                    Description = c.String(maxLength: 300),
                    RegulationsLink = c.String(maxLength: 255),
                    GamesStart = c.DateTime(nullable: false, storeType: "date"),
                    GamesEnd = c.DateTime(nullable: false, storeType: "date"),
                    TransferStart = c.DateTime(nullable: false, storeType: "date"),
                    TransferEnd = c.DateTime(nullable: false, storeType: "date"),
                    ApplyingPeriodStart = c.DateTime(nullable: false, storeType: "date"),
                    ApplyingPeriodEnd = c.DateTime(nullable: false, storeType: "date"),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.UserEntities",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserName = c.String(nullable: false, maxLength: 20),
                    Password = c.String(nullable: false, maxLength: 68),
                    FullName = c.String(maxLength: 60),
                    CellPhone = c.String(maxLength: 20),
                    Email = c.String(nullable: false, maxLength: 80),
                })
                .PrimaryKey(t => t.Id);
        }

        /// <summary>
        /// Migrates schema down
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.Players", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Teams", "CaptainId", "dbo.Players");
            DropForeignKey("dbo.Contributors", "ContributorTeamId", "dbo.ContributorTeams");
            DropIndex("dbo.Players", new[] { "TeamId" });
            DropTable("dbo.UserEntities");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Teams");
            DropTable("dbo.Players");
            DropTable("dbo.ContributorTeams");
            DropTable("dbo.Contributors");
        }
    }
}
