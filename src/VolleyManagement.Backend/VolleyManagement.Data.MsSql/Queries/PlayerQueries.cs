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
    using Data.Queries.Team;

    /// <summary>
    /// Provides Query Object implementation for Player entity
    /// </summary>
    public class PlayerQueries : IQuery<Player, FindByIdCriteria>,
                                 IQuery<int, FindByPlayerCriteria>,
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
            var players = _unitOfWork.Context.Players
                .Where(t => t.Id == criteria.Id)
                .ToList();
            return players.Select(p => GetPlayerMapping(p))
                .SingleOrDefault();
        }

        /// <summary>
        /// Finds Players by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Player"/>. </returns>
        public Player Execute(FindByFullNameCriteria criteria)
        {
            var players = _unitOfWork.Context.Players
                .Where(t => t.FirstName == criteria.FirstName)
                .Where(t => t.LastName == criteria.LastName)
                .ToList();
            return players.Select(p => GetPlayerMapping(p))
                .SingleOrDefault();
        }

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Player"/>. </returns>
        public IQueryable<Player> Execute(GetAllCriteria criteria)
        {
            var players = _unitOfWork.Context.Players.ToList();
            return players.Select(p => GetPlayerMapping(p)).AsQueryable();
        }

        /// <summary>
        /// Finds Tournament by given criteria
        /// </summary>
        /// <param name="criteria"> The criteria. </param>
        /// <returns> The <see cref="Player"/>. </returns>
        public ICollection<Player> Execute(TeamPlayersCriteria criteria)
        {
            var players = _unitOfWork.Context.Players
                .Where(p => p.TeamId == criteria.TeamId)
                .ToList();
            return players.Select(p => GetPlayerMapping(p)).ToList();
        }

        /// <summary>
        /// Find Team by given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>The id of player team</returns>
        public int Execute(FindByPlayerCriteria criteria)
        {
            var players = _unitOfWork.Context.Players
                .Where(t => t.Id == criteria.Id)
                .ToList();
            return players.Select(t => t.TeamId.GetValueOrDefault())
                .SingleOrDefault();
        }

        #endregion

        #region Mapping

        private static Player GetPlayerMapping(PlayerEntity p)
        {
            return new Player(p.Id, p.FirstName, p.LastName, p.BirthYear, p.Height, p.Weight);
        }

        #endregion
    }
}