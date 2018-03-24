namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Contracts;
    using Data.Queries.Common;
    using Data.Queries.Player;
    using Domain.PlayersAggregate;
    using Entities;

    /// <summary>
    /// Provides Query Object implementation for Player entity
    /// </summary>
    public class PlayerQueries : IQuery<Player, FindByIdCriteria>,
                                 IQuery<Player, FindByFullNameCriteria>,
                                 IQuery<IQueryable<Player>, GetAllCriteria>,
                                 IQuery<ICollection<Player>, TeamPlayersCriteria>
    {
        #region Fields

        private readonly VolleyUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerQueries"/> class.
        /// </summary>
        /// <param name="unitOfWork"> The unit of work. </param>
        public PlayerQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
        }

        #endregion

        #region Implemenations

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Player"/>. </returns>
        public Player Execute(FindByIdCriteria criteria)
        {
            return _unitOfWork.Context.Players
                                      .Where(t => t.Id == criteria.Id)
                                      .Select(GetPlayerMapping())
                                      .SingleOrDefault();
        }

        /// <summary>
        /// Finds Players by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Player"/>. </returns>
        public Player Execute(FindByFullNameCriteria criteria)
        {
            return _unitOfWork.Context.Players
                .Where(t => t.FirstName == criteria.FirstName)
                .Where(t => t.LastName == criteria.LastName)
                .Select(GetPlayerMapping())
                .SingleOrDefault();
        }

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Player"/>. </returns>
        public IQueryable<Player> Execute(GetAllCriteria criteria)
        {
            return _unitOfWork.Context.Players.Select(GetPlayerMapping());
        }

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Player"/>. </returns>
        public ICollection<Player> Execute(TeamPlayersCriteria criteria)
        {
            return _unitOfWork.Context.Players
                                      .Where(p => p.TeamId == criteria.TeamId)
                                      .Select(GetPlayerMapping())
                                      .ToList();
        }

        #endregion

        #region Mapping

        private static Expression<Func<PlayerEntity, Player>> GetPlayerMapping()
        {
            return p => new Player(p.Id, p.FirstName, p.LastName, p.BirthYear, p.Height, p.Weight, p.TeamId);
        }

        #endregion
    }
}