namespace VolleyManagement.Data.Queries.Team
{
    using VolleyManagement.Data.Contracts;

    /// <summary>
    /// The find by captain id criteria.
    /// </summary>
    public class FindByCaptainIdCriteria : IQueryCriteria
    {
        public int Id { get; set; }
    }
}