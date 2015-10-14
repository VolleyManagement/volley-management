namespace VolleyManagement.Data.MsSql.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.Player;
    using VolleyManagement.Domain.PlayersAggregate;

    /// <summary>
    /// Provides Query Object implementation for Player entity
    /// </summary>
    public class PlayerQueries : IQuery<Player, FindByIdCriteria>,
                                 IQuery<IQueryable<Player>, GetAllCriteria>,
                                 IQuery<List<Player>, TeamPlayersCriteria>
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
            this._unitOfWork = (VolleyUnitOfWork)unitOfWork;
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
        public List<Player> Execute(TeamPlayersCriteria criteria)
        {
            return _unitOfWork.Context.Players
                                      .Where(p => p.LedTeam.Id == criteria.TeamId)
                                      .Select(GetPlayerMapping())
                                      .ToList();
        }

        #endregion

        #region Mapping

        private static Expression<Func<PlayerEntity, Player>> GetPlayerMapping()
        {
            return p => new Player
                    {
                        Id = p.Id,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        BirthYear = p.BirthYear,
                        Height = p.Height,
                        Weight = p.Weight
                    };
        }

        #endregion
    }
}