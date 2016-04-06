namespace VolleyManagement.Data.MsSql.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Domain.GamesAggregate;
    using VolleyManagement.Domain.PlayersAggregate;
    using VolleyManagement.Domain.TeamsAggregate;
    using VolleyManagement.Domain.TournamentsAggregate;
    using VolleyManagement.Domain.UsersAggregate;

    /// <summary>
    /// Maps Domain models to Dal.
    /// </summary>
    internal static class DomainToDal
    {
        /// <summary>
        /// Maps Tournament model.
        /// </summary>
        /// <param name="to">Mapping target</param>
        /// <param name="from">Mapping source</param>
        public static void Map(TournamentEntity to, Tournament from)
        {
            to.Id = from.Id;
            to.Name = from.Name;
            to.Season = (byte)(from.Season - ValidationConstants.Tournament.SCHEMA_STORAGE_OFFSET);
            to.Description = from.Description;
            to.Scheme = (byte)from.Scheme;
            to.RegulationsLink = from.RegulationsLink;
            to.GamesStart = from.GamesStart;
            to.GamesEnd = from.GamesEnd;
            to.ApplyingPeriodStart = from.ApplyingPeriodStart;
            to.ApplyingPeriodEnd = from.ApplyingPeriodEnd;
            to.TransferStart = from.TransferStart;
            to.TransferEnd = from.TransferEnd;
            to.Divisions = from.Divisions.Select(d => Map(d)).ToList();
        }

        /// <summary>
        /// Maps Player model.
        /// </summary>
        /// <param name="to">Target of the mapping</param>
        /// <param name="from">Source of the mapping</param>
        public static void Map(PlayerEntity to, Player from)
        {
            to.Id = from.Id;
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.BirthYear = from.BirthYear;
            to.Height = from.Height;
            to.Weight = from.Weight;
            to.TeamId = from.TeamId;
        }

        /// <summary>
        /// Maps Team model.
        /// </summary>
        /// <param name="to">Team Entity model</param>
        /// <param name="from">Team Domain model</param>
        public static void Map(TeamEntity to, Team from)
        {
            to.Id = from.Id;
            to.Name = from.Name;
            to.CaptainId = from.CaptainId;
            to.Coach = from.Coach;
            to.Achievements = from.Achievements;
        }

        /// <summary>
        /// Maps User model.
        /// </summary>
        /// <param name="to">User Entity model</param>
        /// <param name="from">User Domain model</param>
        public static void Map(UserEntity to, User from)
        {
            to.Id = from.Id;
            to.FullName = from.PersonName;
            to.UserName = from.UserName;
            to.Email = from.Email;
            to.CellPhone = from.PhoneNumber;
            to.LoginProviders = from.LoginProviders
                .Select(l =>
                {
                    return new LoginInfoEntity
                    {
                        LoginProvider = l.LoginProvider,
                        ProviderKey = l.ProviderKey
                    };
                })
                .ToList();
        }

        /// <summary>
        /// Maps Division model.
        /// </summary>
        /// <param name="from">Division Domain model</param>
        /// <param name="oldDivisions">Divisions which already exists in database</param>
        /// <returns>Division Entity model</returns>
        public static DivisionEntity Map(Division from, ICollection<DivisionEntity> oldDivisions)
        {
            if (from.Id == 0)
            {
                return new DivisionEntity
                {
                    Id = from.Id,
                    Name = from.Name,
                    TournamentId = from.TournamentId
                };
            }
            else
            {
                var division = oldDivisions.Where(d => d.Id == from.Id).SingleOrDefault();
                division.Name = from.Name;
                return division;
            }
        }

        /// <summary>
        /// Maps Division model
        /// </summary>
        /// <param name="to">Division entity model</param>
        /// <param name="from">Division domain model</param>
        public static void Map(DivisionEntity to, Division from)
        {
            to.Id = from.Id;
            to.Name = from.Name;
            to.TournamentId = from.TournamentId;
        }

        /// <summary>
        /// Maps Division model
        /// </summary>
        /// <param name="from">Division domain model</param>
        /// <returns>Dal division model</returns>
        public static DivisionEntity Map(Division from)
        {
            return new DivisionEntity()
            {
                Id = from.Id,
                Name = from.Name,
                TournamentId = from.TournamentId,
                Groups = from.Groups.Select(g => Map(g)).ToList()
            };
        }

        /// <summary>
        /// Maps group model
        /// </summary>
        /// <param name="from">Group to map</param>
        /// <returns>Dal entity</returns>
        public static GroupEntity Map(Group from)
        {
            return new GroupEntity
            {
                Id = from.Id,
                Name = from.Name,
                DivisionId = from.DivisionId
            };
        }

        /// <summary>
        /// Maps DAL model of game to domain model of game result.
        /// </summary>
        /// <param name="to">DAL model of game result.</param>
        /// <param name="from">Domain model of game.</param>
        public static void Map(GameResultEntity to, Game from)
        {
            to.Id = from.Id;
            to.TournamentId = from.TournamentId;
            to.HomeTeamId = from.HomeTeamId;
            to.AwayTeamId = from.AwayTeamId;
            to.HomeSetsScore = from.Result.SetsScore.Home;
            to.AwaySetsScore = from.Result.SetsScore.Away;
            to.IsTechnicalDefeat = from.Result.IsTechnicalDefeat;
            to.HomeSet1Score = from.Result.SetScores[0].Home;
            to.AwaySet1Score = from.Result.SetScores[0].Away;
            to.HomeSet2Score = from.Result.SetScores[1].Home;
            to.AwaySet2Score = from.Result.SetScores[1].Away;
            to.HomeSet3Score = from.Result.SetScores[2].Home;
            to.AwaySet3Score = from.Result.SetScores[2].Away;
            to.HomeSet4Score = from.Result.SetScores[3].Home;
            to.AwaySet4Score = from.Result.SetScores[3].Away;
            to.HomeSet5Score = from.Result.SetScores[4].Home;
            to.AwaySet5Score = from.Result.SetScores[4].Away;
            to.StartTime = from.GameDate;
            to.RoundNumber = Convert.ToByte(from.Round);
        }
    }
}
