namespace VolleyManagement.Data.MsSql.Repositories
{
    using System.Data.Entity;
    using System.Linq;
    using Contracts;
    using Domain.RequestsAggregate;
    using Entities;
    using Exceptions;
    using Mappers;

    /// <summary>
    /// Defines implementation of the IRequestRepository contract.
    /// </summary>
    internal class RequestRepository : IRequestRepository
    {
        private readonly VolleyUnitOfWork _unitOfWork;
        private readonly DbSet<RequestEntity> _dalRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">Instance of class which implements <see cref="IUnitOfWork"/>.</param>
        public RequestRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (VolleyUnitOfWork)unitOfWork;
            _dalRequest = _unitOfWork.Context.Requests;
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        /// <summary>
        /// Adds new game.
        /// </summary>
        /// <param name="newEntity">Request to add.</param>
        public void Add(Request newEntity)
        {
            var newRequestEntity = new RequestEntity();

            DomainToDal.Map(newRequestEntity, newEntity);
            _dalRequest.Add(newRequestEntity);
            _unitOfWork.Commit();
            newEntity.Id = newRequestEntity.Id;
        }

        /// <summary>
        /// Updates specified request.
        /// </summary>
        /// <param name="updatedEntity">Updated request.</param>
        public void Update(Request updatedEntity)
        {
            var requestToUpdate = _dalRequest.SingleOrDefault(t => t.Id == updatedEntity.Id);

            if (requestToUpdate == null)
            {
                throw new ConcurrencyException();
            }

            DomainToDal.Map(requestToUpdate, updatedEntity);
        }

        /// <summary>
        /// Removes request by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the request.</param>
        public void Remove(int id)
        {
            var dalToRemove = new RequestEntity { Id = id };

            _dalRequest.Attach(dalToRemove);
            _dalRequest.Remove(dalToRemove);
        }
    }
}
