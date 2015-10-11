namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.Queries.Tournaments;
    using VolleyManagement.Domain.TournamentsAggregate;

    using Constants = VolleyManagement.Domain.Constants;

    /// <summary>
    /// Provides Object Query implementation for Tournaments
    /// </summary>
    public class TournamentQueries : IQuery<Tournament, FirstByNameCriterion>,
                                     IQuery<List<Tournament>, GetAllCriterion>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork"> The unit of work. </param>
        public TournamentQueries(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (VolleyUnitOfWork)unitOfWork;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criterion"> The criterion. </param>
        /// <returns> The <see cref="Tournament"/>. </returns>
        public Tournament Execute(FirstByNameCriterion criterion)
        {
            var query = _unitOfWork.Context.Tournaments.Where(t => t.Name == criterion.Name);
            if (criterion.EntityId.HasValue)
            {
                query = query.Where(t => t.Id == criterion.EntityId.GetValueOrDefault());
            }

            // ToDo: Use Automapper to substitute Select clause
            return query.Select(TournamentMap()).FirstOrDefault();
        }

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criterion"> The criterion. </param>
        /// <returns> The <see cref="Tournament"/>. </returns>
        public List<Tournament> Execute(GetAllCriterion criterion)
        {
            return _unitOfWork.Context.Tournaments.Select(TournamentMap()).ToList();
        }

        #endregion

        #region Mapping

        private static Expression<Func<TournamentEntity, Tournament>> TournamentMap()
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
                        Season = (short)(Constants.Tournament.SCHEMA_STORAGE_OFFSET + t.Season),
                        GamesStart = t.GamesStart,
                        GamesEnd = t.GamesEnd,
                        ApplyingPeriodStart = t.ApplyingPeriodStart,
                        ApplyingPeriodEnd = t.ApplyingPeriodEnd,
                        TransferEnd = t.TransferEnd,
                        TransferStart = t.TransferStart
                    };
        }

        #endregion
    }
}