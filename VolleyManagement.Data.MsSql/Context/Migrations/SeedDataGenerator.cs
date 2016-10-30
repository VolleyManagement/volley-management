namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Domain.RolesAggregate;

    /// <summary>
    /// Generates and seeds test entity data
    /// </summary>
    internal static class SeedDataGenerator
    {
        private const string ADMINISTRATOR_ROLE_NAME = "Administrator";
        private const string USER_ROLE_NAME = "User";
        private const string TOURNAMENT_ADMINISTRATOR_ROLE_NAME = "TournamentAdministrator";
        private static Random _rand;

        /// <summary>
        /// Generates and seeds test data for
        /// players, trams, tournaments and game results entities
        /// </summary>
        /// <param name="context">Context of the entities</param>
        [Conditional("DEBUG")]
        internal static void GenerateEntities(VolleyManagementEntities context)
        {
            _rand = new Random();

            List<PlayerEntity> players = GeneratePlayers();
            List<TeamEntity> teams = GenerateTeams(players);
            List<TournamentEntity> tours = GenerateTournaments(teams);
            List<GameResultEntity> games = GenerateGames(tours);

            context.Players.AddOrUpdate(p => new { p.FirstName, p.LastName }, players.ToArray());
            context.Teams.AddOrUpdate(t => t.Name, teams.ToArray());
            context.SaveChanges();

            AssignPlayersToTeams(players);
            context.Players.AddOrUpdate(p => new { p.FirstName, p.LastName }, players.ToArray());
            context.SaveChanges();

            context.Tournaments.AddOrUpdate(t => t.Name, tours.ToArray());
            context.SaveChanges();

            SetGameScores(tours[0].GameResults);
            SetGameScores(tours[3].GameResults);
            SetGameScores(tours[1].GameResults, 50);
            SetGameScores(tours[4].GameResults, 30);
            context.GameResults.AddOrUpdate(g => g.Id, games.ToArray());

            context.SaveChanges();
        }

        /// <summary>
        /// Generates and seeds all required data
        /// </summary>
        /// <param name="context">Context of the entities</param>
        internal static void GenerateRequiredEntities(VolleyManagementEntities context)
        {
            GenerateRoles(context);
            GenerateRolesToOperationsMap(context);
        }

        #region Optional

        private static List<PlayerEntity> GeneratePlayers()
        {
            return new List<PlayerEntity>
            {
                new PlayerEntity
                {
                    Id = 1,
                    BirthYear = 1970,
                    FirstName = "John",
                    LastName = "Smith",
                    Height = 180,
                    Weight = 70
                },
                new PlayerEntity
                {
                    Id = 2,
                    BirthYear = 1986,
                    FirstName = "Lex",
                    LastName = "Luthor",
                    Height = 175,
                    Weight = 75
                },
                new PlayerEntity
                {
                    Id = 3,
                    BirthYear = 1977,
                    FirstName = "Darth",
                    LastName = "Vader",
                    Height = 181,
                    Weight = 80
                },
                new PlayerEntity
                {
                    Id = 4,
                    BirthYear = 1988,
                    FirstName = "Kylo",
                    LastName = "Ren",
                    Height = 176,
                    Weight = 80
                },
                new PlayerEntity
                {
                    Id = 5,
                    BirthYear = 85,
                    FirstName = "Han",
                    LastName = "Solo",
                    Height = 180,
                    Weight = 75
                },
                new PlayerEntity
                {
                    Id = 6,
                    BirthYear = 1968,
                    FirstName = "Luke",
                    LastName = "Skywalker",
                    Height = 165,
                    Weight = 60
                },
                new PlayerEntity
                {
                    Id = 7,
                    BirthYear = 1990,
                    FirstName = "Obivan",
                    LastName = "Kenobi",
                    Height = 190,
                    Weight = 120
                },
                new PlayerEntity
                {
                    Id = 8,
                    BirthYear = 2005,
                    FirstName = "Mighty",
                    LastName = "Thor",
                    Height = 250,
                    Weight = 400
                },
                new PlayerEntity
                {
                    Id = 9,
                    BirthYear = 1945,
                    FirstName = "Tony",
                    LastName = "Stark",
                    Height = 150,
                    Weight = 50
                },
                new PlayerEntity
                {
                    Id = 10,
                    BirthYear = 1920,
                    FirstName = "Hulk",
                    LastName = "Incredible",
                    Height = 250,
                    Weight = 500
                },
                new PlayerEntity
                {
                    Id = 11,
                    BirthYear = 1955,
                    FirstName = "Man",
                    LastName = "Ant",
                    Height = 10,
                    Weight = 10
                },
                new PlayerEntity
                {
                    Id = 12,
                    BirthYear = 1900,
                    FirstName = "Clark",
                    LastName = "Kent",
                    Height = 200,
                    Weight = 300
                }
            };
        }

        private static List<TeamEntity> GenerateTeams(List<PlayerEntity> players)
        {
            return new List<TeamEntity>
            {
                new TeamEntity
                {
                    Id = 1,
                    CaptainId = 1,
                    Name = "First Order",
                    Coach = "Coach1"
                },
                new TeamEntity
                {
                    Id = 2,
                    CaptainId = 2,
                    Name = "Empire",
                    Coach = "Coach2",
                },
                new TeamEntity
                {
                    Id = 3,
                    CaptainId = 3,
                    Name = "Rebelion",
                    Coach = "Coach3"
                },
                new TeamEntity
                {
                    Id = 4,
                    CaptainId = 4,
                    Name = "Avengers",
                    Coach = "Coach4"
                },
                new TeamEntity
                {
                    Id = 5,
                    CaptainId = 5,
                    Name = "Cap",
                    Coach = "Coach5"
                },
                new TeamEntity
                {
                    Id = 6,
                    CaptainId = 6,
                    Name = "DC",
                    Coach = "Coach6"
                }
            };
        }

        private static void AssignPlayersToTeams(List<PlayerEntity> players)
        {
            players[0].TeamId = 1;
            players[1].TeamId = 2;
            players[2].TeamId = 3;
            players[3].TeamId = 4;
            players[4].TeamId = 5;
            players[5].TeamId = 6;

            players[6].TeamId = 1;
            players[7].TeamId = 2;
            players[8].TeamId = 3;
            players[9].TeamId = 4;
            players[10].TeamId = 5;
            players[11].TeamId = 6;
        }

        private static List<TournamentEntity> GenerateTournaments(List<TeamEntity> teams)
        {
            List<TournamentEntity> tours = GenerateTournamentsSchemOne(teams);
            tours.AddRange(GenerateTournamentsSchemeTwo(teams));
            tours.AddRange(GenerateTournamentsSchemePlayoff(teams));

            return tours;
        }

        private static List<TournamentEntity> GenerateTournamentsSchemOne(List<TeamEntity> teams)
        {
            return new List<TournamentEntity>
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
                    LastTimeUpdated = new DateTime(2015, 09, 29),
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
                    ApplyingPeriodEnd = DateTime.Now.AddDays(-10),
                    GamesStart = DateTime.Now.AddDays(-8),
                    GamesEnd = DateTime.Now.AddDays(3),
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
                    LastTimeUpdated = DateTime.Now.AddDays(-8),
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
                    GamesEnd = DateTime.Now.AddMonths(2).AddDays(12),
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
                    LastTimeUpdated = DateTime.Now.AddMonths(2).AddDays(2),
                    Teams = new List<TeamEntity>()
                    {
                        teams[0],
                        teams[1],
                        teams[2],
                        teams[3],
                        teams[4]
                    }
                }
            };
        }

        private static List<TournamentEntity> GenerateTournamentsSchemeTwo(List<TeamEntity> teams)
        {
            return new List<TournamentEntity>
            {
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
                    LastTimeUpdated = new DateTime(2017, 06, 01),
                    Teams = new List<TeamEntity>()
                    {
                        teams[0],
                        teams[2],
                        teams[4]
                    }
                },

                // Current tournament, scheme 2
                new TournamentEntity
                {
                    Name = "Epic tour",
                    ApplyingPeriodStart = DateTime.Now.AddMonths(-20),
                    ApplyingPeriodEnd = DateTime.Now.AddDays(-11),
                    GamesStart = DateTime.Now.AddDays(-10),
                    GamesEnd = DateTime.Now.AddDays(2),
                    TransferStart = DateTime.Now.AddDays(-4),
                    TransferEnd = DateTime.Now.AddDays(2),
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
                    LastTimeUpdated = DateTime.Now.AddDays(-10),
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
                    GamesEnd = DateTime.Now.AddMonths(2).AddDays(28),
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
                    LastTimeUpdated = DateTime.Now.AddMonths(2).AddDays(16),
                    Teams = new List<TeamEntity>()
                    {
                        teams[5],
                        teams[4],
                        teams[2],
                        teams[3],
                        teams[0]
                    }
                }
            };
        }

        private static List<TournamentEntity> GenerateTournamentsSchemePlayoff(List<TeamEntity> teams)
        {
            List<TournamentEntity> tournaments = new List<TournamentEntity>()
            {
                // Past
                new TournamentEntity()
                {
                    Name = "Tour Playoff #1",
                    ApplyingPeriodStart = new DateTime(2012, 01, 03),
                    ApplyingPeriodEnd = new DateTime(2012, 05, 04),
                    GamesStart = new DateTime(2012, 05, 20),
                    GamesEnd = new DateTime(2012, 06, 01),
                    TransferStart = new DateTime(2012, 05, 20),
                    TransferEnd = new DateTime(2012, 06, 01),
                    Scheme = 4,
                    Season = 114,
                     Divisions = new List<DivisionEntity>()
                    {
                        new DivisionEntity()
                        {
                            Name = "Division 7",
                            Groups = new List<GroupEntity>()
                            {
                                new GroupEntity()
                                {
                                    Name = "Group 7"
                                }
                            }
                        }
                    },
                    Teams = teams
                },

                // Current
                new TournamentEntity
                {
                    Name = "Tour Playoff #2",
                    ApplyingPeriodStart = DateTime.Now.AddMonths(-21),
                    ApplyingPeriodEnd = DateTime.Now.AddDays(-10),
                    GamesStart = DateTime.Now.AddDays(-9),
                    GamesEnd = DateTime.Now.AddDays(6),
                    TransferStart = DateTime.Now.AddDays(-4),
                    TransferEnd = DateTime.Now.AddDays(3),
                    Scheme = 4,
                    Season = Convert.ToByte(DateTime.Now.Year - 1900),
                    Divisions = new List<DivisionEntity>
                    {
                        new DivisionEntity
                        {
                            Name = "Division 8",
                            Groups = new List<GroupEntity>()
                            {
                                new GroupEntity()
                                {
                                    Name = "Group 8"
                                }
                            }
                        }
                    },
                    Teams = teams
                },

                // Future
                new TournamentEntity
                {
                    Name = "Tour Playoff #3",
                    ApplyingPeriodStart = DateTime.Now.AddMonths(1),
                    ApplyingPeriodEnd = DateTime.Now.AddMonths(2),
                    GamesStart = DateTime.Now.AddMonths(2).AddDays(2),
                    GamesEnd = DateTime.Now.AddMonths(2).AddDays(12),
                    TransferStart = DateTime.Now.AddMonths(2).AddDays(2),
                    TransferEnd = DateTime.Now.AddMonths(2).AddDays(7),
                    Scheme = 4,
                    Season = Convert.ToByte(DateTime.Now.Year - 1900),
                    Divisions = new List<DivisionEntity>()
                    {
                        new DivisionEntity()
                        {
                            Name = "Division 9",
                            Groups = new List<GroupEntity>()
                            {
                                new GroupEntity()
                                {
                                    Name = "Group 9"
                                }
                            }
                        }
                    },
                    Teams = new List<TeamEntity>
                    {
                        teams[0],
                        teams[1],
                        teams[2]
                    }
                }
            };

            return tournaments;
        }

        private static List<GameResultEntity> GenerateGames(List<TournamentEntity> tours)
        {
            List<GameResultEntity> games = new List<GameResultEntity>();
            int gameId = 0;

            for (int i = 0; i < tours.Count; i++)
            {
                if (games.Count > 0)
                {
                    gameId = games[games.Count - 1].Id;
                }

                tours[i].GameResults = new List<GameResultEntity>();
                List<GameResultEntity> gamesInTour = null;

                if (tours[i].Scheme == 1 || tours[i].Scheme == 2)
                {
                    gamesInTour = GenerateGamesForSchemeOneAndTwo(tours[i], i + 1, ++gameId);
                }
                else if (tours[i].Scheme == 4)
                {
                    gamesInTour = GenerateGamesForSchemePlayoff(tours[i], i + 1, ++gameId);
                }

                for (int j = 0; j < gamesInTour.Count; j++)
                {
                    tours[i].GameResults.Add(gamesInTour[j]);
                }

                games.AddRange(gamesInTour);
            }

            return games;
        }

        private static List<GameResultEntity> GenerateGamesForSchemeOneAndTwo(TournamentEntity tour, int tourId, int gameId)
        {
            List<GameResultEntity> games = new List<GameResultEntity>();
            int teamsCount = tour.Teams.Count;
            int roundsNumber = teamsCount % 2 == 0 ? (teamsCount - 1) : teamsCount;
            int gamesInRound = roundsNumber % 2 == 0 ? (roundsNumber / 2) : ((roundsNumber / 2) + 1);

            // Initial round
            byte roundIter = 1;
            int[] homeTeamIds = new int[gamesInRound];
            int[] awayTeamIds = new int[gamesInRound];
            int[] tempHome = new int[gamesInRound];
            int[] tempAway = new int[gamesInRound];
            for (int i = 0, j = 0; i < gamesInRound; i++, j++)
            {
                homeTeamIds[i] = tour.Teams[j].Id;
                awayTeamIds[i] = i == gamesInRound - 1 && teamsCount % 2 != 0 ? 0 : tour.Teams[++j].Id;

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
                        HomeTeamId = currentHomeTeamId == 0 ? currentAwayTeamId.Value : currentHomeTeamId,
                        AwayTeamId = currentHomeTeamId == 0 ? null : currentAwayTeamId,
                        StartTime = games.Count > 0 ?
                            tour.GamesStart.AddDays(roundIter).AddHours(i)
                            : tour.GamesStart.AddDays(1),
                        RoundNumber = Convert.ToByte(roundIter)
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

            return games;
        }

        private static List<GameResultEntity> GenerateGamesForSchemePlayoff(TournamentEntity tour, int tourId, int startGameId)
        {
            int currentNumberOfTeams = tour.Teams.Count;
            List<GameResultEntity> games = GenerateGamesInFirstRoundSchemePlyoff(tour, tourId, startGameId);
            int numberOfGamesInFirstRound = games.Count;
            int numberOfRounds = GetNumberOfRoundsInPlayoffScheme(currentNumberOfTeams);
            int numberOfGamesInTournament = Convert.ToInt32(Math.Pow(2, numberOfRounds));

            for (int i = 1; i < numberOfRounds; i++)
            {
                int numberOfGamesInRound = Convert.ToInt32(numberOfGamesInFirstRound / Math.Pow(2, i));

                // include loser teams into last round
                if (numberOfGamesInRound == 1 && numberOfRounds > 1)
                {
                    numberOfGamesInRound = 2;
                }

                for (int j = 0; j < numberOfGamesInRound; j++)
                {
                    GameResultEntity previousGame = games[games.Count - 1];
                    games.Add(new GameResultEntity
                    {
                        Id = previousGame.Id + 1,
                        TournamentId = tourId,
                        GameNumber = Convert.ToByte(previousGame.GameNumber + 1),
                        RoundNumber = Convert.ToByte(i + 1),
                        StartTime = previousGame.StartTime.Value.AddHours(1)
                    });
                }
            }

            ScheduleGamesSchemePlayoff(numberOfRounds, games);

            return games;
        }

        private static void ScheduleGamesSchemePlayoff(int numberOfRounds, List<GameResultEntity> games)
        {
            // Set results and schedule next game
            for (int i = 0; i < numberOfRounds; i++)
            {
                List<GameResultEntity> gamesInCurrentRound = games
                    .Where(x => x.RoundNumber == i + 1)
                    .ToList();
                foreach (GameResultEntity game in gamesInCurrentRound)
                {
                    SetSingleGameScores(game);

                    if (game.RoundNumber == numberOfRounds)
                    {
                        continue;
                    }

                    int winnerId = 0;
                    if (game.AwayTeamId == null || game.HomeSetsScore > game.AwaySetsScore)
                    {
                        winnerId = game.HomeTeamId.Value;
                    }
                    else
                    {
                        winnerId = game.AwayTeamId.Value;
                    }

                    // Get last game number from previous game
                    int lastGameNumberPreviousRound = 0;

                    if (game.RoundNumber > 1)
                    {
                        lastGameNumberPreviousRound = games
                        .Where(gr => gr.RoundNumber == game.RoundNumber - 1)
                        .Max(gr => gr.GameNumber);
                    }

                    int offset = (game.GameNumber % 2 == 0) ?
                        ((game.GameNumber - lastGameNumberPreviousRound) / 2)
                        : (((game.GameNumber - lastGameNumberPreviousRound) / 2) + 1);

                    int nextWinnerGameNumber = gamesInCurrentRound[gamesInCurrentRound.Count - 1]
                        .GameNumber + offset;

                    if (game.RoundNumber == numberOfRounds - 1 && numberOfRounds > 1)
                    {
                        // add losers to next round
                        int nextLoserGameNumber = nextWinnerGameNumber;
                        nextWinnerGameNumber++;

                        GameResultEntity loserGame = games
                            .Where(gr => gr.GameNumber == nextLoserGameNumber)
                            .SingleOrDefault();

                        int? loserId = 0;
                        if (game.AwayTeamId == null)
                        {
                            loserId = null;
                        }
                        else
                        {
                            loserId = game.HomeSetsScore > game.AwaySetsScore ?
                            game.AwayTeamId.Value : game.HomeTeamId.Value;
                        }

                        if (loserGame.HomeTeamId == null)
                        {
                            loserGame.HomeTeamId = loserId;
                        }
                        else
                        {
                            loserGame.AwayTeamId = loserId;
                        }
                    }

                    GameResultEntity gameInNextRound = games
                        .Where(gr => gr.GameNumber == nextWinnerGameNumber)
                        .SingleOrDefault();

                    if (gameInNextRound.HomeTeamId == null)
                    {
                        gameInNextRound.HomeTeamId = winnerId;
                    }
                    else
                    {
                        gameInNextRound.AwayTeamId = winnerId;
                    }
                }
            }
        }

        private static List<GameResultEntity> GenerateGamesInFirstRoundSchemePlyoff(TournamentEntity tour, int tourId, int startGameId)
        {
            List<GameResultEntity> gamesInFirstRound = new List<GameResultEntity>();
            int teamsCount = Convert.ToInt32(Math.Pow(2, GetNumberOfRoundsInPlayoffScheme(tour.Teams.Count)));
            List<TeamEntity> teamsInTournament = tour.Teams.ToList();

            for (int i = 0; i < teamsCount - tour.Teams.Count; i++)
            {
                teamsInTournament.Add(null);
            }

            int gamesCount = (teamsCount % 2 == 0) ? (teamsCount / 2) : (teamsCount / 2) + 1;
            for (int i = 0; i < gamesCount; i++)
            {
                // Make sure that non of the teams plays with itself or there are two free days in one game
                int homeTeamId = teamsInTournament[0].Id;
                int lastTeamIndex = teamsInTournament.Count - 1;
                int? awayTeamId = teamsInTournament[lastTeamIndex] == null ?
                    null : (int?)teamsInTournament[lastTeamIndex].Id;
                teamsInTournament.RemoveAt(lastTeamIndex);
                teamsInTournament.RemoveAt(0);

                // Now each next game starts one hour after the previous
                gamesInFirstRound.Add(new GameResultEntity
                    {
                        Id = startGameId++,
                        HomeTeamId = homeTeamId,
                        AwayTeamId = awayTeamId,
                        TournamentId = tourId,
                        GameNumber = Convert.ToByte(i + 1),
                        RoundNumber = 1,
                        StartTime = tour.GamesStart.AddHours(i)
                    });
            }

            return gamesInFirstRound;
        }

        private static int GetNumberOfGamesByRoundNumerSchemePlayoff(int roundNumber, int gamesInFirstRound)
        {
            int numberOfGames = roundNumber <= 0 ? 0 : Convert.ToInt32(gamesInFirstRound / Math.Pow(2, roundNumber - 1));
            return numberOfGames == 1 ? 2 : numberOfGames;
        }

        private static int GetNumberOfRoundsInPlayoffScheme(int value)
        {
            return value > 0 ?
                Convert.ToInt32(Math.Ceiling(Math.Log(value) / Math.Log(2)))
                : 0;
        }

        private static void SetSingleGameScores(GameResultEntity game)
        {
            if (game.AwayTeamId == null)
            {
                return;
            }

            byte maxFinalScore = 3;
            byte maxScore = 25;
            byte maxSecondTeamScore = 23;
            byte maxScoreLast = 15;
            byte maxSecondTeamScoreLast = 13;
            byte scoresNumber = 5;

            byte awayFinalScore = 0;
            byte homeFinalScroe = 0;
            byte[] homeScores = new byte[scoresNumber];
            byte[] awayScores = new byte[scoresNumber];

            int r = _rand.Next(0, 2);
            if (r == 0)
            {
                awayFinalScore = maxFinalScore;
                homeFinalScroe = (byte)_rand.Next(0, maxFinalScore);
            }
            else
            {
                awayFinalScore = (byte)_rand.Next(0, maxFinalScore);
                homeFinalScroe = maxFinalScore;
            }

            for (int i = 0, j = 1; i < scoresNumber; i++, j++)
            {
                byte currentMaxScore = j == scoresNumber ? maxScoreLast : maxScore;
                byte currentMaxSecondTeamScore = j == scoresNumber ?
                    maxSecondTeamScoreLast : maxSecondTeamScore;

                if (j > awayFinalScore + homeFinalScroe)
                {
                    awayScores[i] = 0;
                    homeScores[i] = 0;
                }
                else if (homeFinalScroe > awayFinalScore && j <= awayFinalScore)
                {
                    awayScores[i] = currentMaxScore;
                    homeScores[i] = (byte)_rand.Next(0, currentMaxSecondTeamScore);
                }
                else if (homeFinalScroe > awayFinalScore && j > awayFinalScore)
                {
                    awayScores[i] = (byte)_rand.Next(0, currentMaxSecondTeamScore);
                    homeScores[i] = currentMaxScore;
                }
                else if (awayFinalScore > homeFinalScroe && j <= homeFinalScroe)
                {
                    awayScores[i] = (byte)_rand.Next(0, currentMaxSecondTeamScore);
                    homeScores[i] = currentMaxScore;
                }
                else if (awayFinalScore > homeFinalScroe && j > homeFinalScroe)
                {
                    awayScores[i] = currentMaxScore;
                    homeScores[i] = (byte)_rand.Next(0, currentMaxSecondTeamScore);
                }
            }

            game.HomeSetsScore = homeFinalScroe;
            game.AwaySetsScore = awayFinalScore;
            game.HomeSet1Score = homeScores[0];
            game.HomeSet2Score = homeScores[1];
            game.HomeSet3Score = homeScores[2];
            game.HomeSet4Score = homeScores[3];
            game.HomeSet5Score = homeScores[4];
            game.AwaySet1Score = awayScores[0];
            game.AwaySet2Score = awayScores[1];
            game.AwaySet3Score = awayScores[2];
            game.AwaySet4Score = awayScores[3];
            game.AwaySet5Score = awayScores[4];
        }

        private static void SetGameScores(ICollection<GameResultEntity> games, int percentage = 100)
        {
            // Only for past games
            byte maxPercents = 100;

            int actualPercentage = percentage;
            if (percentage > maxPercents)
            {
                actualPercentage = maxPercents;
            }
            else if (percentage < 0)
            {
                actualPercentage = 0;
            }

            IEnumerator<GameResultEntity> gameEnumerator = games.GetEnumerator();
            gameEnumerator.MoveNext();
            for (int k = 0; k < games.Count; k++, gameEnumerator.MoveNext())
            {
                SetSingleGameScores(gameEnumerator.Current);
            }
        }

        private static List<GameResultEntity> GenerateGamesDuplicateInSchemeTwo(
            List<GameResultEntity> games,
            int roundNumber)
        {
            List<GameResultEntity> duplicates = new List<GameResultEntity>();

            int gameId = games[games.Count - 1].Id;
            foreach (GameResultEntity game in games)
            {
                int? homeTeamId = game.HomeTeamId;
                int? awayTeamId = game.AwayTeamId;

                if (awayTeamId.HasValue && homeTeamId.HasValue)
                {
                    int temp = homeTeamId.Value;
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

        #endregion

        #region Required

        private static void GenerateRoles(VolleyManagementEntities context)
        {
            var defaultRoles = new List<RoleEntity>
            {
                CreateRole(ADMINISTRATOR_ROLE_NAME),
                CreateRole(TOURNAMENT_ADMINISTRATOR_ROLE_NAME),
                CreateRole(USER_ROLE_NAME)
            };

            context.Roles.AddOrUpdate(r => r.Name, defaultRoles.ToArray());
            context.SaveChanges();
        }

        private static void GenerateRolesToOperationsMap(VolleyManagementEntities context)
        {
            int roleId = context.Roles.Where(r => r.Name == TOURNAMENT_ADMINISTRATOR_ROLE_NAME).First().Id;
            GenerateTournamentAdministratorOperations(roleId, context);

            roleId = context.Roles.Where(r => r.Name == ADMINISTRATOR_ROLE_NAME).First().Id;
            GenerateAdministratorOperations(roleId, context);
        }

        private static void GenerateTournamentAdministratorOperations(int roleId, VolleyManagementEntities context)
        {
            var operationIds = new List<short>()
            {
                AuthOperations.Tournaments.Create,
                AuthOperations.Tournaments.Edit,
                AuthOperations.Tournaments.Delete,
                AuthOperations.Tournaments.ManageTeams,
                AuthOperations.Teams.Create,
                AuthOperations.Teams.Edit,
                AuthOperations.Teams.Delete,
                AuthOperations.Games.Create,
                AuthOperations.Games.Edit,
                AuthOperations.Games.EditResult,
                AuthOperations.Games.Delete,
                AuthOperations.Games.SwapRounds,
                AuthOperations.Players.Create,
                AuthOperations.Players.Edit,
                AuthOperations.Players.Delete
            };

            var entries = CreateRolesToOperation(roleId, operationIds);

            context.RolesToOperations.AddOrUpdate(r => new { r.RoleId, r.OperationId }, entries.ToArray());
        }

        private static void GenerateAdministratorOperations(int roleId, VolleyManagementEntities context)
        {
            var operationIds = new List<short>()
            {
                AuthOperations.Tournaments.Create,
                AuthOperations.Tournaments.Edit,
                AuthOperations.Tournaments.Delete,
                AuthOperations.Tournaments.ManageTeams,
                AuthOperations.Teams.Create,
                AuthOperations.Teams.Edit,
                AuthOperations.Teams.Delete,
                AuthOperations.Games.Create,
                AuthOperations.Games.Edit,
                AuthOperations.Games.EditResult,
                AuthOperations.Games.Delete,
                AuthOperations.Games.SwapRounds,
                AuthOperations.Players.Create,
                AuthOperations.Players.Edit,
                AuthOperations.Players.Delete,
                AuthOperations.AdminDashboard.View,
                AuthOperations.AllUsers.ViewList,
                AuthOperations.AllUsers.ViewDetails,
                AuthOperations.Feedbacks.Edit,
                AuthOperations.Feedbacks.Delete
            };

            var entries = CreateRolesToOperation(roleId, operationIds);

            context.RolesToOperations.AddOrUpdate(r => new { r.RoleId, r.OperationId }, entries.ToArray());
        }

        private static List<RoleToOperationEntity> CreateRolesToOperation(int roleId, List<short> operationIds)
        {
            var entries = new List<RoleToOperationEntity>();
            foreach (var operationId in operationIds)
            {
                entries.Add(new RoleToOperationEntity { RoleId = roleId, OperationId = operationId });
            }

            return entries;
        }

        private static RoleEntity CreateRole(string name)
        {
            return new RoleEntity { Name = name };
        }

        #endregion
    }
}
