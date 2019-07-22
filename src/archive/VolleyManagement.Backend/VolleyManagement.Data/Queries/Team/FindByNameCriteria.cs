namespace VolleyManagement.Data.Queries.Team
{
    using VolleyManagement.Data.Contracts;

    public class FindByNameCriteria : IQueryCriteria
    {
        public string Name { get; set; }
    }
}
