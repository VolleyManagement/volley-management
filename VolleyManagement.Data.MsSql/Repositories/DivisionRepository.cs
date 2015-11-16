namespace VolleyManagement.Data.MsSql.Repositories
{
    using System;
    using System.Data.Entity;

    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.MsSql.Mappers;
    using VolleyManagement.Domain.DivisionAggregate;

    /// <summary>
    /// Defines implementation of the IDivisionRepository contract.
    /// </summary>
    public class DivisionRepository : IDivisionRepository
    {
        private readonly DbSet<DivisionEntity> _divisions;
        private readonly VolleyUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        public DivisionRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _divisions = _unitOfWork.Context.Divisions;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public Contracts.IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Add new Division.
        /// </summary>
        /// <param name="newEntity">Division to add.</param>
        public void Add(Division newEntity)
        {
            var division = new DivisionEntity();
            DomainToDal.Map(division, newEntity);
            _divisions.Add(division);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// Updates division
        /// </summary>
        /// <param name="oldEntity">Division to update</param>
        public void Update(Division oldEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes division by id
        /// </summary>
        /// <param name="id">Division id</param>
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
