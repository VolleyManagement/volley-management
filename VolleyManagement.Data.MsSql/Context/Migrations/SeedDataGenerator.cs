namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics; 

    using VolleyManagement.Data.MsSql.Entities;

    /// <summary>
    /// Genetrates and seeds test entity data 
    /// </summary>
    internal static class SeedDataGenerator
    {
        /// <summary>
        /// Generates and seeds test data for 
        /// players, trams, tournaments and game results entities
        /// </summary>
        /// <param name="context">Context of the entities</param>
        [Conditional("DEBUG")]
        internal static void GenerateEntities(VolleyManagementEntities context)
        {
            PlayerEntity[] players;
            TeamEntity[] teams;
            TournamentEntity[] tournaments;
            GameResultEntity[] games;

            players = GeneratePlayers(); 
            teams = GenerateTeams(players); 
            tournaments = GenerateTournaments(teams); 
            games = GenerateGamesFromTournaments(tournaments);

            context.Players.AddOrUpdate(p => new { p.FirstName, p.LastName }, players);
            context.Teams.AddOrUpdate(t => t.Name, teams);
            context.Tournaments.AddOrUpdate(t => t.Name, tournaments);

            context.SaveChanges();

            context.GameResults.AddOrUpdate(g => g.Id, games);

            context.SaveChanges(); 
        }

        private static PlayerEntity[] GeneratePlayers()
        {
            return new PlayerEntity[]
            {
                new PlayerEntity
                {
                    BirthYear = 1970,
                    FirstName = "John",
                    LastName = "Smith",
                    Height = 180,
                    Weight = 70
                },
                new PlayerEntity
                {
                    BirthYear = 1986,
                    FirstName = "Lex",
                    LastName = "Luthor",
                    Height = 175,
                    Weight = 75
                },
                new PlayerEntity
                {
                    BirthYear = 1977,
                    FirstName = "Darth",
                    LastName = "Vader",
                    Height = 181,
                    Weight = 80
                },
                new PlayerEntity
                {
                    BirthYear = 1988,
                    FirstName = "Kylo",
                    LastName = "Ren",
                    Height = 176,
                    Weight = 80
                },
                new PlayerEntity
                {
                    BirthYear = 85,
                    FirstName = "Han",
                    LastName = "Solo",
                    Height = 180,
                    Weight = 75
                },
                new PlayerEntity
                {
                    BirthYear = 1968,
                    FirstName = "Luke",
                    LastName = "Skywalker",
                    Height = 165,
                    Weight = 6
                },
                new PlayerEntity
                {
                    BirthYear = 1990,
                    FirstName = "Obivan",
                    LastName = "Kenobi",
                    Height = 190,
                    Weight = 120
                },
                new PlayerEntity
                {
                    BirthYear = 2005,
                    FirstName = "Mighty",
                    LastName = "Thor",
                    Height = 250,
                    Weight = 400
                },
                new PlayerEntity
                {
                    BirthYear = 1945,
                    FirstName = "Tony",
                    LastName = "Stark",
                    Height = 150,
                    Weight = 50
                },
                new PlayerEntity
                {
                    BirthYear = 1920,
                    FirstName = "Hulk",
                    LastName = "Incredible",
                    Height = 250,
                    Weight = 500
                },
                new PlayerEntity
                {
                    BirthYear = 1955,
                    FirstName = "Man",
                    LastName = "Ant",
                    Height = 10,
                    Weight = 10
                },
                new PlayerEntity
                {
                    BirthYear = 1900,
                    FirstName = "Clark",
                    LastName = "Kent",
                    Height = 200,
                    Weight = 300
                }
            };
        }

        private static TeamEntity[] GenerateTeams(PlayerEntity[] players)
        {
            return new TeamEntity[]
            {
                new TeamEntity
                {
                    Players = new List<PlayerEntity> { players[1] },
                    CaptainId = 1,
                    Name = "First Order",
                    Coach = "Coach1"
                },
                new TeamEntity
                {
                    Players = new List<PlayerEntity> { players[3] },
                    CaptainId = 2,
                    Name = "Empire",
                    Coach = "Coach2",
                },
                new TeamEntity
                {
                    Players = new List<PlayerEntity> { players[5], players[6] },
                    CaptainId = 3,
                    Name = "Rebelion",
                    Coach = "Coach3"
                },
                new TeamEntity
                {
                    Players = new List<PlayerEntity> { players[8] },
                    CaptainId = 4,
                    Name = "Avengers",
                    Coach = "Coach4"
                },
                new TeamEntity
                {
                    Players = new List<PlayerEntity> { players[10] },
                    CaptainId = 5,
                    Name = "Cap",
                    Coach = "Coach5"
                },
                new TeamEntity
                {
                    Players = new List<PlayerEntity> { players[11] },
                    CaptainId = 6,
                    Name = "DC",
                    Coach = "Coach6"
                }
            };
        }

        private static TournamentEntity[] GenerateTournaments(TeamEntity[] teams)
        {
            return new TournamentEntity[]
            {
                // Past torunament, scheme 1
                    new TournamentEntity
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
                    Teams = new List<TeamEntity>() 
                    {
                        teams[0],
                        teams[1],
                        teams[2],
                        teams[3],
                        teams[4],
                        teams[5]
                    }
                },
                // Current tournament, shceme 1
                new TournamentEntity
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
                    Teams = new List<TeamEntity>()
                    { 
                        teams[0],
                        teams[1],
                        teams[2],
                        teams[3],
                        teams[4] 
                    }
                },
                // Future tournament scheme 1
                new TournamentEntity
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
                    Teams = new List<TeamEntity>() 
                    { 
                        teams[0],
                        teams[1],
                        teams[2],
                        teams[3],
                        teams[4]
                    }
                },
                // Past tournament, scheme 2
                new TournamentEntity
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
                        teams[0],
                        teams[2],
                        teams[4]
                    }
                }, 
                // Current tournament, scheme 2
                new TournamentEntity
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
                    Teams = new List<TeamEntity>() 
                    { 
                        teams[0],
                        teams[1],
                        teams[2],
                        teams[3],
                        teams[4]
                    }
                },
                // Future tournament, scheme 2
                new TournamentEntity
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
                    Teams = new List<TeamEntity>() { 
                        teams[5], 
                        teams[4],
                        teams[2],
                        teams[3],
                        teams[0] 
                    }
                }
            };
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
                games.AddRange(GenerateGames(tours[i], i + 1, ++gameId));
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
            while (roundIter != roundsNumber + 1);

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
            int maxFinalScore = 3;
            int maxScore = 25;
            int scoresNumber = 5;
            Random rand = new Random();

            int awayFinalScore = 0;
            int homeFinalScroe = 0;
            int[] homeScores = new int[scoresNumber];
            int[] awayScores = new int[scoresNumber];

            foreach (GameResultEntity game in games)
            {
                int r = rand.Next(0, 2);
                if (r == 0)
                {
                    awayFinalScore = maxFinalScore;
                    homeFinalScroe = rand.Next(0, 3);
                }
                else
                {
                    awayFinalScore = rand.Next(0, 3);
                    homeFinalScroe = maxFinalScore;
                }

                for (int i = 0, j = 1; i < scoresNumber; i++, j++)
                {
                    if (j > awayFinalScore + homeFinalScroe)
                    {
                        awayScores[i] = 0;
                        homeScores[i] = 0;
                    }
                    else if (homeFinalScroe > awayFinalScore && j <= awayFinalScore)
                    {
                        awayScores[i] = maxScore;
                        homeScores[i] = rand.Next(0, 20);
                    }
                    else if (homeFinalScroe > awayFinalScore && j > awayFinalScore)
                    {
                        awayScores[i] = rand.Next(0, 20);
                        homeScores[i] = maxScore;
                    }
                    else if (awayFinalScore > homeFinalScroe && j <= homeFinalScroe)
                    {
                        awayScores[i] = rand.Next(0, 20);
                        homeScores[i] = maxScore;
                    }
                    else if (awayFinalScore > homeFinalScroe && j > homeFinalScroe)
                    {
                        awayScores[i] = maxScore;
                        homeScores[i] = rand.Next(0, 20);
                    }
                }

                game.HomeSetsScore = (byte)homeFinalScroe;
                game.AwaySetsScore = (byte)awayFinalScore;
                game.HomeSet1Score = (byte)homeScores[0];
                game.HomeSet2Score = (byte)homeScores[1];
                game.HomeSet3Score = (byte)homeScores[2];
                game.HomeSet4Score = (byte)homeScores[3];
                game.HomeSet5Score = (byte)homeScores[4];
                game.AwaySet1Score = (byte)awayScores[0];
                game.AwaySet2Score = (byte)awayScores[1];
                game.AwaySet3Score = (byte)awayScores[2];
                game.AwaySet4Score = (byte)awayScores[3];
                game.AwaySet5Score = (byte)awayScores[4];
            }
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
    }
}
