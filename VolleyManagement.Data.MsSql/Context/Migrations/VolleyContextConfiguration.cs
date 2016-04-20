namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System; 
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    using VolleyManagement.Data.MsSql.Entities;

    /// <summary>
    /// The volley context configuration.
    /// </summary>
    internal sealed class VolleyContextConfiguration : DbMigrationsConfiguration<VolleyManagementEntities>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyContextConfiguration"/> class.
        /// </summary>
        public VolleyContextConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Context\Migrations";
            ContextKey = "VolleyManagement.Data.MsSql.Context.VolleyManagementEntities";
        }

        /// <summary>
        /// This method will be called after migrating to the latest version.
        /// </summary>
        /// <param name="context"> Volley Management context</param>
        protected override void Seed(VolleyManagementEntities context)
        {
            var defaultRoles = new List<RoleEntity>();

            defaultRoles.Add(CreateRole("Administrator"));
            defaultRoles.Add(CreateRole("TournamentAdministrator"));
            defaultRoles.Add(CreateRole("User"));

            context.Roles.AddOrUpdate(r => r.Name, defaultRoles.ToArray());

            var contributorTeams = new[]
            {
                ContributorsProMan(),
                Contributors042Net(),
                Contributors052Net(),
                Contributors061Net(),
                Contributors064Net(),
                Contributors064Atqc(),
                Contributors065Ui(),
                Contributors070Ui(),
                Contributors072Net(),
                Contributors076Atqc(),
                Contributors085Net()
            };

            context.ContributorTeams.AddOrUpdate(s => s.Name, contributorTeams);

            PlayerEntity[] players;
            TeamEntity[] teams;
            TournamentEntity[] tournaments;
            GameResultEntity[] games;

            GenerateEntities(out players, out teams, out tournaments, out games);

            context.Players.AddOrUpdate(p => new { p.FirstName, p.LastName }, players);
            context.Teams.AddOrUpdate(t => t.Name, teams);
            context.Tournaments.AddOrUpdate(t => t.Name, tournaments);

            context.SaveChanges();

            context.GameResults.AddOrUpdate(g => g.Id, games);

            context.SaveChanges(); 
        } 

        private static void GenerateEntities(
            out PlayerEntity[] players,
            out TeamEntity[] teams,
            out TournamentEntity[] tournaments,
            out GameResultEntity[] games)
        {
            #region Seed players
            PlayerEntity player1 = new PlayerEntity
            {
                BirthYear = 1970,
                FirstName = "John",
                LastName = "Smith",
                Height = 180,
                Weight = 70
            };
            PlayerEntity player2 = new PlayerEntity
            {
                BirthYear = 1986,
                FirstName = "Lex",
                LastName = "Luthor",
                Height = 175,
                Weight = 75
            };
            PlayerEntity player3 = new PlayerEntity
            {
                BirthYear = 1977,
                FirstName = "Darth",
                LastName = "Vader",
                Height = 181,
                Weight = 80
            };
            PlayerEntity player4 = new PlayerEntity
            {
                BirthYear = 1988,
                FirstName = "Kylo",
                LastName = "Ren",
                Height = 176,
                Weight = 80
            };
            PlayerEntity player5 = new PlayerEntity
            {
                BirthYear = 85,
                FirstName = "Han",
                LastName = "Solo",
                Height = 180,
                Weight = 75
            };
            PlayerEntity player6 = new PlayerEntity
            {
                BirthYear = 1968,
                FirstName = "Luke",
                LastName = "Skywalker",
                Height = 165,
                Weight = 6
            };
            PlayerEntity player7 = new PlayerEntity
            {
                BirthYear = 1990,
                FirstName = "Obivan",
                LastName = "Kenobi",
                Height = 190,
                Weight = 120
            };
            PlayerEntity player8 = new PlayerEntity
            {
                BirthYear = 2005,
                FirstName = "Mighty",
                LastName = "Thor",
                Height = 250,
                Weight = 400
            };
            PlayerEntity player9 = new PlayerEntity
            {
                BirthYear = 1945,
                FirstName = "Tony",
                LastName = "Stark",
                Height = 150,
                Weight = 50
            };
            PlayerEntity player10 = new PlayerEntity
            {
                BirthYear = 1920,
                FirstName = "Hulk",
                LastName = "Incredible",
                Height = 250,
                Weight = 500
            };
            PlayerEntity player11 = new PlayerEntity
            {
                BirthYear = 1955,
                FirstName = "Man",
                LastName = "Ant",
                Height = 10,
                Weight = 10
            };
            PlayerEntity player12 = new PlayerEntity
            {
                BirthYear = 1900,
                FirstName = "Clark",
                LastName = "Kent",
                Height = 200,
                Weight = 300
            };

            players = new PlayerEntity[]
            {
                player1, 
                player2,
                player3,
                player4,
                player5,
                player6,
                player7,
                player8,
                player9,
                player10,
                player11,
                player12
            };

            #endregion

            #region Seed teams
            TeamEntity team1 = new TeamEntity
            {
                Players = new List<PlayerEntity> { player2 },
                CaptainId = 1,
                Name = "First Order",
                Coach = "Coach1"
            };
            TeamEntity team2 = new TeamEntity
            {
                Players = new List<PlayerEntity> { player4 },
                CaptainId = 2,
                Name = "Empire",
                Coach = "Coach2",
            };
            TeamEntity team3 = new TeamEntity
            {
                Players = new List<PlayerEntity> { player6, player7 },
                CaptainId = 3,
                Name = "Rebelion",
                Coach = "Coach3"
            };
            TeamEntity team4 = new TeamEntity
            {
                Players = new List<PlayerEntity> { player9 },
                CaptainId = 4,
                Name = "Avengers",
                Coach = "Coach4"
            };
            TeamEntity team5 = new TeamEntity
            {
                Players = new List<PlayerEntity> { player11 },
                CaptainId = 5,
                Name = "Cap",
                Coach = "Coach5"
            };
            TeamEntity team6 = new TeamEntity
            {
                Players = new List<PlayerEntity> { player12 },
                CaptainId = 6,
                Name = "DC",
                Coach = "Coach6"
            };

            teams = new TeamEntity[]
                {
                    team1, 
                    team2, 
                    team3, 
                    team4, 
                    team5, 
                    team6 
                };
            #endregion

            #region Seed tournaments
            // Past torunament, scheme 1
            TournamentEntity tour1 = new TournamentEntity
            {
                Name = "Clone Wars",
                ApplyingPeriodStart = new DateTime(2015, 06, 02),
                ApplyingPeriodEnd = new DateTime(2015, 09, 02),
                GamesStart = new DateTime(2015, 09, 03),
                GamesEnd = new DateTime(2015, 09, 29),
                TransferStart = new DateTime(2015, 09, 04),
                TransferEnd = new DateTime(2015, 09, 28),
                Scheme = 1,
                Season = 115,
                Divisions = new List<DivisionEntity>()
                {
                    new DivisionEntity()
                    {
                        Name = "Division 1",
                        Groups = new List<GroupEntity>()
                        {
                            new GroupEntity()
                            {   
                                Name = "Group 1"
                            }
                        }
                    }
                },
                Teams = new List<TeamEntity>() { team1, team2, team3, team4, team5, team6 }
            };

            // Current tournament, shceme 1
            TournamentEntity tour2 = new TournamentEntity
            {
                Name = "New Hope",
                ApplyingPeriodStart = DateTime.Now.AddMonths(-1),
                ApplyingPeriodEnd = DateTime.Now.AddDays(-2),
                GamesStart = DateTime.Now.AddDays(-1),
                GamesEnd = DateTime.Now.AddDays(5),
                TransferStart = DateTime.Now,
                TransferEnd = DateTime.Now.AddDays(4),
                Scheme = 1,
                Season = Convert.ToByte(DateTime.Now.Year - 1900),
                Divisions = new List<DivisionEntity>()
                {
                    new DivisionEntity()
                    {
                        Name = "Division 2",
                        Groups = new List<GroupEntity>()
                        {
                            new GroupEntity()
                            {   
                                Name = "Group 2"
                            }
                        }
                    }
                },
                Teams = new List<TeamEntity>() { team1, team2, team3, team5, team6 }
            };
            // Future tournament scheme 1
            TournamentEntity tour3 = new TournamentEntity
            {
                Name = "Force Awakens",
                ApplyingPeriodStart = DateTime.Now.AddMonths(1),
                ApplyingPeriodEnd = DateTime.Now.AddMonths(2),
                GamesStart = DateTime.Now.AddMonths(2).AddDays(2),
                GamesEnd = DateTime.Now.AddMonths(2).AddDays(10),
                TransferStart = DateTime.Now.AddMonths(2).AddDays(2),
                TransferEnd = DateTime.Now.AddMonths(2).AddDays(7),
                Scheme = 1,
                Season = Convert.ToByte(DateTime.Now.AddYears(1).Year - 1900),
                Divisions = new List<DivisionEntity>()
                {
                    new DivisionEntity()
                    {
                        Name = "Division 3",
                        Groups = new List<GroupEntity>()
                        {
                            new GroupEntity()
                            {   
                                Name = "Group 3"
                            }
                        }
                    }
                },
                Teams = new List<TeamEntity>() { team1, team2, team3, team4, team5, team6 }
            };

            // Past tournament, scheme 2
            TournamentEntity tour4 = new TournamentEntity
            {
                Name = "Hunger Games",
                ApplyingPeriodStart = new DateTime(2012, 01, 02),
                ApplyingPeriodEnd = new DateTime(2012, 05, 05),
                GamesStart = new DateTime(2012, 05, 20),
                GamesEnd = new DateTime(2012, 06, 01),
                TransferStart = new DateTime(2012, 05, 20),
                TransferEnd = new DateTime(2012, 06, 01),
                Scheme = 2,
                Season = 112,
                Divisions = new List<DivisionEntity>()
                {
                    new DivisionEntity()
                    {
                        Name = "Division 4",
                        Groups = new List<GroupEntity>()
                        {
                            new GroupEntity()
                            {   
                                Name = "Group 4"
                            }
                        }
                    }
                },
                Teams = new List<TeamEntity>() { 
                    team1,
                    team2,
                    team3}
            };

            // Current tournament, scheme 2
            TournamentEntity tour5 = new TournamentEntity
            {
                Name = "Epic tour",
                ApplyingPeriodStart = DateTime.Now.AddMonths(-2),
                ApplyingPeriodEnd = DateTime.Now.AddDays(-3),
                GamesStart = DateTime.Now.AddDays(-2),
                GamesEnd = DateTime.Now.AddDays(20),
                TransferStart = DateTime.Now.AddDays(2),
                TransferEnd = DateTime.Now.AddDays(19),
                Scheme = 2,
                Season = Convert.ToByte(DateTime.Now.Year - 1900),
                Divisions = new List<DivisionEntity>()
                {
                    new DivisionEntity()
                    {
                        Name = "Division 5",
                        Groups = new List<GroupEntity>()
                        {
                            new GroupEntity()
                            {   
                                Name = "Group 5"
                            }
                        }
                    }
                },
                Teams = new List<TeamEntity>() { team1, team2, team3, team4, team5 }
            };
            // Future tournament, scheme 2
            TournamentEntity tour6 = new TournamentEntity
            {
                Name = "Empire Strikes Back",
                ApplyingPeriodStart = DateTime.Now.AddMonths(1),
                ApplyingPeriodEnd = DateTime.Now.AddMonths(2).AddDays(15),
                GamesStart = DateTime.Now.AddMonths(2).AddDays(16),
                GamesEnd = DateTime.Now.AddMonths(2).AddDays(7),
                TransferEnd = null,
                TransferStart = null,
                Scheme = 2,
                Season = Convert.ToByte(DateTime.Now.Year - 1900),
                Divisions = new List<DivisionEntity>()
                {
                    new DivisionEntity()
                    {
                        Name = "Division 6",
                        Groups = new List<GroupEntity>()
                        {
                            new GroupEntity()
                            {   
                                Name = "Group 6"
                            }
                        }
                    }
                },
                Teams = new List<TeamEntity>() { team6, team5, team3, team4, team1 }
            };

            tournaments = new TournamentEntity[]
            { 
                tour1,
                tour2,
                tour3,
                tour4,
                tour5,
                tour6
            };
            #endregion

            games = GenerateGamesFromTournaments(tournaments); 
        }

        private static GameResultEntity[] GenerateGamesFromTournaments(TournamentEntity[] tours)
        {
            List<GameResultEntity> games = new List<GameResultEntity>();
            int gameId = 0;
            for (int i = 0; i < tours.Length; i++)
            {
                if (games.Count > 0)
                {
                    gameId = games[games.Count - 1].Id;
                }
                games.AddRange(GenerateGames(tours[i], i+1, ++gameId)); 
            }

            return games.ToArray(); 
        }

        private static List<GameResultEntity> GenerateGames(TournamentEntity tour, int tourId, int gameId)
        {
            List<GameResultEntity> games = new List<GameResultEntity>(); 

            int teamsCount = tour.Teams.Count;
            int roundsNumber = teamsCount % 2 == 0 ? teamsCount - 1 : teamsCount;
            int gamesInRound = roundsNumber % 2 == 0 ? roundsNumber / 2 : roundsNumber / 2 + 1;

            // Initial round 
            byte roundIter = 1;
            int[] homeTeamIds = new int[gamesInRound];
            int[] awayTeamIds = new int[gamesInRound];
            int[] tempHome = new int[gamesInRound];
            int[] tempAway = new int[gamesInRound];
            for (int i = 0, j = 1; i < gamesInRound; i++, j++)
            {
                homeTeamIds[i] = j;
                awayTeamIds[i] = i == teamsCount - 1 && teamsCount % 2 != 0 ? 0 : ++j;

                tempHome[i] = homeTeamIds[i];
                tempAway[i] = awayTeamIds[i]; 
            }

            // round robin swap 
            do
            { 
                for (int i = 0; i < gamesInRound; i++)
                {
                    int currentHomeTeamId = 0;
                    int? currentAwayTeamId = 0;

                    if (homeTeamIds[0] == 0)
                    {
                        currentAwayTeamId = null;
                        currentHomeTeamId = awayTeamIds[i];
                    }
                    else if (awayTeamIds[i] == 0)
                    {
                        currentAwayTeamId = null;
                        currentHomeTeamId = homeTeamIds[i];
                    }
                    else
                    {
                        currentAwayTeamId = awayTeamIds[i];
                        currentHomeTeamId = homeTeamIds[i];
                    }

                    games.Add(new GameResultEntity
                    {
                        Id = gameId++,
                        TournamentId = tourId,
                        HomeTeamId = currentHomeTeamId,
                        AwayTeamId = currentAwayTeamId,
                        StartTime = tour.GamesStart.AddDays(1),
                        RoundNumber = (byte)(roundIter)
                    });

                   if (i == 0)
                   {
                       awayTeamIds[i] = tempHome[i + 1];
                   }
                   else if (i == gamesInRound - 1)
                   {
                       homeTeamIds[i] = tempAway[i];
                       awayTeamIds[i] = tempAway[i - 1];
                   }
                   else
                   {
                       awayTeamIds[i] = tempAway[i - 1];
                       homeTeamIds[i] = tempHome[i + 1];
                   } 
                }

                for (int i = 0; i < homeTeamIds.Length; i++)
                {
                    tempHome[i] = homeTeamIds[i];
                    tempAway[i] = awayTeamIds[i]; 
                }
            
                roundIter++; 
            }
            while (roundIter != roundsNumber+1);

            // Free day games may occur in a wrong order 

            if (tour.Scheme == 2)
            {
                games = GenerateGamesDuplicateInSchemeTwo(games, roundsNumber); 
            }

            if (tour.GamesStart < DateTime.Now)
            {
                SetGameScores(games); 
            }

            return games; 
        } 
         

        private static void SetGameScores(List<GameResultEntity> games)
        {
            // Only for past games  
           
        }

        private static List<GameResultEntity> GenerateGamesDuplicateInSchemeTwo(List<GameResultEntity> games, int roundNumber)
        {
            List<GameResultEntity> duplicates = new List<GameResultEntity>();

            int gameId = games[games.Count - 1].Id; 
            foreach (GameResultEntity game in games)
            {
                int homeTeamId = game.HomeTeamId;
                int? awayTeamId = game.AwayTeamId; 

                if (awayTeamId != null)
                {
                    int temp = homeTeamId;
                    homeTeamId = awayTeamId.Value;
                    awayTeamId = temp; 
                }

                duplicates.Add(new GameResultEntity
                    {
                        Id = ++gameId,
                        TournamentId = game.TournamentId,
                        StartTime = game.StartTime,
                        HomeTeamId = homeTeamId,
                        AwayTeamId = awayTeamId,
                        RoundNumber = Convert.ToByte(game.RoundNumber + roundNumber)
                    }); 
            }

            games.AddRange(duplicates); 

            return games; 
        }

        private static RoleEntity CreateRole(string name)
        {
            return new RoleEntity { Name = name };
        }

        private static ContributorTeamEntity ContributorsProMan()
        {
            ContributorTeamEntity contributors = new ContributorTeamEntity()
                {
                    Name = "Project Management",
                    CourseDirection = "All",
                    Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Sergii Diachenko" },
                        new ContributorEntity { Name = "Iurii Osipchuk" },
                        new ContributorEntity { Name = "Oleksii Konovalenko" },
                        new ContributorEntity { Name = "Pavlo Ignatenko" },
                        new ContributorEntity { Name = "Dmytro Petin" },
                        new ContributorEntity { Name = "Oleg Shvets" }
                     }
                };
            return contributors;
        }

        private static ContributorTeamEntity Contributors042Net()
        {
            ContributorTeamEntity contributors = new ContributorTeamEntity()
            {
                Name = "Dp-042 .NET",
                CourseDirection = ".NET",
                Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Igor Leta" },
                        new ContributorEntity { Name = "Dmytro Balayev" },
                        new ContributorEntity { Name = "Ielyzaveta Kalinchuk" },
                        new ContributorEntity { Name = "Oleg Petrushov" },
                        new ContributorEntity { Name = "Evgenij Kozhan" }
                    }
            };
            return contributors;
        }

        private static ContributorTeamEntity Contributors052Net()
        {
            ContributorTeamEntity contributors = new ContributorTeamEntity()
            {
                Name = "Dp-052 .NET",
                CourseDirection = ".NET",
                Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Julia Bukova" },
                        new ContributorEntity { Name = "Alexandr Marchotko" },
                        new ContributorEntity { Name = "Pavlo Dragoetskij" },
                        new ContributorEntity { Name = "Anastacia Necheporenko" },
                        new ContributorEntity { Name = "Egor Meshcheriakov" },
                        new ContributorEntity { Name = "Pavel Goncharenko" }
                    }
            };
            return contributors;
        }

        private static ContributorTeamEntity Contributors061Net()
        {
            ContributorTeamEntity contributors = new ContributorTeamEntity()
            {
                Name = "Dp-061 .NET",
                CourseDirection = ".NET",
                Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Nikita Gordienko" },
                        new ContributorEntity { Name = "Marina Gaponova" },
                        new ContributorEntity { Name = "Sofia Shaposhnik" },
                        new ContributorEntity { Name = "Pavlo Pokhylenko" },
                        new ContributorEntity { Name = "Sergii Kuzmin" }
                    }
            };
            return contributors;
        }

        private static ContributorTeamEntity Contributors064Net()
        {
            ContributorTeamEntity contributors = new ContributorTeamEntity()
            {
                Name = "Dp-064 .NET",
                CourseDirection = ".NET",
                Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Andrii Zelyk" },
                        new ContributorEntity { Name = "Viktoriia Ryndina" },
                        new ContributorEntity { Name = "Alex Lapin" },
                        new ContributorEntity { Name = "Dmytro Chernyshov" },
                        new ContributorEntity { Name = "Maria Kochetkova" },
                        new ContributorEntity { Name = "Oleh Vovkodav" },
                        new ContributorEntity { Name = "Alexandr Maha" }
                    }
            };
            return contributors;
        }

        private static ContributorTeamEntity Contributors064Atqc()
        {
            ContributorTeamEntity contributors = new ContributorTeamEntity()
            {
                Name = "Dp-064 ATQC",
                CourseDirection = "ATQC",
                Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Mykola Kolisnyk" },
                        new ContributorEntity { Name = "Maria Tsymbaliuk" },
                        new ContributorEntity { Name = "Ipatov Sergii" },
                        new ContributorEntity { Name = "Dmitriy Otrashevskiy" },
                        new ContributorEntity { Name = "Ruslan Borisenko" },
                        new ContributorEntity { Name = "Zavizion Stanislav" },
                        new ContributorEntity { Name = "Oleksandr Rudyi" },
                        new ContributorEntity { Name = "Sergey Bondarenko" }
                    }
            };
            return contributors;
        }

        private static ContributorTeamEntity Contributors065Ui()
        {
            ContributorTeamEntity contributors = new ContributorTeamEntity()
            {
                Name = "Dp-065 UI",
                CourseDirection = "UI",
                Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Denis Chernysh" },
                        new ContributorEntity { Name = "Oleh Man'kov" },
                        new ContributorEntity { Name = "Kateryna Nikolaieva" },
                        new ContributorEntity { Name = "Stanislav Makhnyts'kyi" },
                        new ContributorEntity { Name = "Yevhen Alf'orov" }
                    }
            };
            return contributors;
        }

        private static ContributorTeamEntity Contributors070Ui()
        {
            ContributorTeamEntity contributors = new ContributorTeamEntity()
            {
                Name = "Dp-070 UI",
                CourseDirection = "UI",
                Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Dykhov Egor" },
                        new ContributorEntity { Name = "Shafarenko Ol'ha" },
                        new ContributorEntity { Name = "Ovcharenko Ekateryna" },
                        new ContributorEntity { Name = "Korostienko Daniil" }
                    }
            };
            return contributors;
        }

        private static ContributorTeamEntity Contributors072Net()
        {
            ContributorTeamEntity contributors = new ContributorTeamEntity()
            {
                Name = "Dp-072 .NET",
                CourseDirection = ".NET",
                Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Denis Rudenko" },
                        new ContributorEntity { Name = "Mykyta Kurchenkov" },
                        new ContributorEntity { Name = "Elyzaveta Rudakova" },
                        new ContributorEntity { Name = "Mihail Zazharskiy" },
                        new ContributorEntity { Name = "Dmitriy Ryndin" },
                        new ContributorEntity { Name = "Dmitriy Shapoval" }
                    }
            };
            return contributors;
        }

        private static ContributorTeamEntity Contributors076Atqc()
        {
            ContributorTeamEntity contributors = new ContributorTeamEntity()
            {
                Name = "Dp-076 ATQC",
                CourseDirection = "ATQC",
                Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Alla Prykhodchenko" },
                        new ContributorEntity { Name = "Andriy Lantukh" },
                        new ContributorEntity { Name = "Oleksandr Zaitsev" },
                        new ContributorEntity { Name = "Artem Pychenko" },
                        new ContributorEntity { Name = "Dmytro Maslov" },
                        new ContributorEntity { Name = "Artem  Pozdeev" }
                    }
            };
            return contributors;
        }

        private static ContributorTeamEntity Contributors085Net()
        {
            ContributorTeamEntity contributors = new ContributorTeamEntity()
            {
                Name = "Dp-085 .NET",
                CourseDirection = ".NET",
                Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Ievgen Oparyshev" },
                        new ContributorEntity { Name = "Mariia Shvets" },
                        new ContributorEntity { Name = "Eugene Gerbut" },
                        new ContributorEntity { Name = "Pavel Novichkhin" },
                        new ContributorEntity { Name = "Artem Kolisnyk" },
                        new ContributorEntity { Name = "Oleksii Khloptsev" }
                    }
            };
            return contributors;
        }
    }
}
