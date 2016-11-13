namespace VolleyManagement.Data.MsSql.Repositories
{
    using System.Data.Entity;
    using System.Linq;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Exceptions;
    using VolleyManagement.Data.MsSql.Entities;
    using VolleyManagement.Data.MsSql.Mappers;
    using VolleyManagement.Domain.RequestsAggregate;

    /// <summary>
    /// Defines implementation of the IRequestRepository contract.
    /// </summary>
    public class RequestRepository : IRequestRepository
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
        /// <param name="newRequest">Request to add.</param>
        public void Add(Request newRequest)
        {
            var newRequestEntity = new RequestEntity();

            DomainToDal.Map(newRequestEntity, newRequest);
            _dalRequest.Add(newRequestEntity);
            _unitOfWork.Commit();
            newRequest.Id = newRequestEntity.Id;
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
