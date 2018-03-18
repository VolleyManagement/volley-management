namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTeamGroupRelations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupTeam",
                c => new
                {
                    GroupId = c.Int(nullable: false),
                    TeamId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.GroupId, t.TeamId })
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.GroupId)
                .Index(t => t.TeamId);

            var query_Group_Temp = @"(SELECT MIN(Groups.Id) AS GroupId, 
                                                Tournaments.Id AS TournId 
                                        FROM Tournaments  
                                            INNER JOIN Divisions ON Tournaments.Id = Divisions.TournamentId  
                                            INNER JOIN Groups ON Divisions.Id = Groups.DivisionId 
                                        GROUP BY Tournaments.Id)
                                        AS Group_Temp ";

            var query_TournamentTeam_To_GroupTeam = @"INSERT INTO GroupTeam (GroupId, TeamId) 
                                                        SELECT Group_Temp.GroupId AS GroupId, 
                                                               Teams.Id AS TeamId 
                                                        FROM Groups 
                                                             INNER JOIN " + query_Group_Temp + @" ON Groups.Id = Group_Temp.GroupId
                                                             INNER JOIN TournamentTeam ON Group_Temp.TournId = TournamentTeam.TournamentId 
                                                             INNER JOIN Teams ON Teams.Id = TournamentTeam.TeamId 
                                                        WHERE Group_Temp.TournId = TournamentTeam.TournamentId ";

            Sql(query_TournamentTeam_To_GroupTeam);

            DropTable("dbo.TournamentTeam");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.TournamentTeam",
                c => new
                {
                    TournamentId = c.Int(nullable: false),
                    TeamId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.TournamentId, t.TeamId });
            CreateIndex("dbo.TournamentTeam", "TeamId");
            CreateIndex("dbo.TournamentTeam", "TournamentId");
            AddForeignKey("dbo.TournamentTeam", "TeamId", "dbo.Teams", "Id");
            AddForeignKey("dbo.TournamentTeam", "TournamentId", "dbo.Tournaments", "Id");

            var query_GroupTeam_To_TournamentTeam = @"INSERT INTO TournamentTeam(TeamId, TournamentId) 
                                                         Select  Teams.Id, 
                                                               Tournaments.Id 
                                                         From Tournaments 
                                                                Inner Join Divisions On Divisions.TournamentId = Tournaments.Id 
                                                                Inner Join Groups On Groups.DivisionId = Divisions.Id  
                                                                Inner Join GroupTeam On GroupTeam.GroupId = Groups.Id 
                                                                Inner Join Teams On Teams.Id = GroupTeam.TeamId ";

            Sql(query_GroupTeam_To_TournamentTeam);

            DropTable("dbo.GroupTeam");
        }
    }
}
