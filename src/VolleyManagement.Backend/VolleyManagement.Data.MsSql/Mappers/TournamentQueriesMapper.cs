namespace VolleyManagement.Data.MsSql.Mappers
{
    using Domain.TournamentsAggregate;
    using Entities;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    static class TournamentQueriesMapper
    {
        public static Expression<Func<TournamentEntity, Tournament>> GetTournamentMapping()
        {
            return
                t =>
                new Tournament
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    RegulationsLink = t.RegulationsLink,
                    Scheme = (TournamentSchemeEnum)t.Scheme,
                    Season = (short)(ValidationConstants.Tournament.SCHEMA_STORAGE_OFFSET + t.Season),
                    GamesStart = t.GamesStart,
                    GamesEnd = t.GamesEnd,
                    ApplyingPeriodStart = t.ApplyingPeriodStart,
                    ApplyingPeriodEnd = t.ApplyingPeriodEnd,
                    TransferEnd = t.TransferEnd,
                    TransferStart = t.TransferStart,
                    Divisions = t.Divisions
                                    .AsQueryable()
                                    .Where(d => d.TournamentId == t.Id)
                                    .Select(GetDivisionMapping())
                                    .ToList(),
                    LastTimeUpdated = t.LastTimeUpdated,
                    IsArchived = t.IsArchived
                };
        }

        public static Expression<Func<DivisionEntity, Division>> GetDivisionMapping()
        {
            return
                d =>
                new Division
                {
                    Id = d.Id,
                    Name = d.Name,
                    TournamentId = d.TournamentId,
                    Groups = d.Groups
                                .AsQueryable()
                                .Where(g => g.DivisionId == d.Id)
                                .Select(GetGroupMapping())
                                .ToList()
                };
        }

        public static Expression<Func<GroupEntity, Group>> GetGroupMapping()
        {
            return
                g =>
                new Group
                {
                    Id = g.Id,
                    Name = g.Name,
                    DivisionId = g.DivisionId,
                    IsEmpty = g.Teams.Count == 0
                };
        }
    }
}
