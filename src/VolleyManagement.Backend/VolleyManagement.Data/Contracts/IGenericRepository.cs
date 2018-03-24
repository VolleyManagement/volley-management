namespace VolleyManagement.Data.Contracts
{
    /// <summary>
    /// Define methods to work with data from store.
    /// </summary>
    /// <typeparam name="T">The type from the store.</typeparam>
    public interface IGenericRepository<in T> 
        where T : class
    {
        /// <summary>
        /// Adds the T type element to the store.
        /// </summary>
        /// <param name="newEntity">Element to add.</param>
        void Add(T newEntity);

        /// <summary>
        /// Update the T type element to the DB.
        /// </summary>
        /// <param name="updatedEntity">Updated element.</param>
        void Update(T updatedEntity);

        /// <summary>
        /// Deletes the element by id from the store.
        /// </summary>
        /// <param name="id">The id of element to remove.</param>
        void Remove(int id);
    }
}
