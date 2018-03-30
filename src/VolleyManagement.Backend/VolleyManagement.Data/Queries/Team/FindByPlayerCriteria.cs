using VolleyManagement.Data.Contracts;
namespace VolleyManagement.Data.Queries.Team
{
    /// <summary>
    /// Find by player id criteria
    /// </summary>
    public class FindByPlayerCriteria : IQueryCriteria
    {
        public FindByPlayerCriteria()
        {

        }

        public FindByPlayerCriteria(int id)
        {
            Id = id;
        }
        /// <summary>
        /// Gets or sets Player ID to search for
        /// </summary>
        public int Id { get; set; }
    }
}
